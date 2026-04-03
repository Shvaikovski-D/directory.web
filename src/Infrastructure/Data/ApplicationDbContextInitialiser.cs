using directory.web.Domain.Constants;
using directory.web.Domain.Entities;
using directory.web.Domain.ValueObjects;
using directory.web.Infrastructure.Identity;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace directory.web.Infrastructure.Data;

public static class InitialiserExtensions
{
    public static async Task InitialiseDatabaseAsync(this WebApplication app)
    {
        using var scope = app.Services.CreateScope();

        var initialiser = scope.ServiceProvider.GetRequiredService<ApplicationDbContextInitialiser>();

        await initialiser.InitialiseAsync();
        await initialiser.SeedAsync();
    }
}

public class ApplicationDbContextInitialiser
{
    private readonly ILogger<ApplicationDbContextInitialiser> _logger;
    private readonly ApplicationDbContext _context;
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly RoleManager<IdentityRole> _roleManager;

    public ApplicationDbContextInitialiser(ILogger<ApplicationDbContextInitialiser> logger, ApplicationDbContext context, UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager)
    {
        _logger = logger;
        _context = context;
        _userManager = userManager;
        _roleManager = roleManager;
    }

    private async Task<IdentityRole> CreateRole(string roleName)
    {
        var role = new IdentityRole(roleName);
        
        if (_roleManager.Roles.All(r => r.Name != role.Name))
        {
            await _roleManager.CreateAsync(role);
        }
        else
        {
            role = await _roleManager.FindByNameAsync(roleName) ?? throw new Exception($"Role '{roleName}' not found.");
        }
        
        return role;
    }

    public async Task InitialiseAsync()
    {
        try
        {
            // See https://jasontaylor.dev/ef-core-database-initialisation-strategies
            //await _context.Database.EnsureDeletedAsync();
            //await _context.Database.EnsureCreatedAsync();
            await _context.Database.MigrateAsync();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An error occurred while initialising the database.");
            throw;
        }
    }

    public async Task SeedAsync()
    {
        try
        {
            await TrySeedAsync();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An error occurred while seeding the database.");
            throw;
        }
    }

    public async Task TrySeedAsync()
    {
        // Default roles
        var administratorRole = await CreateRole(Roles.Administrator);
        var managerRole = await CreateRole(Roles.Manager);

        // Default users
        var administrator = new ApplicationUser {
            UserName = "admin",
            Email = "admin@local.com",
            FirstName = "Антон",
            LastName = "Комаров"
        };

        if (_userManager.Users.All(u => u.UserName != administrator.UserName))
        {
            await _userManager.CreateAsync(administrator, "Password1!");
            if (!string.IsNullOrWhiteSpace(administratorRole.Name))
            {
                await _userManager.AddToRolesAsync(administrator, new [] { administratorRole.Name });
            }
        }

        var manager = new ApplicationUser {
            UserName = "manager",
            Email = "manager@local.com",
            FirstName = "Дарья",
            LastName = "Кириллова"
        };

        if (_userManager.Users.All(u => u.UserName != manager.UserName))
        {
            await _userManager.CreateAsync(manager, "Password1!");
            if (!string.IsNullOrWhiteSpace(managerRole.Name))
            {
                await _userManager.AddToRolesAsync(manager, new [] { managerRole.Name });
            }
        }

        // Default data
        if (!_context.Forklifts.Any())
        {
            _context.Forklifts.Add(new Forklift
            {
                Brand = "Brand 001",
                Number = "A1-B392",
                LoadCapacity = 3.274M,
                IsActive = true,
                Created = DateTimeOffset.UtcNow,
                CreatedBy = administrator.Id,
                LastModified = DateTimeOffset.UtcNow,
                LastModifiedBy = administrator.Id
            });
            _context.Forklifts.Add(new Forklift
            {
                Brand = "Brand 002",
                Number = "B6-V927",
                LoadCapacity = 8.291M,
                IsActive = true,
                Created = DateTimeOffset.UtcNow,
                CreatedBy = administrator.Id,
                LastModified = DateTimeOffset.UtcNow,
                LastModifiedBy = administrator.Id
            });
            await _context.SaveChangesAsync();
        }
        // Seed, if necessary
        if (!_context.TodoLists.Any())
        {
            _context.TodoLists.Add(new TodoList
            {
                Title = "Tasks",
                Colour = Colour.Green,
                Items =
                {
                    new TodoItem { Title = "Make a todo list 📃" },
                    new TodoItem { Title = "Check off the first item ✅" },
                    new TodoItem { Title = "Realise you've already done two things on the list! 🤯"},
                    new TodoItem { Title = "Reward yourself with a nice, long nap 🏆" },
                }
            });

            await _context.SaveChangesAsync();
        }
    }
}

using ContactPro.Data;
using ContactPro.Models;
using Microsoft.AspNetCore.Identity;

namespace ContactPro.Helpers
{
    public class DataSeeder
    {
        private readonly ApplicationDbContext _dbContext;
        private readonly UserManager<AppUser> _userManager;

        private string? appUserId;
        private int contactId;
        private int categoryId;

        public DataSeeder(ApplicationDbContext dbContext, UserManager<AppUser> userManager)
        {
            _dbContext = dbContext;
            _userManager = userManager;
        }

        public async Task SeedDataAsync()
        {
            await SeedUsersAsync();
            await SeedContactsAsync();
            await SeedCategoriesAsync();
            //await SeedCategoryContactAsync();
        }
        private async Task SeedUsersAsync()
        {
            if (!_dbContext.Users.Any())
            {
                //start repeat
                var demoUser = new AppUser()
                {
                    UserName = "demouser@mail.com",
                    Email = "demouser@mail.com",
                    FirstName = "Demo",
                    LastName = "User",
                    EmailConfirmed = true
                };

                await _userManager.CreateAsync(demoUser, "Abc&123!");
                await _dbContext.SaveChangesAsync();

                appUserId = _userManager.Users.FirstOrDefault(u => u.Email == "demouser@mail.com").Id;
                //appUserId = _userManager.FindByEmailAsync("demouser@mail.com");

            }
        }


        private async Task SeedContactsAsync()
        {
            if (!_dbContext.Contacts.Any())
            {
                var contact = new List<Contact>()
                {
                    // start repeat
                    new Contact()
                    {
                        FirstName = "John",
                        LastName = "Dickens",
                        Address1 = "12 Main St.",
                        City = "Concord",
                        States  = ContactPro.Enums.States.NH,
                        ZipCode = 03101,
                        Email = "john@mail.com",
                        AppUserId = appUserId,
                    }
                    // end repeat
                };

                await _dbContext.Contacts.AddRangeAsync(contact);
                await _dbContext.SaveChangesAsync();
                //contactId = _dbContext.Contacts.FirstOrDefault(u => u.Email == "john@mail.com").Id;
            }
        }


        private async Task SeedCategoriesAsync()
        {
            if (!_dbContext.Categories.Any())
            {
                var category = new List<Category>()
                {
                    new Category()
                    {
                        AppUserId = appUserId,
                        Name = "_UnCategorized",
                    },
                    new Category()
                    {
                        Name = "Friend",
                        AppUserId = appUserId
                    },
                    new Category()
                    {
                        Name = "Colleague",
                        AppUserId = appUserId
                    },
                    new Category()
                    {
                        Name = "Vendor",
                        AppUserId = appUserId
                    },
                    new Category()
                    {
                        Name = "Contractor",
                        AppUserId = appUserId
                    },
                    new Category()
                    {
                        Name = "Send Invitation",
                        AppUserId = appUserId
                    }
                };

                await _dbContext.Categories.AddRangeAsync(category);
                await _dbContext.SaveChangesAsync();

                //categoryId = _dbContext.Categories.FirstOrDefault(u => u.Name == "_UnCategorized").Id;
            }
        }




        private async Task SeedCategoryContactAsync()
        {

        }

    }
}

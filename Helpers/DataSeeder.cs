using ContactPro.Data;
using ContactPro.Models;
using Microsoft.AspNetCore.Identity;

namespace ContactPro.Helpers
{
    public class DataSeeder
    {
        private readonly ApplicationDbContext _dbContext;
        private readonly UserManager<AppUser> _userManager;

        private static string appUserId;
        private static int contactId;
        private static int categoryId;

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
            if (_dbContext.Users.Any())
            {
                return;
            }

            //start repeat
            var demoUser = new AppUser()
            {
                UserName = "demouser@mail.com",
                Email = "demouser@mail.com",
                FirstName = "Demo",
                LastName = "User",
                EmailConfirmed = true,
            };

            await _userManager.CreateAsync(demoUser, "Abc&123!");

            appUserId = _dbContext.Users.FirstOrDefault(u => u.Email == "demouser@mail.com").Id;
            // end repeat, the create async will save the changes
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

                contactId = _dbContext.Contacts.FirstOrDefault(u => u.Email == "john@mail.com").Id;
                await _dbContext.Contacts.AddRangeAsync(contact);
                _dbContext.SaveChanges();
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
                        Name = "_UnCategorized",
                        AppUserId = appUserId
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

                categoryId = _dbContext.Categories.FirstOrDefault(u => u.Name == "_UnCategorized").Id;
                await _dbContext.Categories.AddRangeAsync(category);
                _dbContext.SaveChanges();
            }
        }




        private async Task SeedCategoryContactAsync()
        {

        }

    }
}

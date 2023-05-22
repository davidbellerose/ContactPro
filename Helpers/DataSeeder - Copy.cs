using ContactPro.Data;
using ContactPro.Models;
using Microsoft.AspNetCore.Identity;

namespace ContactPro.Helpers
{
    public class DataSeeder1
    {
        private readonly ApplicationDbContext _dbContext;
        private readonly UserManager<AppUser> _userManager;

        private string? appUserId;
        //private int contactId;
        //private int categoryId;

        // create contact new list
        List<Contact> contact = new List<Contact>();
        List<Category> category = new List<Category>();
        //Contact? contactI = new Contact();


        public DataSeeder1(ApplicationDbContext dbContext, UserManager<AppUser> userManager)
        {
            _dbContext = dbContext;
            _userManager = userManager;
        }

        public async Task SeedDataAsync()
        {
            await SeedUsersAsync();
            await SeedContactsAsync();
            await SeedCategoriesAsync();
        }
        private async Task SeedUsersAsync()
        {
            if (!_dbContext.Users.Any())
            {
                var demoUser = new AppUser()
                {
                    UserName = "demouser@mail.com",
                    Email = "demouser@mail.com",
                    FirstName = "Demo",
                    LastName = "User",
                    EmailConfirmed = true
                };

                await _userManager.CreateAsync(demoUser, "Abc&123!");
            }
            await _dbContext.SaveChangesAsync();

            //appUserId = _userManager.Users.FirstOrDefault(u => u.Email == "demouser@mail.com").Id;
            appUserId = (await _userManager.FindByEmailAsync("demouser@mail.com")).Id;
        }



        private async Task SeedContactsAsync()
        {
            try
            {
                if (!_dbContext.Contacts.Any())
                {
                    contact = new List<Contact>()
                    {
                        new Contact()
                        {
                            FirstName = "John",
                            LastName = "Dickens",
                            Address1 = "12 Main St.",
                            City = "Concord",
                            States  = ContactPro.Enums.States.NH,
                            ZipCode = 03101,
                            Email = "john@mail.com",
                            BirthDate = new DateTime(1994,8,20),
                            PhoneNumber = "(212)345-8587",
                            DateCreated = new DateTime(2023,5,20),
                            AppUserId = appUserId,
                        }
                    };

                }
                await _dbContext.Contacts.AddRangeAsync(contact);
                await _dbContext.SaveChangesAsync();
                //contactId = _dbContext.Contacts.FirstOrDefault(u => u.Email == "john@mail.com").Id;
                //contactI = await _dbContext.Contacts.FindAsync(contactId);
                //store whole conact instance at the top of the class
                //contacts.categories.add vendor, 
            }
            catch (Exception ex)
            {
                Console.WriteLine("*************  ERROR  *************");
                Console.WriteLine("Error Seeding Project Priorities.");
                Console.WriteLine(ex.Message);
                Console.WriteLine("***********************************");
                throw;
            }
        }


        private async Task SeedCategoriesAsync()
        {
            if (!_dbContext.Categories.Any())
            {
                category = new List<Category>()
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
            }
            await _dbContext.Categories.AddRangeAsync(category);
            await _dbContext.SaveChangesAsync();

            // update the contacts
            //var contactId = (await _dbContext.Fin("demouser@mail.com")).Id;
            //var contactId = 1;
            //Category? category = await _dbContext.Categories.FindAsync(categoryId);
            //category.Add(contactI);
            //await _dbContext.Categories.Add(contactI);
            //await _dbContext.Categories.Add(contactId).ToString();

            //categoryId = _dbContext.Categories.FirstOrDefault(u => u.Name == "_UnCategorized").Id;

            //foreach (int categoryId in CategoryList)
            //{
            //    await _addressBookService.AddContactToCategoryAsync(categoryId, contact.Id);
            //}
        }
    }
}

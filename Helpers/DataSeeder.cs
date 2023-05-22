using ContactPro.Data;
using ContactPro.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace ContactPro.Helpers
{
    public class DataSeeder
    {
        public static async Task InitializeAsync(IServiceProvider serviceProvider)
        {
            var context = new ApplicationDbContext(serviceProvider.GetRequiredService<DbContextOptions<ApplicationDbContext>>());

            var userManager = serviceProvider.GetRequiredService<UserManager<AppUser>>();

            string? appUserId;
            int? contactId1;
            string? contactId2;

            //Add this line
            var contactItem = new Contact();


            List<Contact> contact = new List<Contact>();
            List<Category> category = new List<Category>();

            using (context)
            {
                if (!context.Users.Any())
                {
                    var demoUser = new AppUser()
                    {
                        UserName = "demouser@mail.com",
                        Email = "demouser@mail.com",
                        FirstName = "Demo",
                        LastName = "User",
                        EmailConfirmed = true
                    };
                    await userManager.CreateAsync(demoUser, "Abc&123!");
                }
                await context.SaveChangesAsync();
                appUserId = (await userManager.FindByEmailAsync("demouser@mail.com")).Id;

                try
                {
                    if (!context.Contacts.Any())
                    {
                        contactItem = new Contact()
                        {
                            FirstName = "Anna",
                            LastName = "Bickford",
                            Address1 = "12 Main St.",
                            City = "Concord",
                            States = ContactPro.Enums.States.NH,
                            ZipCode = 03101,
                            Email = "anna@mail.com",
                            BirthDate = new DateTime(1994, 8, 20),
                            PhoneNumber = "(212)345-8587",
                            DateCreated = new DateTime(2023, 5, 20),
                            AppUserId = appUserId,
                        };

                        contact.Add(contactItem);
                        //contactId1 = context.Contacts.FirstOrDefault(p => p.Email == "anna@mail.com").Id;

                        contactItem = new Contact()
                        {
                            FirstName = "Ian",
                            LastName = "Bartow",
                            Address1 = "245 Elm St.",
                            City = "Phoenix",
                            States = ContactPro.Enums.States.AZ,
                            ZipCode = 32854,
                            Email = "ian@mail.com",
                            BirthDate = new DateTime(1994, 8, 20),
                            PhoneNumber = "(514)121-7845",
                            DateCreated = new DateTime(2023, 5, 20),
                            AppUserId = appUserId,
                        };
                        contact.Add(contactItem);
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine("*************  ERROR  *************");
                    Console.WriteLine("Error Seeding Contacts.");
                    Console.WriteLine(ex.Message);
                    Console.WriteLine("***********************************");
                    throw;
                }

                if (!context.Categories.Any())
                {
                    category = new List<Category>()
                    {
                        new Category()
                        {
                            AppUserId = appUserId,
                            Name = "_UnCategorized",
                            //Contacts = contactId1
                        },
                        new Category()
                        {
                            Name = "Friend",
                            AppUserId = appUserId,
                            Contacts = contact
                        },
                        new Category()
                        {
                            Name = "Colleague",
                            AppUserId = appUserId,
                            Contacts = contact
                        },
                        new Category()
                        {
                            Name = "Vendor",
                            AppUserId = appUserId,
                            Contacts = contact
                        },
                        new Category()
                        {
                            Name = "Contractor",
                            AppUserId = appUserId,
                            Contacts = contact
                        },
                        new Category()
                        {
                            Name = "Send Invitation",
                            AppUserId = appUserId,
                            Contacts = contact
                        }
                    };
                }

                //Operate the database at the end
                contactItem.Categories = category;
                await context.Contacts.AddRangeAsync(contact);
                await context.Categories.AddRangeAsync(category);
                await context.SaveChangesAsync();
            }
        }
    }
}

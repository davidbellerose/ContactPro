using ContactPro.Data;
using ContactPro.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics.Contracts;

namespace ContactPro.Helpers
{
    public class DataSeeder
    {
        public static async Task InitializeAsync(IServiceProvider serviceProvider)
        {
            var context = new ApplicationDbContext(serviceProvider.GetRequiredService<DbContextOptions<ApplicationDbContext>>());

            var userManager = serviceProvider.GetRequiredService<UserManager<AppUser>>();

            string? appUserId;

            List<Contact> contact = new List<Contact>();
            List<Category> category = new List<Category>();
            Category friendCategory = new();
            Category uncategorizedCategory = new();
            Category colleagueCategory = new();
            Category vendorCategory = new();
            Category contractorCategory = new();
            Category inviteCategory = new();

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

                // start categories **************************************
                if (!context.Categories.Any(c => c.Name == "_UnCategorized"))
                {
                    uncategorizedCategory = new()
                    {
                        Name = "_UnCategorized",
                        AppUserId = appUserId,
                    };

                    context.Add(uncategorizedCategory);
                }

                if (!context.Categories.Any(c => c.Name == "Friend"))
                {
                    friendCategory = new()
                    {
                        Name = "Friend",
                        AppUserId = appUserId,
                    };

                    context.Add(friendCategory);
                }

                if (!context.Categories.Any(c => c.Name == "Vendor"))
                {
                    vendorCategory = new()
                    {
                        Name = "Vendor",
                        AppUserId = appUserId,
                    };

                    context.Add(vendorCategory);
                }

                if (!context.Categories.Any(c => c.Name == "Contractor"))
                {
                    contractorCategory = new()
                    {
                        Name = "Contractor",
                        AppUserId = appUserId,
                    };

                    context.Add(vendorCategory);
                }

                if (!context.Categories.Any(c => c.Name == "Colleague"))
                {
                    colleagueCategory = new()
                    {
                        Name = "Colleague",
                        AppUserId = appUserId,
                    };

                    context.Add(vendorCategory);
                }

                if (!context.Categories.Any(c => c.Name == "Invite"))
                {
                    inviteCategory = new()
                    {
                        Name = "Invite",
                        AppUserId = appUserId,
                    };

                    context.Add(vendorCategory);
                }


                // start contacts ******************************************
                if (!context.Contacts.Any(c => c.Email == "anna@mail.com"))
                {
                    Contact contact1 = new()
                    {
                        FirstName = "Anna",
                        LastName = "Bickford",
                        Address1 = "12 Main St.",
                        City = "Concord",
                        States = ContactPro.Enums.States.NH,
                        ZipCode = 03101,
                        Email = "anna@mailinator.com",
                        BirthDate = new DateTime(1994, 8, 20),
                        PhoneNumber = "(212)345-8587",
                        DateCreated = new DateTime(2023, 5, 20),
                        AppUserId = appUserId,
                    };

                    contact1.Categories.Add(uncategorizedCategory);
                    context.Add(contact1);
                }

                if (!context.Contacts.Any(c => c.Email == "ian@mail.com"))
                {
                    Contact contact2 = new()
                    {
                        FirstName = "Ian",
                        LastName = "Bartow",
                        Address1 = "245 Elm St.",
                        City = "Phoenix",
                        States = ContactPro.Enums.States.AZ,
                        ZipCode = 32854,
                        Email = "ian@mailinator.com",
                        BirthDate = new DateTime(1994, 8, 20),
                        PhoneNumber = "(514)121-7845",
                        DateCreated = new DateTime(2023, 5, 20),
                        AppUserId = appUserId,
                    };

                    contact2.Categories.Add(friendCategory);
                    contact2.Categories.Add(vendorCategory);
                    context.Add(contact2);
                }

                if (!context.Contacts.Any(c => c.Email == "jenna@mail.com"))
                {
                    Contact contact3 = new()
                    {
                        FirstName = "Jenna",
                        LastName = "Cooper",
                        Address1 = "3333 Broadway",
                        Address2 = "Apt 17D",
                        City = "New York",
                        States = ContactPro.Enums.States.NY,
                        ZipCode = 10031,
                        Email = "jenna@mailinator.com",
                        BirthDate = new DateTime(1999, 4, 02),
                        PhoneNumber = "(212)641-2322",
                        DateCreated = new DateTime(2023, 5, 20),
                        AppUserId = appUserId,
                    };

                    contact3.Categories.Add(colleagueCategory);
                    context.Add(contact3);
                }

                if (!context.Contacts.Any(c => c.Email == "jeri@mail.com"))
                {
                    Contact contact4 = new()
                    {
                        FirstName = "Jeri",
                        LastName = "Nolan",
                        Address1 = "P.O. Box 342",
                        City = "Boston",
                        States = ContactPro.Enums.States.MA,
                        ZipCode = 02134,
                        Email = "jeri@mailinator.com",
                        BirthDate = new DateTime(1982, 6, 30),
                        PhoneNumber = "(617)218-8745",
                        DateCreated = new DateTime(2023, 5, 20),
                        AppUserId = appUserId,
                    };

                    contact4.Categories.Add(colleagueCategory);
                    contact4.Categories.Add(inviteCategory);
                    context.Add(contact4);
                }

                if (!context.Contacts.Any(c => c.Email == "john@mail.com"))
                {
                    Contact contact5 = new()
                    {
                        FirstName = "John",
                        LastName = "Carter",
                        Address1 = "P.O. Box 342",
                        City = "Boston",
                        States = ContactPro.Enums.States.MA,
                        ZipCode = 02134,
                        Email = "john@mailinator.com",
                        BirthDate = new DateTime(1982, 6, 30),
                        PhoneNumber = "(617)218-8745",
                        DateCreated = new DateTime(2023, 5, 20),
                        AppUserId = appUserId,
                    };

                    contact5.Categories.Add(contractorCategory);
                    context.Add(contact5);
                }

                if (!context.Contacts.Any(c => c.Email == "liz@mail.com"))
                {
                    Contact contact6 = new()
                    {
                        FirstName = "Liz",
                        LastName = "Dickens",
                        Address1 = "615 Granite Rd.",
                        Address2 = "Suite 555",
                        City = "St. Louis",
                        States = ContactPro.Enums.States.MI,
                        ZipCode = 62351,
                        Email = "liz@mailinator.com",
                        BirthDate = new DateTime(2001, 9, 19),
                        PhoneNumber = "(485)454-9565",
                        DateCreated = new DateTime(2023, 5, 20),
                        AppUserId = appUserId,
                    };

                    contact6.Categories.Add(contractorCategory);
                    context.Add(contact6);
                }

                if (!context.Contacts.Any(c => c.Email == "mateo@mail.com"))
                {
                    Contact contact7 = new()
                    {
                        FirstName = "Mateo",
                        LastName = "Gonzalez",
                        Address1 = "3454 Sunset Blvd.",
                        Address2 = "Apt. 876",
                        City = "Los Angeles",
                        States = ContactPro.Enums.States.CA,
                        ZipCode = 45123,
                        Email = "mateo@mailinator.com",
                        BirthDate = new DateTime(2005, 2, 05),
                        PhoneNumber = "(412)565-2354",
                        DateCreated = new DateTime(2023, 5, 20),
                        AppUserId = appUserId,
                    };

                    contact7.Categories.Add(friendCategory);
                    contact7.Categories.Add(inviteCategory);
                    context.Add(contact7);
                }

                if (!context.Contacts.Any(c => c.Email == "natya@mail.com"))
                {
                    Contact contact8 = new()
                    {
                        FirstName = "Natya",
                        LastName = "Agapof",
                        Address1 = "234 17th St.",
                        Address2 = "Unit 17B",
                        City = "Salt Lake City",
                        States = ContactPro.Enums.States.UT,
                        ZipCode = 84521,
                        Email = "natya@mailinator.com",
                        BirthDate = new DateTime(2011, 11, 23),
                        PhoneNumber = "(325)485-6485",
                        DateCreated = new DateTime(2023, 5, 20),
                        AppUserId = appUserId,
                    };

                    contact8.Categories.Add(uncategorizedCategory);
                    context.Add(contact8);
                }

                if (!context.Contacts.Any(c => c.Email == "ronan@mail.com"))
                {
                    Contact contact9 = new()
                    {
                        FirstName = "Ronan",
                        LastName = "Masters",
                        Address1 = "117 Market St.",
                        City = "Phoenix",
                        States = ContactPro.Enums.States.AZ,
                        ZipCode = 91546,
                        Email = "ronan@mailinator.com",
                        BirthDate = new DateTime(1989, 5, 18),
                        PhoneNumber = "(646)215-1324",
                        DateCreated = new DateTime(2023, 5, 20),
                        AppUserId = appUserId,
                    };

                    contact9.Categories.Add(uncategorizedCategory);
                    context.Add(contact9);
                }


                if (!context.Contacts.Any(c => c.Email == "sarah@mail.com"))
                {
                    Contact contact10 = new()
                    {
                        FirstName = "Sarah",
                        LastName = "Cohen",
                        Address1 = "341 Park Way",
                        City = "Orlando",
                        States = ContactPro.Enums.States.FL,
                        ZipCode = 30915,
                        Email = "sarah@mailinator.com",
                        BirthDate = new DateTime(2002, 10, 02),
                        PhoneNumber = "(723)614-8217",
                        DateCreated = new DateTime(2023, 5, 20),
                        AppUserId = appUserId,
                    };

                    contact10.Categories.Add(vendorCategory);
                    context.Add(contact10);
                }


                if (!context.Contacts.Any(c => c.Email == "sonya@mail.com"))
                {
                    Contact contact11 = new()
                    {
                        FirstName = "Sonya",
                        LastName = "Jackson",
                        Address1 = "1021 Sepulveda Ave.",
                        City = "Richmond",
                        States = ContactPro.Enums.States.VA,
                        ZipCode = 46137,
                        Email = "sonya@mailinator.com",
                        BirthDate = new DateTime(1997, 03, 07),
                        PhoneNumber = "(515)145-9137",
                        DateCreated = new DateTime(2023, 5, 20),
                        AppUserId = appUserId,
                    };

                    contact11.Categories.Add(friendCategory);
                    context.Add(contact11);
                }


                if (!context.Contacts.Any(c => c.Email == "victor@mail.com"))
                {
                    Contact contact12 = new()
                    {
                        FirstName = "Victor",
                        LastName = "Mansfield",
                        Address1 = "1342 Park Ave.",
                        Address2 = "Suite 32",
                        City = "New York",
                        States = ContactPro.Enums.States.NY,
                        ZipCode = 10232,
                        Email = "victor@mailinator.com",
                        BirthDate = new DateTime(1989, 08, 12),
                        PhoneNumber = "(212)998-4651",
                        DateCreated = new DateTime(2023, 5, 20),
                        AppUserId = appUserId,
                    };

                    contact12.Categories.Add(vendorCategory);
                    context.Add(contact12);
                }



                // after adding all your contacts/categories, just save once
                //await context.SaveChangesAsync();
            }
        }
    }
}

using ContactPro.Data;
using ContactPro.Enums;
using ContactPro.Models;
using ContactPro.Models.ViewModels;
using ContactPro.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;


namespace ContactPro.Controllers
{
    [Authorize]
    public class ContactsController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<AppUser> _userManager;
        private readonly IImageService _imageService;
        private readonly IAddressBookService _addressBookService;
        private readonly IEmailSender _emailSender;

        public ContactsController(ApplicationDbContext context, UserManager<AppUser> userManager, IImageService imageService, IAddressBookService addressBookService, IEmailSender emailSender)
        {
            _context = context;
            _userManager = userManager;
            _imageService = imageService;
            _addressBookService = addressBookService;
            _emailSender = emailSender;
        }

        // GET: Contacts
        public IActionResult Index(int categoryId, string swalMessage = null)
        {
            // old index shows all contacts
            //var applicationDbContext = _context.Contacts.Include(c => c.AppUser);
            //return view();


            // the below filters contacts by the logged in user
            //List<Contact> contacts = new();
            ViewData["SwalMessage"] = swalMessage;

            var contacts = new List<Contact>();

            string appUserId = _userManager.GetUserId(User);

            AppUser? appUser = _context.Users
                                      .Include(c => c.Contacts)
                                      .ThenInclude(c => c.Categories)
                                      .FirstOrDefault(u => u.Id == appUserId);

            var categories = appUser?.Categories;

            if (categoryId == 0)
            {
                contacts = appUser?.Contacts.OrderBy(c => c.LastName)
                                            .ThenBy(c => c.FirstName)
                                            .ToList();
            }
            else
            {
                if (appUser?.Categories.FirstOrDefault(c => c.Id == categoryId) is null)
                {
                    return RedirectToAction("EmptyContact", "Contacts");
                }
                contacts = appUser?.Categories.FirstOrDefault(c => c.Id == categoryId)
                                             .Contacts
                                             .OrderBy(c => c.LastName)
                                             .ThenBy(c => c.FirstName)
                                             .ToList();
            }

            //contacts = appUser.Contacts.OrderBy(c => c.LastName).ThenBy(c => c.FirstName).ToList();

            //ViewData["CategoryId"] = new SelectList(categories, "Id", "Name");
            ViewData["CategoryId"] = new SelectList(categories.OrderBy(c=>c.Name), "Id", "Name", categoryId);

            return View(contacts);
        }

        public IActionResult SearchContacts(string searchString)
        {
            string appUserId = _userManager.GetUserId(User);

            var contacts = new List<Contact>();

            AppUser? appUser = _context.Users
                                      .Include(c => c.Contacts)
                                      .ThenInclude(c => c.Categories)
                                      .FirstOrDefault(u => u.Id == appUserId);

            if (string.IsNullOrEmpty(searchString))
            {
                contacts = appUser?.Contacts.OrderBy(c => c.LastName)
                                             .ThenBy(c => c.FirstName)
                                             .ToList();
            }
            else
            {
                contacts = appUser?.Contacts.Where(c => c.FullName!.ToLower().Contains(searchString.ToLower()))
                                             .OrderBy(c => c.LastName)
                                             .ThenBy(c => c.FirstName)
                                             .ToList();
                if (contacts.Count == 0)
                {
                    return RedirectToAction("EmptyContact", "Contacts");
                }
            }
            ViewData["CategoryId"] = new SelectList(appUser.Categories, "Id", "Name", 0);

            return View(nameof(Index), contacts);

        }


        public IActionResult EmptyContact()
        {
            return View();
        }

        public async Task<IActionResult> EmailContact(int id)
        {

            string appUserId = _userManager.GetUserId(User);
            Contact? contact = await _context.Contacts.Where(c => c.Id == id && c.AppUserId == appUserId).FirstOrDefaultAsync();

            if (contact == null)
            {
                return NotFound();
            }


            EmailData emailData = new EmailData()
            {
                EmailAddress = contact.Email,
                FirstName = contact.FirstName,
                LastName = contact.LastName
            };

            EmailContactViewModel model = new EmailContactViewModel()
            {
                Contact = contact,
                EmailData = emailData
            };

            return View(model);

        }

        [HttpPost]
        public async Task<IActionResult> EmailContact(EmailContactViewModel ecvm)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    await _emailSender.SendEmailAsync(ecvm.EmailData.EmailAddress, ecvm.EmailData.Subject, ecvm.EmailData.Body);
                    return RedirectToAction("Index", "Contacts", new { swalMessage = "Success: Email Sent." });
                }
                catch (Exception)
                {
                    return RedirectToAction("Index", "Contacts", new { swalMessage = "Error: Email Send Failed." });
                    throw;
                }
            }
            return View(ecvm);
        }



        // GET: Contacts/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Contacts == null)
            {
                return NotFound();
            }

            var contact = await _context.Contacts
                .Include(c => c.AppUser)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (contact == null)
            {
                return NotFound();
            }

            return View(contact);
        }

        // GET: Contacts/Create
        public async Task<IActionResult> Create()
        {
            string appUserId = _userManager.GetUserId(User);

            ViewData["StatesList"] = new SelectList(Enum.GetValues(typeof(States)).Cast<States>().ToList());

            ViewData["CategoryList"] = new MultiSelectList(await _addressBookService.GetUserCategoriesAsync(appUserId), "Id", "Name");

            return View();
        }

        // POST: Contacts/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,FirstName,LastName,BirthDate,Address1,Address2,City,States,ZipCode,Email,PhoneNumber,ImageFile")] Contact contact
            , List<int> CategoryList)
        {
            ModelState.Remove("AppUserId");  //removed from model so it doesn't try to validate it.

            if (ModelState.IsValid)
            {
                contact.AppUserId = _userManager.GetUserId(User); // the user id will be taken from the logged in user
                contact.DateCreated = DateTime.SpecifyKind(DateTime.Now, DateTimeKind.Utc);

                if (contact.BirthDate != null)
                {
                    contact.BirthDate = DateTime.SpecifyKind(contact.BirthDate.Value, DateTimeKind.Utc);
                }

                if (contact.ImageFile != null)
                {
                    contact.ImageData = await _imageService.ConvertFileToByteArrayAsync(contact.ImageFile);
                    contact.ImageType = contact.ImageFile.ContentType;
                }

                _context.Add(contact);
                await _context.SaveChangesAsync();


                if (CategoryList.Count == 0)
                {
                    int categoryId = 1;
                    await _addressBookService.AddContactToCategoryAsync(categoryId, contact.Id);
                    return RedirectToAction(nameof(Index));
                }

                foreach (int categoryId in CategoryList)
                {
                    await _addressBookService.AddContactToCategoryAsync(categoryId, contact.Id);
                }

                return RedirectToAction(nameof(Index));
            }
            return RedirectToAction(nameof(Index));
        }

        // GET: Contacts/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Contacts == null)
            {
                return NotFound();
            }

            string? appUserId = _userManager.GetUserId(User);

            //var contact = await _context.Contacts.FindAsync(id);

            var contact = await _context.Contacts.Where(c => c.Id == id && c.AppUserId == appUserId).FirstOrDefaultAsync();

            if (contact == null)
            {
                return NotFound();
            }

            ViewData["StatesList"] = new SelectList(Enum.GetValues(typeof(States)).Cast<States>().ToList());


            ViewData["CategoryList"] = new MultiSelectList(await _addressBookService.GetUserCategoriesAsync(appUserId), "Id", "Name", await _addressBookService.GetContactCategoryIdsAsync(contact.Id));

            return View(contact);
        }

        // POST: Contacts/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,AppUserId,FirstName,LastName,BirthDate,Address1,Address2,City,States,ZipCode,Email,PhoneNumber,DateCreated,ImageData,ImageType,ImageFile")] Contact contact, List<int> CategoryList)
        {
            if (id != contact.Id)
            {
                return NotFound();
            }


            if (ModelState.IsValid)
            {
                try
                {
                    contact.DateCreated = DateTime.SpecifyKind(contact.DateCreated, DateTimeKind.Utc);

                    if (contact.BirthDate != null)
                    {
                        contact.BirthDate = DateTime.SpecifyKind(contact.BirthDate.Value, DateTimeKind.Utc);
                    }

                    if (contact.ImageFile != null)
                    {
                        contact.ImageData = await _imageService.ConvertFileToByteArrayAsync(contact.ImageFile);
                        contact.ImageType = contact.ImageFile.ContentType;
                    }

                    _context.Update(contact);
                    await _context.SaveChangesAsync();

                    List<Category> oldCategories = (await _addressBookService.GetContactCategoriesAsync(contact.Id)).ToList();

                    foreach (var category in oldCategories)
                    {
                        await _addressBookService.RemoveContactFromCategoryAsync(category.Id, contact.Id);
                    }

                    foreach (int categoryId in CategoryList)
                    {
                        await _addressBookService.AddContactToCategoryAsync(categoryId, contact.Id);
                    }
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ContactExists(contact.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            ViewData["AppUserId"] = new SelectList(_context.Users, "Id", "Id", contact.AppUserId);
            return View(contact);
        }

        // GET: Contacts/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Contacts == null)
            {
                return NotFound();
            }

            string appUserId = _userManager.GetUserId(User);

            var contact = await _context.Contacts.FirstOrDefaultAsync(m => m.Id == id && m.AppUserId == appUserId);


            if (contact == null)
            {
                return NotFound();
            }

            return View(contact);
        }

        // POST: Contacts/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {


            string appUserId = _userManager.GetUserId(User);

            var contact = await _context.Contacts.FirstOrDefaultAsync(m => m.Id == id && m.AppUserId == appUserId);



            if (contact != null)
            {
                _context.Contacts.Remove(contact);
                await _context.SaveChangesAsync();
            }

            return RedirectToAction(nameof(Index));
        }

        private bool ContactExists(int id)
        {
            return (_context.Contacts?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}

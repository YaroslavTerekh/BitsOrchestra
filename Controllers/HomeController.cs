using ContactManager.Models;
using ContactManager.Repositories.Abstractions;
using ContactManager.RequestModels;
using ContactManager.Services.Abstractions;
using Microsoft.AspNetCore.Mvc;

namespace ContactManager.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IContactsRepository _contactsRepository;
        private readonly ICSVConverter _converter;

        public HomeController(ILogger<HomeController> logger, IContactsRepository contactsRepository, ICSVConverter converter)
        {
            _logger = logger;
            _contactsRepository = contactsRepository;
            _converter = converter;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var contacts = await _contactsRepository.GetContactsAsync();

            return View();
        }

        [HttpGet("allcontacts")]
        public async Task<IActionResult> Contacts()
        {
            var contacts = await _contactsRepository.GetContactsAsync();

            return Ok(contacts);
        }

        [HttpGet("contact/create")]
        public IActionResult CreateContact()
        {
            return View("Upload");
        }

        [HttpPost("contact/create")]
        public async Task<IActionResult> CreateContact(
            IFormFile file
        )
        {
            var requests = _converter.ConvertCSVToJson(file is not null ? file : HttpContext.Request.Form.Files[0]);

            foreach (var request in requests)
            {
                var valid = TryValidateModel(request);
            }
            if (requests is not null && ModelState.IsValid)
            {
                foreach (var request in requests)
                {
                    var contactId = await _contactsRepository.CreateContactAsync(request);
                }

                return RedirectToAction(nameof(Index));
            }

            return View("Upload");
        }

        [HttpPost("contact/{id:guid}/modify")]
        public async Task<IActionResult> ModifyContact(
            [FromRoute] Guid id,
            [FromForm] Contact contact
        )
        {
            if (ModelState.IsValid)
            {
                var request = new CreateOrModifyContact<DateTime>
                {
                    Name = contact.Name,
                    DateOfBirth = contact.DateOfBirth,
                    Married = contact.Married,
                    Phone = contact.Phone,
                    Salary = contact.Salary
                };

                var contactId = await _contactsRepository.ModifyContactAsync(id, request);

                return NoContent();
            }

            return BadRequest(ModelState.Select(x => x.Value.Errors)
                           .Where(y => y.Count > 0)
                           .ToList());
        }

        [HttpDelete("{id:guid}")]
        public async Task<IActionResult> DeleteContact(
            [FromRoute] Guid id
        )
        {
            await _contactsRepository.DeleteContactAsync(id);

            return NoContent();
        }
    }
}
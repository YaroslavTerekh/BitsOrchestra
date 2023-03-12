using ContactManager.DatabaseConnection;
using ContactManager.Models;
using ContactManager.Repositories.Abstractions;
using ContactManager.RequestModels;
using Microsoft.EntityFrameworkCore;

namespace ContactManager.Repositories.Realizations
{
    public class ContactsRepository : IContactsRepository
    {
        private readonly DataContext _context;

        public ContactsRepository(DataContext context)
        {
            _context = context;
        }

        public async Task<Guid> CreateContactAsync(CreateOrModifyContact<string> request)
        {
            var contact = new Contact
            {
                Name = request.Name,
                DateOfBirth = DateTime.Parse(request.DateOfBirth),
                Married = request.Married,
                Phone = request.Phone,
                Salary = request.Salary
            };

            await _context.Contacts.AddAsync(contact);
            await _context.SaveChangesAsync();

            return contact.Id;
        }

        public async Task DeleteContactAsync(Guid id)
        {
            var contact = await _context.Contacts
                .FirstOrDefaultAsync(t => t.Id == id);

            _context.Contacts.Remove(contact);
            await _context.SaveChangesAsync();
        }

        public async Task<Contact> GetContactAsync(Guid id)
        {
            return await _context.Contacts.FirstOrDefaultAsync(t => t.Id == id);
        }

        public async Task<List<Contact>> GetContactsAsync()
        {
            var contacts = await _context.Contacts.ToListAsync();

            return contacts;
        }

        public async Task<Guid> ModifyContactAsync(Guid id, CreateOrModifyContact<DateTime> request)
        {
            var contact = await _context.Contacts
                .FirstOrDefaultAsync(t => t.Id == id);

            contact.Name = request.Name;
            contact.Salary = request.Salary;
            contact.Phone = request.Phone;
            contact.DateOfBirth = request.DateOfBirth;
            contact.Married = request.Married;

            await _context.SaveChangesAsync();

            return id;
        }
    }
}

using ContactManager.Models;
using ContactManager.RequestModels;

namespace ContactManager.Repositories.Abstractions
{
    public interface IContactsRepository
    {
        public Task<List<Contact>> GetContactsAsync();

        public Task<Contact> GetContactAsync(Guid id);

        public Task<Guid> CreateContactAsync(CreateOrModifyContact<string> request);

        public Task<Guid> ModifyContactAsync(Guid id, CreateOrModifyContact<DateTime> request);

        public Task DeleteContactAsync(Guid id);
    }
}

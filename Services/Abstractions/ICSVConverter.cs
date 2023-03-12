using ContactManager.RequestModels;

namespace ContactManager.Services.Abstractions
{
    public interface ICSVConverter
    {
        public List<CreateOrModifyContact<string>> ConvertCSVToJson(IFormFile file);
    }
}

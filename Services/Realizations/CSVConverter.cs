using ContactManager.RequestModels;
using ContactManager.Services.Abstractions;
using CsvHelper;
using System.Globalization;

namespace ContactManager.Services.Realizations
{
    public class CSVConverter : ICSVConverter
    {
        public List<CreateOrModifyContact<string>> ConvertCSVToJson(IFormFile file)
        {
            if (file.ContentType == "text/csv")
            {
                using (var reader = new StreamReader(file.OpenReadStream()))
                using (var csv = new CsvReader(reader, CultureInfo.InvariantCulture))
                {
                    var records = csv.GetRecords<CreateOrModifyContact<string>>().ToList();

                    return records;
                }
            }

            return null;
        }
    }
}

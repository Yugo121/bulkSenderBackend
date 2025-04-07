using Application.Models.DTO_s;

namespace Application.Interfaces
{
    public interface IProductImportService
    {
        Task ImportAsync(CsvImportRequest importRequest, CancellationToken cancellationToken);
    }
}

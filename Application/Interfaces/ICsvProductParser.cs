using Application.Models.DTO_s;
using Application.Models.DTOs;

namespace Application.Interfaces
{
    public interface ICsvProductParser
    {
        Task<List<ProductDTO>> ParseCsv(CsvImportRequest import);
    }
}

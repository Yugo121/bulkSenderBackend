using Application.Interfaces;
using Application.Models.DTOs;
using Application.Services;
using Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Models.Commands.ProductsCommands
{
    public class ImportCsvHandler : IRequestHandler<ImportCsvCommand>
    {
        private readonly IProductImportService _productImportService;

        public ImportCsvHandler(IProductImportService productImportService)
        {
            _productImportService = productImportService;
        }
            
        public async Task Handle(ImportCsvCommand request, CancellationToken cancellationToken)
        {

            try
            {
                await _productImportService.ImportAsync(request.import, cancellationToken);

            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred while importing CSV data.", ex);
            }

            return;
        }
    }
}

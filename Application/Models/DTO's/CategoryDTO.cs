using Application.Models.DTO_s;
using Domain.Entities;
using System.Text.Json.Serialization;

namespace Application.Models.DTOs
{
    public class CategoryDTO
    {
        public Guid Id { get; set; }
        public int BaselinkerId { get; set; }
        public string BaselinkerName { get; set; }
        public List<CategoryAliasDTO> Aliases { get; set; }

        public CategoryDTO() {}
        public CategoryDTO(Category category)
        {
            Id = category.Id;
            BaselinkerName = category.BaselinkerName;
            BaselinkerId = category.BaselinkerId;
            Aliases = category.Aliases.Select(Alias => new CategoryAliasDTO
            {
                Id = Alias.Id,
                Name = Alias.Name,
                CategoryId = Alias.CategoryId
            }).ToList();
        }
    }
}

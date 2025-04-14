using Domain.Entities;
using System.Text.Json.Serialization;

namespace Application.Models.DTOs
{
    public class CategoryDTO
    {
        [JsonIgnore]
        public Guid Id { get; set; }
        public int BaselinkerId { get; set; }
        public string Name { get; set; }

        public CategoryDTO() {}
        public CategoryDTO(Category category)
        {
            Id = category.Id;
            Name = category.Name;
        }
    }
}

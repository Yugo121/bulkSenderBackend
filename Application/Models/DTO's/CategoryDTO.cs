using Domain.Entities;

namespace Application.Models.DTOs
{
    public class CategoryDTO
    {
        public int Id { get; set; }
        public string Name { get; set; }

        public CategoryDTO(Category category)
        {
            Id = category.Id;
            Name = category.Name;
        }
    }
}

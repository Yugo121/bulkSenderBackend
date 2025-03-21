using Domain.Entities;

namespace Application.Models.DTOs
{
    public class CategoryDTO
    {
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

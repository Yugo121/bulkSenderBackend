using Domain.Entities;

namespace Application.Models.DTO_s
{
    public class CategoryAliasDTO
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public Guid CategoryId { get; set; }

        public CategoryAliasDTO() { }
        public CategoryAliasDTO(CategoryAlias categoryAlias)
        {
            Id = categoryAlias.Id;
            Name = categoryAlias.Name;
            CategoryId = categoryAlias.CategoryId;
        }
    }
}

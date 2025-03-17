using Domain.Entities;

namespace Application.Models.DTOs
{
    public class PropertyDTO
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Value { get; set; }

        public PropertyDTO(Property property)
        {
            Id = property.Id;
            Name = property.Name;
            Value = property.Value;
        }
    }
}

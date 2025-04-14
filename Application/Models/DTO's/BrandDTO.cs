using Domain.Entities;
using System.Text.Json.Serialization;

namespace Application.Models.DTOs
{
    public class BrandDTO
    {
        [JsonIgnore]
        public Guid Id { get; set; }
        public int BaselinkerId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }

        public BrandDTO() { }

        public BrandDTO(Brand brand) 
        {
            Id = brand.Id;
            Name = brand.Name;
            Description = brand.Description;
        }
    }
}

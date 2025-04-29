using Application.Models.DTOs;
using Domain.Entities;
using System.Text.Json.Serialization;

namespace Application.Models.DTO_s
{
    public class MappingDTO
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string Title { get; set; }
        public CategoryDTO Category { get; set; }
        public BrandDTO Brand { get; set; }
        public List<MappingEntryDTO> MappingEntriesDTO { get; set; }


        public MappingDTO() { }
        public MappingDTO(Mapping mapping)
        {
            Id = mapping.Id;
            Name = mapping.Name;
            Description = mapping.Description;
            Title = mapping.Title;
            Category = new CategoryDTO(mapping.Category);
            Brand = new BrandDTO(mapping.Brand);
            MappingEntriesDTO = mapping.MappingEntries.Select(p => new MappingEntryDTO
            {
                Id = p.Id,
                //MappingType = p.MappingType,
                ColumnName = p.ColumnName,
                TargetField = p.TargetField,
            }).ToList();
        }

    }
}

using Domain.Entities;

namespace Application.Models.DTOs
{
    public class ParameterDTO
    {
        public Guid Id { get; set; }
        public int BaselinkerId { get; set; }
        public string Name { get; set; }
        public string Value { get; set; }

        public ParameterDTO() {}
        public ParameterDTO(Parameter parameter)
        {
            Id = parameter.Id;
            Name = parameter.Name;
            Value = parameter.Value;
        }
    }
}

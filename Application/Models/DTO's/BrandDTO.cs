﻿using Domain.Entities;

namespace Application.Models.DTOs
{
    public class BrandDTO
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }

        public BrandDTO(Brand brand) 
        {
            Id = brand.Id;
            Name = brand.Name;
            Description = brand.Description;
        }
    }
}

﻿using System.ComponentModel.DataAnnotations;

namespace BasketAPI.Models.Dto
{
    public class ProductDTO
    {
        public Guid Id { get; set; }
        public string? Name { get; set; }
        public string? Description { get; set; }
        public int Kcal { get; set; }
    }
}

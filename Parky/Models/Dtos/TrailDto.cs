﻿using System.ComponentModel.DataAnnotations;
using static ParkyApi.Models.Trail;

namespace ParkyApi.Models.Dtos
{
    public class TrailDto
    {
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        public double Distance { get; set; }
        public DifficultyType Difficulty { get; set; }

        public NationalParkDto NationalPark { get; set; }
        [Required]
        public int NationalParkId { get; set; }
    }
}

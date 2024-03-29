﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

#nullable disable

namespace NetCoreUnitTest.Models
{
    public partial class Product
    {
        [Required]
        public int Id { get; set; }

        [Required]

        public string Name { get; set; }

        [Required]

        public decimal? Price { get; set; }

        [Required]

        public int? Stock { get; set; }

        [Required]

        public string Color { get; set; }

        public int CategoryId { get; set; }
        public Category Category { get; set; }
    }
}

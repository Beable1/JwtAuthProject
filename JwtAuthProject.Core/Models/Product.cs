﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JwtAuthProject.Core.Models
{
    public class Product
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public Decimal Price { get; set; }

        public int Stock { get; set; }

        public string userId { get; set; }

    }
}

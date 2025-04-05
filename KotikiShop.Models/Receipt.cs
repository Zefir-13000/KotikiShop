﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KotikiShop.Models
{
    public class Receipt
    {
        public int Id { get; set; }
        public string UserId { get; set; }
        public float TotalPrice { get; set; }
        public DateTime Date { get; set; }
        public List<Cat> Cats { get; set; }
    }
}

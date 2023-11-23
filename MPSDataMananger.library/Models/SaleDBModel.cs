﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MPSDataMananger.library.Models
{
    public  class SaleDBModel
    {
        public int Id { get; set; }
        public string CashierId  { get; set; }
        public DateTime SaleDate { get; set; } = DateTime.Now;
        public decimal SubTotal {  get; set; }
        public decimal Tax {  get; set; }
        public decimal Total { get; set; }

    }
}

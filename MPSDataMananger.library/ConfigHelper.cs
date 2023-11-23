﻿using System;
using System.Collections.Generic;
using System.Configuration;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MPSDataMananger.library
{
    public class ConfigHelper 
    {
        public static decimal GetTaxRate()
        {

            string taxRate = ConfigurationManager.AppSettings["taxRate"];

            NumberFormatInfo format = new NumberFormatInfo();
            format.NumberDecimalSeparator = ".";

            //bool IsValidTtx = decimal.TryParse(taxRate, out decimal output);
            bool isValidTtx = decimal.TryParse(taxRate, NumberStyles.Number, format, out decimal output);

            if (!isValidTtx)
            {
                throw new ConfigurationErrorsException("the tax rate is not set up property");
            }
            return output;

        }
    }
}

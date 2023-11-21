using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MPSWPFDesktopUI.Library.Models
{
    public class CartItemModel
    {
        public ProductModel Product { get; set; }
        public int QuantatyInCart { get; set; }

        public string DisplayText
        {
            get {
                return $"{Product.ProductName} ({QuantatyInCart})";
            }
        }
    }
}

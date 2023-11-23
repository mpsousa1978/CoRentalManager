using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MPSWPFDesktopUI.Models
{
    public class ProductDisplayModel: INotifyPropertyChanged //because of mapper, it change the screem value CallPropertyChanged
    {
        public int Id { get; set; }
        public string ProductName { get; set; }
        public string Description { get; set; }
        public decimal RetailPrice { get; set; }

        private int _quantatyInStock;

        public int QuantatyInStock
        {
            get { return _quantatyInStock; }
            set { _quantatyInStock = value;
                CallPropertyChanged(nameof(QuantatyInStock));
            }
        }

        public bool Istaxable { get; set; }

        public event PropertyChangedEventHandler PropertyChanged;

        public void CallPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}

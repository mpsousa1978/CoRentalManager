using Caliburn.Micro;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MPSWPFDesktopUI.ViewModels
{
    public class SalesViewModel:Screen
    {
		private BindingList<string> _products;

		public BindingList<string> Products
		{
			get { return _products; }
			set { 
				_products = value;
                NotifyOfPropertyChange(() => Products);
            }
		}

        private BindingList<string> _cart;

        public BindingList<string> Cart
        {
            get { return _cart; }
            set
            {
                _cart = value;
                NotifyOfPropertyChange(() => Cart);
            }
        }


        private string _itemQuantaty;

		public string ItemQuantaty
        {
			get { return _itemQuantaty; }
			set { _itemQuantaty = value;
                NotifyOfPropertyChange(() => ItemQuantaty);
            }
		}

        public string SubTotal {
            //TODO Replace de calculator
            get
            {
                return "$0.00";
            }
        }

        public string Total
        {
            //TODO Replace de calculator
            get
            {
                return "$0.00";
            }
        }

        public string Tax
        {
            //TODO Replace de calculator
            get
            {
                return "$0.00";
            }
        }

        public bool CanAddtoCart
        {
            get
            {

                //make sure something is selected
                //make sure there is an item quantaty

                    return false;

            }
        }

        public void AddtoCart()
        {
        }

        public bool CanRemoveFromCart
        {
            get
            {

                //make sure something is selected
                //make sure there is an item quantaty

                return false;

            }
        }

        public void RemoveFromCart()
        {
        }


        public bool CanCheckOut
        {
            get
            {

                //there is someting in the cart


                return false;

            }
        }

        public void CheckOut()
        {
        }


    }
}

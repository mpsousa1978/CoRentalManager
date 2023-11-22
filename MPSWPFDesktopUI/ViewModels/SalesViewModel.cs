using Caliburn.Micro;
using MPSWPFDesktopUI.Library.Api;
using MPSWPFDesktopUI.Library.Helpers;
using MPSWPFDesktopUI.Library.Models;
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
        private IProductEndPoint _productEndPoint;
        private IConfigHelper _configHelper;


        public SalesViewModel( IProductEndPoint productEndPoint, IConfigHelper configHelper)
        {
            _productEndPoint = productEndPoint;
            _configHelper = configHelper;


        }

        private ProductModel _selectProduct;

        public ProductModel SelectProduct
        {
            get { return _selectProduct; }
            set {
                _selectProduct = value;
                NotifyOfPropertyChange(() => SelectProduct);
                NotifyOfPropertyChange(() => CanAddtoCart);
            }
        }

        protected override async void OnViewLoaded(object view)
        {
            base.OnViewLoaded(view);
            await LoadProduct();
        }
        private async Task LoadProduct()
        {
            var productList = await _productEndPoint.GetAll();
            Products = new BindingList<ProductModel>(productList);
        }

        private BindingList<ProductModel> _products;
		public BindingList<ProductModel> Products
		{
			get { return _products; }
			set { 
				_products = value;
                NotifyOfPropertyChange(() => Products);
            }
		}

        private BindingList<CartItemModel> _cart = new BindingList<CartItemModel>();

        public BindingList<CartItemModel> Cart
        {
            get { return _cart; }
            set
            {
                _cart = value;
                NotifyOfPropertyChange(() => Cart);
            }
        }



        private int _itemQuantaty = 0;

		public int ItemQuantaty
        {
			get { return _itemQuantaty; }
			set { _itemQuantaty = value;
                NotifyOfPropertyChange(() => ItemQuantaty);
                NotifyOfPropertyChange(() => CanAddtoCart);
                
            }
		}

        public string SubTotal {
            //TODO Replace de calculator
            get
            {
                return CalculateSubTotal().ToString("C");
            }
        }

        private decimal CalculateSubTotal()
        {
            decimal subTotal = 0;
            foreach (var item in Cart)
            {
                subTotal += (item.Product.RetailPrice * item.QuantatyInCart);
            }
            return subTotal;
        }

        public string Tax
        {
            get
            {

                return CalculateTax().ToString("C");

            }
        }
        private decimal CalculateTax()
        {
            decimal taxAmount = 0;
            decimal taxRate = _configHelper.GetTaxRate()/100;

            foreach (var item in Cart)
            {
                if (item.Product.Istaxable)
                {
                    taxAmount += (item.Product.RetailPrice * (item.QuantatyInCart * taxRate));
                }
            }
            return taxAmount;
        }


        public string Total
        {
            //TODO Replace de calculator
            get
            {
                decimal total = CalculateSubTotal() + CalculateTax(); 
                return total.ToString("C");
            }
        }



        public bool CanAddtoCart
        {
            get
            {
                //make sure something is selected
                //make sure there is an item quantaty
                if(ItemQuantaty > 0 && SelectProduct?.QuantatyInStock >= ItemQuantaty )
                {
                    return true;
                }
                else
                {
                    return false;
                }
                    

            }
        }

        public void AddtoCart()
        {
            CartItemModel existItem = Cart.FirstOrDefault(x => x.Product.ProductName == SelectProduct.ProductName);
            if (existItem != null)
            {
                existItem.QuantatyInCart += ItemQuantaty;
                Cart.Remove(existItem);
                Cart.Add(existItem);
            }
            else
            {
                CartItemModel item = new CartItemModel()
                {
                    Product = SelectProduct,
                    QuantatyInCart = ItemQuantaty
                };
                Cart.Add(item);
            }

            SelectProduct.QuantatyInStock -= ItemQuantaty;
            ItemQuantaty = 0;
            NotifyOfPropertyChange(() => SubTotal);
            NotifyOfPropertyChange(() => Tax);
            NotifyOfPropertyChange(() => Total);
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

            NotifyOfPropertyChange(() => SubTotal);
            NotifyOfPropertyChange(() => Tax);
            NotifyOfPropertyChange(() => Total);

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

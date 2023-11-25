using Caliburn.Micro;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MPSWPFDesktopUI.Library.Api;
using MPSWPFDesktopUI.Library.Helpers;
using MPSWPFDesktopUI.Library.Models;
using AutoMapper;
using MPSWPFDesktopUI.Models;
using System.Runtime.InteropServices;

namespace MPSWPFDesktopUI.ViewModels
{
    public class SalesViewModel:Screen
    {
        private IProductEndPoint _productEndPoint;
        private ISaleEndPoint _saleEndPoint;
        private IConfigHelper _configHelper;
        private IMapper _mapper;


        public SalesViewModel( IProductEndPoint productEndPoint, IConfigHelper configHelper, ISaleEndPoint saleEndPoint, IMapper mapper)
        {
            _productEndPoint = productEndPoint;
            _configHelper = configHelper;
            _saleEndPoint = saleEndPoint;
            _mapper = mapper;

        }


        protected override async void OnViewLoaded(object view)
        {
            base.OnViewLoaded(view);
            await LoadProduct();
        }
        private async Task LoadProduct()
        {
            var productList = await _productEndPoint.GetAll();
            var products = _mapper.Map<List<ProductDisplayModel>>(productList);
            Products = new BindingList<ProductDisplayModel>(products);

        }


        private async Task ResetSaleViewModel()
        {
           Cart = new BindingList<CartItemDisplayModel>();
            //Toto Add clearing the selectedCartItem if it does not do it itself
            await LoadProduct();
            NotifyOfPropertyChange(() => SubTotal);
            NotifyOfPropertyChange(() => Tax);
            NotifyOfPropertyChange(() => Total);
            NotifyOfPropertyChange(() => CanCheckOut);
        }
        private ProductDisplayModel _selectProduct;

        public ProductDisplayModel SelectProduct
        {
            get { return _selectProduct; }
            set {
                _selectProduct = value;
                NotifyOfPropertyChange(() => SelectProduct);
                NotifyOfPropertyChange(() => CanAddtoCart);
            }
        }


        

        private CartItemDisplayModel _selectCartItem;

        public CartItemDisplayModel SelectCartItem
        {
            get { return _selectCartItem; }
            set
            {
                _selectCartItem = value;
                NotifyOfPropertyChange(() => SelectCartItem);
                NotifyOfPropertyChange(() => CanRemoveFromCart);
            }
        }




        private BindingList<ProductDisplayModel> _products;
		public BindingList<ProductDisplayModel> Products
		{
			get { return _products; }
			set { 
				_products = value;
                NotifyOfPropertyChange(() => Products);
            }
		}

        private BindingList<CartItemDisplayModel> _cart = new BindingList<CartItemDisplayModel>();

        public BindingList<CartItemDisplayModel> Cart
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
                subTotal += (item.Product.RetailPrice * item.QuantityInCart);
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

            //Linq
            taxAmount = Cart
                .Where(x => x.Product.Istaxable)
                .Sum(x => x.Product.RetailPrice * (x.QuantityInCart * taxRate));


            //foreach (var item in Cart)
            //{
            //    if (item.Product.Istaxable)
            //    {
            //        taxAmount += (item.Product.RetailPrice * (item.QuantatyInCart * taxRate));
            //    }
            //}
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
            CartItemDisplayModel existItem = Cart.FirstOrDefault(x => x.Product.ProductName == SelectProduct.ProductName);
            if (existItem != null)
            {
                existItem.QuantityInCart += ItemQuantaty;
            }
            else
            {
                CartItemDisplayModel item = new CartItemDisplayModel()
                {
                    Product = SelectProduct,
                    QuantityInCart = ItemQuantaty
                };
                Cart.Add(item);
            }

            SelectProduct.QuantatyInStock -= ItemQuantaty; //when change the value here automaticle change on screen becouse of mapper
            ItemQuantaty = 0;
            NotifyOfPropertyChange(() => SubTotal);
            NotifyOfPropertyChange(() => Tax);
            NotifyOfPropertyChange(() => Total);
            NotifyOfPropertyChange(() => CanCheckOut);
        }

        public bool CanRemoveFromCart
        {
            get
            {

                //make sure something is selected
                if (SelectCartItem != null)
                {
                    return true;
                }
                else
                {
                    return false;
                }

            }
        }

        public void RemoveFromCart()
        {


            SelectCartItem.Product.QuantatyInStock += SelectCartItem.QuantityInCart;
            Cart.Remove(SelectCartItem);

            NotifyOfPropertyChange(() => SubTotal);
            NotifyOfPropertyChange(() => Tax);
            NotifyOfPropertyChange(() => Total);
            NotifyOfPropertyChange(() => CanCheckOut);
            NotifyOfPropertyChange(() => CanAddtoCart);
            

        }


        public bool CanCheckOut
        {
            get
            {
                if (Cart.Count > 0)
                {
                    return true;
                }
                return false;

            }
        }

        public async Task CheckOut()
        {

            SaleModel sale = new SaleModel();
            foreach (var item in  Cart)
            {
                sale.SaleDetails.Add(new SaleDetailModel
                {
                    ProductId = item.Product.Id,
                    Quantity = item.QuantityInCart
                });
            }

            
            await _saleEndPoint.PostSale(sale);
            await ResetSaleViewModel();
        }


    }
}

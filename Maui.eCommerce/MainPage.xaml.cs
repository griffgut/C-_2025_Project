using Library.eCommerce.Services;
using Maui.eCommerce.ViewModels;

namespace Maui.eCommerce
{
    public partial class MainPage : ContentPage
    {
        int count = 0;

        public MainPage()
        {
            InitializeComponent();
            BindingContext = this;
        }

        private void InventoryClicked(object sender, EventArgs e)
        {
            Shell.Current.GoToAsync("//InventoryManagement");
        }
        private void ShopClicked(object sender, EventArgs e)
        {
            Shell.Current.GoToAsync("//ShoppingManagement");
        }
        private void CheckoutClicked(object sender, EventArgs e) 
        {
            if (ShoppingServiceProxy.Current.CartItems.Any())
            {
                double? p = 0;
                double? q = 0;
                double? total = 0;
                foreach (var item in ShoppingServiceProxy.Current.CartItems)
                {
                    p = item.Price;
                    q = item.Quantity;
                    total += p * q;
                }
                total = (total * .07) + total;
                var stotal = $"{total:0.00}";
                ResultLabel.Text="Total with .7% tax added: $" + stotal;
            }
            else
            {
                ResultLabel.Text ="You had no items in the shopping cart";
            }
        }

    }

}

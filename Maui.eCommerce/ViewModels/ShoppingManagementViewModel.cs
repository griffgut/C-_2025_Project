using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using Library.eCommerce.Models;
using Library.eCommerce.Services;
using Spring2025_Samples.Models;

namespace Maui.eCommerce.ViewModels
{
    public class ShoppingManagementViewModel : INotifyPropertyChanged
    {
        private ProductServiceProxy _invSvc = ProductServiceProxy.Current;
        private ShoppingServiceProxy _cartSvc = ShoppingServiceProxy.Current;
        public Item? SelectedItem { get; set; }
        public Item? SelectedCartItem { get; set; }

        public event PropertyChangedEventHandler? PropertyChanged;

        public ObservableCollection<Item?> Inventory
        {
            get
            {
                return new ObservableCollection<Item?>(_invSvc.Products
                    .Where(i => i?.Quantity > 0)
                    );
            }
        }
        public ObservableCollection<Item?> Cart
        {
            get
            {
                return new ObservableCollection<Item?>(_cartSvc.CartItems
                    .Where(i => i?.Quantity > 0)
                    );
            }
        }
        public void NotifyPropertyChanged([CallerMemberName] string propertyName = "")
        {
            if (propertyName is null)
            {
                throw new ArgumentNullException(nameof(propertyName));
            }

            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        public void RefreshList()
        {
            NotifyPropertyChanged(nameof(Inventory));
            NotifyPropertyChanged(nameof(Cart));
        }

        public void PurchaseItem()
        {
            if (SelectedItem != null)
            {
                var shouldRefresh = SelectedItem.Quantity >= 1;
                var updatedItem = _cartSvc.AddOrUpdate(SelectedItem);
                if(updatedItem != null && shouldRefresh)
                {
                    NotifyPropertyChanged(nameof(Inventory));
                    NotifyPropertyChanged(nameof(Cart));
                }
            }
        }
        public void ReturnItem()
        {

            if (SelectedCartItem != null) 
            {
                var shouldRefresh = SelectedCartItem.Quantity >= 1;
                var updatedItem = _cartSvc.ReturnItem(SelectedCartItem);
                if (updatedItem != null && shouldRefresh)
                {
                    NotifyPropertyChanged(nameof(Inventory));
                    NotifyPropertyChanged(nameof(Cart));
                }
            }

        }

    }
}

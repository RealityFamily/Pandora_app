using Pandora.Core;
using Pandora.DI;
using Pandora.MVVM.Models;
using Pandora.Network;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Pandora.MVVM.ViewModels
{
    class ItemsListViewModel
    {
        private ObservableObject<Dictionary<string, List<Item>>> _itemsCollection = new ObservableObject<Dictionary<string, List<Item>>>();

        public ObservableObject<Dictionary<string, List<Item>>> ItemsCollection { get { return _itemsCollection; } }

        public void SetData(string subGroupId = null)
        {
            if (string.IsNullOrEmpty(subGroupId) ) 
            {
                _itemsCollection.Value = new Dictionary<string, List<Item>>() {
                    { "", new LocalServiceLocator().FileSystemMethods.GetLocalId().Select(id => {
                        return new Item() { Id = id };
                    }).ToList() } 
                };
            } 
            else 
            {
                _itemsCollection.Value = ItemsNetworkLogic.GetItems(subGroupId);
            }
        }
    }
}

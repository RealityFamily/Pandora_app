using Pandora.Core;
using Pandora.DI;
using Pandora.MVVM.Models;
using Pandora.Network;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Media.Imaging;

namespace Pandora.MVVM.ViewModels
{
    class ItemInfoViewModel
    {
        private ObservableObject<Item> _choosedItem = new ObservableObject<Item>();

        public ObservableObject<Item> ChoosedItem { get { return _choosedItem; } }


        public void ActionItem()
        {
            if (_choosedItem.Value != null)
            {
                if (_choosedItem.Value.Contains)
                {
                    new LocalServiceLocator().FileSystemMethods.DeleteModel(_choosedItem.Value);
                }
                else
                {
                    ItemsNetworkLogic.DownloadZIP(_choosedItem.Value);
                }

                _choosedItem.ObjectChanged();
                new LocalServiceLocator().ListOfGroupViewModel.SetData(new LocalServiceLocator().MainPage.ChoosedGroup.Value.id);
            }
        }
    }
}

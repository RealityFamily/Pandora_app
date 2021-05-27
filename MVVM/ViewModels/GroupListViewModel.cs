using Pandora.Core;
using Pandora.DI;
using Pandora.MVVM.Models;
using Pandora.Network;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Pandora.MVVM.ViewModels
{
    class GroupListViewModel
    {
        private List<Group> _subGroups;
        private ObservableObject<Group> _choosedGroup = new ObservableObject<Group>();

        public List<Group> SubGroups
        {
            get
            {
                return _subGroups;
            }
        }
        public List<Group> Groups
        {
            get
            {
                var groups = GroupsNetworkLogic.GetGroups();
                groups.Insert(0, new Group(null, "Скаченное"));
                return groups;
            }
        }

        public ObservableObject<Group> ChoosedGroup
        {
            get { return _choosedGroup; }
        }

        public GroupListViewModel()
        {
            _choosedGroup.PropertyChanged += (sender, e) =>
            {
                if (_choosedGroup.Value != null && !string.IsNullOrEmpty(_choosedGroup.Value.category))
                {
                    new LocalServiceLocator().ItemInfoViewModel.ChoosedItem.Value = null;
                    if ((_subGroups != null && _subGroups.Any(group => group.id == _choosedGroup.Value.id)) || _choosedGroup.Value.category.Equals("Скаченное"))
                    {
                        new LocalServiceLocator().ListOfGroupViewModel.SetData(_choosedGroup.Value.id);
                    }
                    else
                    {
                        _subGroups = GroupsNetworkLogic.GetSubGroups(_choosedGroup.Value.id);
                    }
                }
            };
        }
    }
}

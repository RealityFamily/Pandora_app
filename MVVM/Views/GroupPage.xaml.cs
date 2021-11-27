using Pandora.DI;
using Pandora.MVVM.Models;
using Pandora.MVVM.ViewModels;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Controls;
using System.Windows.Media;

namespace Pandora.MVVM.Views
{
    /// <summary>
    /// Логика взаимодействия для GroupPage.xaml
    /// </summary>
    public partial class GroupPage : Page
    {
        private GroupListViewModel mViewModel;
        private Color nextColor;

        public GroupPage(Color BackgroundColor, Color SelectionColor, List<Group> GroupList)
        {
            InitializeComponent();
            this.DataContext = this;
            nextColor = SelectionColor;

            LocalServiceLocator serviceLocator = new LocalServiceLocator();
            mViewModel = serviceLocator.MainPage;

            Page.Background = new SolidColorBrush(BackgroundColor);
            GroupsList.Resources["ContentSelectColor"] = new SolidColorBrush(SelectionColor);

            GroupsList.ItemsSource = GroupList;
        }

        private void GroupSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            Group selected = (Group)((ListBox)e.OriginalSource).SelectedItem;

            if (selected.category.Equals("Скачанное"))
            {
                content.Content = new ListOfObjects(nextColor);
                mViewModel.ChoosedGroup.Value = selected;
            }
            else
            {
                mViewModel.ChoosedGroup.Value = selected;
                if (mViewModel.SubGroups.Any(group => group.id == selected.id))
                {
                    content.Content = new ListOfObjects(nextColor);
                    mViewModel.ChoosedGroup.ObjectChanged();
                }
                else
                {
                    content.Content = new GroupPage(nextColor, Color.FromArgb(255, 89, 97, 111), mViewModel.SubGroups);
                }
            }
        }
    }
}

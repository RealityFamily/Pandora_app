using CommonServiceLocator;
using GalaSoft.MvvmLight.Ioc;
using Pandora._3Ds_Max;
using Pandora.Core;
using Pandora.MVVM.ViewModels;

namespace Pandora.DI
{
    class LocalServiceLocator
    {
        public LocalServiceLocator()
        {
            if (!ServiceLocator.IsLocationProviderSet)
            {
                ServiceLocator.SetLocatorProvider(() => SimpleIoc.Default);

                SimpleIoc.Default.Register<GroupListViewModel>();
                SimpleIoc.Default.Register<MainWindowViewModel>();
                SimpleIoc.Default.Register<ItemsListViewModel>();
                SimpleIoc.Default.Register<FileSystemMethods>();
                SimpleIoc.Default.Register<ApplicationConfig>();
                SimpleIoc.Default.Register<UserViewModel>(); 
                SimpleIoc.Default.Register<ItemInfoViewModel>();
                SimpleIoc.Default.Register<UploadViewModel>();

            }
        }

        public UploadViewModel UploadViewModel
        {
            get
            {
                return ServiceLocator.Current.GetInstance<UploadViewModel>();
            }
        }

        public GroupListViewModel MainPage
        {
            get
            {
                return ServiceLocator.Current.GetInstance<GroupListViewModel>();
            }
        }

        public MainWindowViewModel MainWindow
        {
            get
            {
                return ServiceLocator.Current.GetInstance<MainWindowViewModel>();
            }
        }

        public ItemsListViewModel ListOfGroupViewModel
        {
            get
            {
                return ServiceLocator.Current.GetInstance<ItemsListViewModel>();
            }
        }

        public FileSystemMethods FileSystemMethods
        {
            get
            {
                return ServiceLocator.Current.GetInstance<FileSystemMethods>();
            }
        }

        public ApplicationConfig ApplicationConfig
        {
            get
            {
                return ServiceLocator.Current.GetInstance<ApplicationConfig>();
            }
        }

        public UserViewModel UserViewModel
        {
            get
            {
                return ServiceLocator.Current.GetInstance<UserViewModel>();
            }
        }

        public ItemInfoViewModel ItemInfoViewModel
        {
            get
            {
                return ServiceLocator.Current.GetInstance<ItemInfoViewModel>();
            }
        }

        public static void Cleanup()
        {
            // TODO Clear the ViewModels
        }
    }
}

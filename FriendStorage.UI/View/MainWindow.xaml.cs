using FriendStorage.UI.ViewModel;
using System.Windows;

namespace FriendStorage.UI.View
{
    public partial class MainWindow : Window
    {
        MainViewModel viewModel;

        public MainWindow()
        {
            InitializeComponent();
        }

        public MainWindow(MainViewModel viewModel)
            : this()
        {
            this.viewModel = viewModel;
            this.DataContext = this.viewModel;
            this.Loaded += MainWindow_Loaded;
        }

        private void MainWindow_Loaded(object sender, RoutedEventArgs e)
        {
            viewModel.Load();
        }
    }
}

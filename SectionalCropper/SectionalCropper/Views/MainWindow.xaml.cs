using System.Windows;
using System.Windows.Controls;

namespace SectionalCropper.Views
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {

        private readonly ViewModels.MainWindowViewModel _viewModel;

        public MainWindow()
        {
            InitializeComponent();
            DataContext = _viewModel = new ViewModels.MainWindowViewModel();
        }

        private void Image_PreviewMouseLeftButtonDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            _viewModel.MouseDown(e.GetPosition((Image)sender));
        }

        private void Image_PreviewMouseLeftButtonUp(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            _viewModel.MouseUp(e.GetPosition((Image)sender));
        }
    }
}

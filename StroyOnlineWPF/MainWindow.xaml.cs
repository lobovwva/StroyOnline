using StroyOnlineWPF.ViewModel;
using System.Windows;

namespace StroyOnlineWPF
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {

        private EmployeeViewModel _employeeViewModel;

        public MainWindow()
        {
            InitializeComponent();
            Loaded += MainWindow_Loaded;
            _employeeViewModel = new EmployeeViewModel();
            DataContext = _employeeViewModel;
        }

        private async void MainWindow_Loaded(object sender, RoutedEventArgs e)
        {
            await _employeeViewModel.GetEmployees();
        }
    }
}

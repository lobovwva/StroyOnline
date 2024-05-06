using StroyOnlineWPF.Models;
using StroyOnlineWPF.ViewModel;
using System.Windows;

namespace StroyOnlineWPF
{
    /// <summary>
    /// Логика взаимодействия для EditWindow.xaml
    /// </summary>
    public partial class EditWindow : Window
    {
        private EditViewModel _editViewModel;

        public EditWindow(Employee employee)
        {
            InitializeComponent();

            _editViewModel = new EditViewModel(employee);

            DataContext = _editViewModel;
        }
    }
}

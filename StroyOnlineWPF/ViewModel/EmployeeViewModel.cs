using GalaSoft.MvvmLight.Command;
using Newtonsoft.Json;
using StroyOnlineWPF.Models;
using StroyOnlineWPF.Service;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace StroyOnlineWPF.ViewModel
{
    public class EmployeeViewModel : INotifyPropertyChanged
    {
        public ICommand EditCommand { get; }
        public ICommand SelectEmployeeCommand { get; }
        public ICommand DeleteCommand { get; }
        public ICommand AddCommand { get; private set; }

        private ObservableCollection<Employee> _employees;
        public ObservableCollection<Employee> Employees
        {
            get { return _employees; }
            set
            {
                _employees = value;
                OnPropertyChanged(nameof(Employees));
            }
        }

        private ObservableCollection<Position> _positions;
        public ObservableCollection<Position> Positions
        {
            get { return _positions; }
            set
            {
                _positions = value;
                OnPropertyChanged(nameof(Positions));
            }
        }

        private Employee _selectedEmployee;
        public Employee SelectedEmployee
        {
            get { return _selectedEmployee; }
            set
            {
                _selectedEmployee = value;
                OnPropertyChanged(nameof(SelectedEmployee));
            }
        }

        public EmployeeViewModel()
        {
            SelectEmployeeCommand = new RelayCommand<Employee>(SelectEmployee);
            EditCommand = new RelayCommand(EditEmployee);
            DeleteCommand = new RelayCommand(DeleteEmployee);
            AddCommand = new RelayCommand(AddEmployee);
        }

        private void AddEmployee()
        {
            EditWindow addEmployeeWindow = new EditWindow(new Employee());
            addEmployeeWindow.ShowDialog();
        }

        private void DeleteEmployee()
        {
            if (SelectedEmployee != null)
            {
                MessageBoxResult result = MessageBox.Show("Вы уверены, что хотите удалить данные сотрудника?", "Подтверждение удаления", MessageBoxButton.YesNo, MessageBoxImage.Question);

                if (result == MessageBoxResult.Yes)
                {
                    DeleteEmployee(SelectedEmployee.Id);
                }
            }
        }

        private void EditEmployee()
        {
            if (SelectedEmployee != null)
            {
                EditWindow editWindow = new EditWindow(SelectedEmployee);
                editWindow.ShowDialog();
            }
            else
            {
                MessageBox.Show("Выберите сотрудника для редактирования.");
            }
        }

        private void SelectEmployee(Employee selectedEmployee)
        {
            if (selectedEmployee != null)
            {
                SelectedEmployee = selectedEmployee;
            }
        }

        public async Task GetEmployees()
        {
            using (HttpClient client = new HttpClient())
            {
                try
                {
                    HttpResponseMessage response = await client.GetAsync(EmployeeService.EmployeesSvcIP);
                    response.EnsureSuccessStatusCode();
                    var employees = await response.Content.ReadAsAsync<IList<Employee>>();

                    HttpResponseMessage positionsResponse = await client.GetAsync(EmployeeService.PositionsSvcIP);
                    positionsResponse.EnsureSuccessStatusCode();
                    var positions = await positionsResponse.Content.ReadAsAsync<IList<Position>>();

                    foreach (var employee in employees)
                    {
                        employee.Position = positions.FirstOrDefault(p => p.Id == employee.PositionId);
                    }

                    Employees = new ObservableCollection<Employee>(employees);
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Ошибка при получении информации о сотрудниках: {ex.Message}");
                }
            }
        }

        public async Task<bool> AddEmployee(Employee employee)
        {
            using (HttpClient client = new HttpClient())
            {
                try
                {
                    string jsonEmployee = JsonConvert.SerializeObject(employee);
                    StringContent content = new StringContent(jsonEmployee, Encoding.UTF8, "application/json");

                    HttpResponseMessage response = await client.PostAsync(EmployeeService.EmployeesSvcIP, content);
                    response.EnsureSuccessStatusCode();

                    return true;
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Ошибка при добавлении сотрудника: {ex.Message}");
                    return false;
                }
            }
        }

        private async void DeleteEmployee(Guid employeeId)
        {
            using (HttpClient client = new HttpClient())
            {
                try
                {
                    HttpResponseMessage response = await client.DeleteAsync($"{EmployeeService.EmployeesSvcIP}/{employeeId}");
                    response.EnsureSuccessStatusCode();
                    MessageBox.Show("Сотрудник успешно удален.");
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Ошибка при удалении сотрудника: {ex.Message}");
                }
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
    

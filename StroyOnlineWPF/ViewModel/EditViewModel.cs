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
    public class EditViewModel : INotifyPropertyChanged
    {
        public ICommand SaveCommand { get; }
        public ICommand CancelCommand { get; }

        public string Firstname { get; set; }
        public string Lastname { get; set; }
        public string Surname { get; set; }
        public DateTime Birthday { get; set; }
        public short? PositionId { get; set; }
        public int Salary { get; set; }
        public bool IsActive { get; set; }

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

        private Employee _employee;
        private EmployeeViewModel _employeeViewModel;

        public EditViewModel(Employee employee)
        {
            _employee = employee;

            Firstname = _employee.Firstname;
            Lastname = _employee.Lastname;
            Surname = _employee.Surname;
            PositionId = _employee.PositionId;
            Salary = _employee.Salary;
            IsActive = _employee.IsActive;
            Birthday = _employee.Birthday;

            if (_employee.Id == Guid.Empty) 
            {
                IsActive = true; 
            }

            LoadPositions();
            SaveCommand = new RelayCommand(async () => await SaveEmployee());
            CancelCommand = new RelayCommand(CancelWindow);
            _employeeViewModel = new EmployeeViewModel();
        }

        private void CancelWindow()
        {
            var window = Application.Current.Windows.OfType<Window>().FirstOrDefault(w => w.IsActive);

            window?.Close();
        }


        private async void LoadPositions()
        {
            var positions = await GetPositions();
            if (positions != null)
            {
                Positions = new ObservableCollection<Position>(positions);
            }
        }

        public async Task<IList<Position>> GetPositions()
        {
            using (HttpClient client = new HttpClient())
            {
                try
                {
                    HttpResponseMessage response = await client.GetAsync(EmployeeService.PositionsSvcIP);
                    response.EnsureSuccessStatusCode();
                    IList<Position> positions = await response.Content.ReadAsAsync<IList<Position>>();
                    return positions;
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Ошибка при получении списка должностей: {ex.Message}");
                    return null;
                }
            }
        }

        private async Task SaveEmployee()
        {
            if (string.IsNullOrEmpty(Firstname) || string.IsNullOrEmpty(Lastname) || !PositionId.HasValue || Salary <= 0)
            {
                MessageBox.Show("Пожалуйста, заполните все обязательные поля (Имя, Фамилия, Должность, Зарплата).");
                return;
            }

            Employee updatedEmployee = new Employee
            {
                Id = _employee.Id,
                Firstname = Firstname,
                Lastname = Lastname,
                Surname = Surname,
                Birthday = Birthday,
                PositionId = PositionId.Value,
                Salary = Salary,
                IsActive = IsActive
            };

            bool success = false;

            if (_employee.Id != Guid.Empty)
            {
                success = await UpdateEmployee(updatedEmployee);

                if (success)
                {
                    MessageBox.Show("Данные сотрудника успешно обновлены.");
                    CancelWindow();
                    await _employeeViewModel.GetEmployees(); 
                }
            }
            else
            {
                success = await _employeeViewModel.AddEmployee(updatedEmployee);

                if (success)
                {
                    MessageBox.Show("Сотрудник успешно добавлен.");
                    CancelWindow();
                    await _employeeViewModel.GetEmployees();
                }
            }
        }

        private async Task<bool> UpdateEmployee(Employee employee)
        {
            using (HttpClient client = new HttpClient())
            {
                try
                {
                    string jsonEmployee = JsonConvert.SerializeObject(employee);
                    StringContent content = new StringContent(jsonEmployee, Encoding.UTF8, "application/json");

                    HttpResponseMessage response = await client.PutAsync($"{EmployeeService.EmployeesSvcIP}/{employee.Id}", content);
                    response.EnsureSuccessStatusCode();

                    return true;
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Ошибка при обновлении данных сотрудника: {ex.Message}");
                    return false;
                }
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

    }
}

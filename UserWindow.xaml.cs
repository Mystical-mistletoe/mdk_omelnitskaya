using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Intrinsics.Arm;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using UIApplication.Models;
using UIApplication.Services;

namespace UIApplication
{
    /// <summary>
    /// Логика взаимодействия для UserWindow.xaml
    /// </summary>
    public partial class UserWindow : Window
    {
        private string _currentUserLogin;
        private List<Service> _services;
        private List<Appointment> _userAppointments;

        public UserWindow(string userLogin)
        {
            InitializeComponent();
            _currentUserLogin = userLogin;
            Title = $"Окно пользователя: {userLogin}";
            LoadServices();
            LoadUserAppointments();
        }

        private void LoadServices()
        {
            _services = ServiceManager.GetAllServices();
            servicesListBox.ItemsSource = _services;
            servicesListBox.DisplayMemberPath = "Name";
        }

        private void LoadUserAppointments()
        {
            _userAppointments = AppointmentManager.GetUserAppointments(_currentUserLogin);
            appointmentsListBox.ItemsSource = _userAppointments;
            appointmentsListBox.DisplayMemberPath = "AppointmentTime";
        }

        private void servicesListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (servicesListBox.SelectedItem is Service selectedService)
            {
                serviceNameTextBlock.Text = selectedService.Name;
                serviceDescriptionTextBlock.Text = selectedService.Description;
                servicePriceTextBlock.Text = selectedService.Price.ToString("C");

            }
        }

        private void bookAppointmentButton_Click(object sender, RoutedEventArgs e)
        {
            if (!(servicesListBox.SelectedItem is Service selectedService))
            {
                MessageBox.Show("Пожалуйста, выберите услугу.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            DateTime appointmentDate;
     
            if (!DateTime.TryParse(appointmentDateTextBox.Text, out appointmentDate))
            {
                MessageBox.Show("Пожалуйста, введите корректную дату.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            var newAppointment = new Appointment
            {
                UserLogin = _currentUserLogin,
                ServiceId = selectedService.Id,
                AppointmentTime = appointmentDate
            };

            if (AppointmentManager.AddAppointment(newAppointment))
            {
                MessageBox.Show("Запись успешно создана!", "Успех", MessageBoxButton.OK, MessageBoxImage.Information);
                LoadUserAppointments();
            }
            else
            {
                MessageBox.Show("Не удалось создать запись. Возможно, это время уже занято.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void refreshAppointmentsButton_Click(object sender, RoutedEventArgs e)
        {
            LoadUserAppointments();
        }

        private void logoutButton_Click(object sender, RoutedEventArgs e)
        {
            MainWindow mainWindow = new MainWindow();
            mainWindow.Show();
            this.Close();
        }

    }
}

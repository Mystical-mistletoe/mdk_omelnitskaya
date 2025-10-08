using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Linq;
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
    /// Логика взаимодействия для ModeratorWindow.xaml
    /// </summary>
    public partial class ModeratorWindow : Window
    {
        private List<Service> _services;

        public ModeratorWindow()
        {
            InitializeComponent();
            Title = "Окно модератора";
            LoadServices();
        }

        private void LoadServices()
        {
            _services = ServiceManager.GetAllServices();
            servicesListBox.ItemsSource = _services;
            servicesListBox.DisplayMemberPath = "Name";
        }

        private void servicesListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (servicesListBox.SelectedItem is Service selectedService)
            {
                serviceNameTextBox.Text = selectedService.Name;
                serviceDescriptionTextBox.Text = selectedService.Description;
                servicePriceTextBox.Text = selectedService.Price.ToString("C");
            }
        }

        private void addServiceButton_Click(object sender, RoutedEventArgs e)
        {
            decimal price;
            if (!decimal.TryParse(servicePriceTextBox.Text, out price))
            {
                MessageBox.Show("Пожалуйста, введите корректную цену.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            var newService = new Service
            {
                Name = serviceNameTextBox.Text,
                Description = serviceDescriptionTextBox.Text,
                Price = price
            };

            if (ServiceManager.AddService(newService))
            {
                MessageBox.Show("Услуга успешно добавлена!", "Успех", MessageBoxButton.OK, MessageBoxImage.Information);
                LoadServices();
            }
            else
            {
                MessageBox.Show("Не удалось добавить услугу.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void updateServiceButton_Click(object sender, RoutedEventArgs e)
        {
            if (!(servicesListBox.SelectedItem is Service selectedService))
            {
                MessageBox.Show("Пожалуйста, выберите услугу.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            decimal price;
            if (!decimal.TryParse(servicePriceTextBox.Text, out price))
            {
                MessageBox.Show("Пожалуйста, введите корректную цену.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            var updatedService = new Service
            {
                Id = selectedService.Id,
                Name = serviceNameTextBox.Text,
                Description = serviceDescriptionTextBox.Text,
                Price = price
            };

            if (ServiceManager.UpdateService(updatedService))
            {
                MessageBox.Show("Услуга успешно обновлена!", "Успех", MessageBoxButton.OK, MessageBoxImage.Information);
                LoadServices();
            }
            else
            {
                MessageBox.Show("Не удалось обновить услугу.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void deleteServiceButton_Click(object sender, RoutedEventArgs e)
        {
            if (!(servicesListBox.SelectedItem is Service selectedService))
            {
                MessageBox.Show("Пожалуйста, выберите услугу для удаления.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            if (ServiceManager.DeleteService(selectedService.Id))
            {
                MessageBox.Show("Услуга успешно удалена!", "Успех", MessageBoxButton.OK, MessageBoxImage.Information);
                LoadServices();
            }
            else
            {
                MessageBox.Show("Не удалось удалить услугу.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void exportAppointmentsButton_Click(object sender, RoutedEventArgs e)
        {
            var saveFileDialog = new SaveFileDialog
            {
                Filter = "Text files (*.txt)|*.txt",
                DefaultExt = ".txt"
            };

            if (saveFileDialog.ShowDialog() == true)
            {
                try
                {
                    AppointmentManager.ExportAppointmentsToTxt(saveFileDialog.FileName);
                    MessageBox.Show("Записи успешно экспортированы!", "Успех", MessageBoxButton.OK, MessageBoxImage.Information);
                }
                catch
                {
                    MessageBox.Show("Не удалось экспортировать записи.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }

        private void logoutButton_Click(object sender, RoutedEventArgs e)
        {
            MainWindow mainWindow = new MainWindow();
            mainWindow.Show();
            this.Close();
        }
    }
}

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
using UIApplication.DataManagers;
using UIApplication.Models;

namespace UIApplication
{
    /// <summary>
    /// Логика взаимодействия для RegisterWindow.xaml
    /// </summary>
    public partial class RegisterWindow : Window
    {
        public RegisterWindow()
        {
            InitializeComponent();
        }

        private void BtnRegisterUser_Click(object sender, RoutedEventArgs e)
        {
            string login = txtLogin.Text;
            string password = pbPassword.Password;
            string confirmPassword = pbConfirmPassword.Password;

            if (string.IsNullOrWhiteSpace(login) || string.IsNullOrWhiteSpace(password) || string.IsNullOrWhiteSpace(confirmPassword))
            {
                MessageBox.Show("Пожалуйста, заполните все поля.", "Ошибка регистрации", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            if (password != confirmPassword)
            {
                MessageBox.Show("Пароли не совпадают.", "Ошибка регистрации", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            if (password.Length < 8)
            {
                MessageBox.Show("Пароль должен содержать не менее 8 символов.", "Ошибка регистрации", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            if (!login.Contains("@yandex.ru"))
            {
                MessageBox.Show("Логин должен содержать домен @yandex.ru.", "Ошибка регистрации", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            if (!login.EndsWith("@yandex.ru", StringComparison.OrdinalIgnoreCase))
            {
                MessageBox.Show("Логин должен заканчиваться доменом @yandex.ru.", "Ошибка регистрации", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            if (login.StartsWith("@yandex.ru"))
            {
                MessageBox.Show("Логин не может начинаться с домена @yandex.ru. Введите имя пользователя перед доменом.",
                               "Ошибка регистрации", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            if (login.IndexOf("@yandex.ru") == 0)
            {
                MessageBox.Show("Логин должен содержать имя пользователя перед доменом @yandex.ru.",
                               "Ошибка регистрации", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            if (UserManager.RegisterUser(login, password))
            {
                MessageBox.Show("Регистрация прошла успешно!", "Успех", MessageBoxButton.OK, MessageBoxImage.Information);
                MainWindow mainWindow = new MainWindow();
                mainWindow.Show();
                this.Close();
            }
            else
            {
                MessageBox.Show("Пользователь с таким логином уже существует.", "Ошибка регистрации", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void BtnBackToMain_Click(object sender, RoutedEventArgs e)
        {
            MainWindow mainWindow = new MainWindow();
            mainWindow.Show();
            this.Close();
        }
    }
}

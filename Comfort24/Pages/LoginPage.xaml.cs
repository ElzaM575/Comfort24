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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Comfort24.Pages
{
    /// <summary>
    /// Логика взаимодействия для LoginPage.xaml
    /// </summary>
    public partial class LoginPage : Page
    {
        public LoginPage()
        {
            InitializeComponent();
        }

        private void enter_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrEmpty(login.Text) || string.IsNullOrEmpty(password.Password))
            {

                MessageBox.Show("Заполните все поля", "Ошика авторизации", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
            ;//проверка данных
            try
            {
                var userOb = Connection.DBConnection.comfort.Logins.FirstOrDefault(x => x.Login == login.Text.Trim() && x.Password == password.Password.Trim());
                if (userOb != null)
                {                   
                    UserClass.User = userOb;
                    NavigationService.Navigate(new PartnerListPage());
                   
                }
                else
                {
                      MessageBox.Show("Неверный логин и пароль!!", "Ошика авторизации", MessageBoxButton.OK, MessageBoxImage.Warning);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при подключение к бд:{ex.Message}", "Уведомление", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
        
        private void Registration_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new RegPage());
        }
    }
}

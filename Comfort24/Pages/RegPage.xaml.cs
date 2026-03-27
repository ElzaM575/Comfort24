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
using Comfort24.Connection;

namespace Comfort24.Pages
{
    /// <summary>
    /// Логика взаимодействия для RegPage.xaml
    /// </summary>
    public partial class RegPage : Page
    {
        public RegPage()
        {
            InitializeComponent();
            LoadComboxes();
        }
        private void LoadComboxes()
        {
            try
            {
                familyStatusCombo.ItemsSource = DBConnection.comfort.FamilyStatus.ToList();
               
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void registration_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(login.Text) ||
               string.IsNullOrWhiteSpace(password.Password) ||
               string.IsNullOrWhiteSpace(surname.Text) ||
               string.IsNullOrWhiteSpace(name.Text) ||
               birthday.SelectedDate == null ||
               string.IsNullOrWhiteSpace(passportSeries.Text) ||
               string.IsNullOrWhiteSpace(passportNumber.Text) ||
               familyStatusCombo.SelectedItem == null ||
               healthCombo.SelectedItem == null ||
               positionCombo.SelectedItem == null)
            {
                MessageBox.Show("Заполните все поля!", "Ошибка регистрации",
                          MessageBoxButton.OK, MessageBoxImage.Warning);
            }
            else
            {
                if (DBConnection.comfort.Logins.Count(x => x.Login == login.Text) > 0)
                {
                    MessageBox.Show("Такой пользователь уже сущеаствует", "Ошибка регистрации",
                         MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }
                try
                {
                    Employee employeesObj = new Employee
                    {
                        Surname = surname.Text.Trim(),
                        Name = name.Text.Trim(),
                        Patronumic = string.IsNullOrWhiteSpace(patronymic.Text) ? null : patronymic.Text.Trim(),
                        Birthday = birthday.SelectedDate.Value,
                        PassportSeria = passportSeries.Text.Trim(),
                        PassportNumber = passportNumber.Text.Trim(),
                        Id_family = (int)familyStatusCombo.SelectedValue,
                        Id_health = (int)healthCombo.SelectedValue,
                        Id_role = 3
                    };

                    DBConnection.comfort.Employee.Add(employeesObj);
                    DBConnection.comfort.SaveChanges();

                    Logins loginsObj = new Logins
                    {
                        Login = login.Text.Trim(),
                        Password = password.Password.Trim(),
                        Id_user = employeesObj.Id_employee
                    };

                    MessageBox.Show("Данные успешно добавлены", "Уведомление",
                        MessageBoxButton.OK, MessageBoxImage.Information);
                }
                catch
                {
                    MessageBox.Show("Ошибка при добавлении данных", "Ошибка регистрации",
                        MessageBoxButton.OK, MessageBoxImage.Warning);
                }
            }
        }

        private void back_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.GoBack();
        }
    }
}

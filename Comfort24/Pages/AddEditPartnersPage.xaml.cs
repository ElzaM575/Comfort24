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
    /// Логика взаимодействия для AddEditPartnersPage.xaml
    /// </summary>
    public partial class AddEditPartnersPage : Page
    {
        Partners partners;
        public AddEditPartnersPage(Partners _partner)
        {
            InitializeComponent();
            partners = _partner;
            this.DataContext = partners;
            TypeCB.ItemsSource = DBConnection.comfort.TypeOfBusiness.ToList();
            TypeCB.DisplayMemberPath = "NameBusiness";
        }

        private void SaveBtn_Click(object sender, RoutedEventArgs e)
        {
            try
            {
            partners.Id_type=(TypeCB.SelectedItem as TypeOfBusiness).Id_type;
            if(partners.Id_partner==0)
                {
                    DBConnection.comfort.Partners.Add(partners);
                }
            DBConnection.comfort.SaveChanges();
            MessageBox.Show("Успешно");
            NavigationService.Navigate(new PartnerListPage());
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
           
        }

        private void CancleBtn_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}

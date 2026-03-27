using System;
using System.Collections.Generic;
using System.Data.Common;
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
    /// Логика взаимодействия для PartnerListPage.xaml
    /// </summary>
    public partial class PartnerListPage : Page
    {
        List<Partners> allPartners;
        public PartnerListPage()
        {
            InitializeComponent();
            //PartnersLW.ItemsSource = DBConnection.comfort.Partners.ToList();
            allPartners = DBConnection.comfort.Partners.ToList();
            PartnersLW.ItemsSource = allPartners;

            try
            {
                var businessTypes = DBConnection.comfort.TypeOfBusiness.ToList();
                foreach (var type in businessTypes)
                {
                    FilterCombo.Items.Add(type);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error loading filters: " + ex.Message);
            }

        }



        

        private void SearchBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            ApplySearchAndFilter();
        }

        private void FilterCombo_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ApplySearchAndFilter();
        }

        private void ClearFilterBtn_Click(object sender, RoutedEventArgs e)
        {
            FilterCombo.SelectedIndex = 0;
            SearchBox.Text = "";
            ApplySearchAndFilter();
        }

        private void ApplySearchAndFilter()
        {
            if (allPartners == null) return;

            var result = allPartners.AsEnumerable();

            // Search
            if (!string.IsNullOrWhiteSpace(SearchBox.Text))
            {
                string searchText = SearchBox.Text.ToLower();
                result = result.Where(x =>
                    (x.NamePartner != null && x.NamePartner.ToLower().Contains(searchText)) ||
                    (x.SurnameDirector != null && x.SurnameDirector.ToLower().Contains(searchText)) ||
                    (x.NameDirector != null && x.NameDirector.ToLower().Contains(searchText)) ||
                    (x.Phone != null && x.Phone.ToLower().Contains(searchText))
                );
            }

            // Filter
            if (FilterCombo.SelectedItem is TypeOfBusiness selectedType)
            {
                result = result.Where(x => x.Id_type == selectedType.Id_type);
            }

            PartnersLW.ItemsSource = result.ToList();
        }

        private void DeleteBtn_Click(object sender, RoutedEventArgs e)
        {
            var selectedPartner = PartnersLW.SelectedItem as Partners;
            if (selectedPartner != null)
            {
                var result = MessageBox.Show($"Delete partner {selectedPartner.NamePartner}?",
                    "Confirm", MessageBoxButton.YesNo);

                if (result == MessageBoxResult.Yes)
                {
                    try
                    {
                        DBConnection.comfort.Partners.Remove(selectedPartner);
                        DBConnection.comfort.SaveChanges();

                        // Refresh data
                        allPartners = DBConnection.comfort.Partners.ToList();
                        ApplySearchAndFilter();

                        MessageBox.Show("Deleted successfully!");
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Error deleting: " + ex.Message);
                    }
                }
            }
            else
            {
                MessageBox.Show("Please select a partner!");
            }
        }

        private void historyBtn_Click(object sender, RoutedEventArgs e)
        {
            var selectedPartner = PartnersLW.SelectedItem as Partners;
            if (selectedPartner != null)
            {
                NavigationService.Navigate(new HistoryPage(selectedPartner));
            }
            else
            {
                MessageBox.Show("Пожалуйста, выберите партнера!",
                    "Внимание", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }

        private void AddBtn_Click_1(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new AddEditPartnersPage(new Partners()));
        }

        private void EditBtn_Click_1(object sender, RoutedEventArgs e)
        {
            var selPartner = PartnersLW.SelectedItem as Partners;
            if (selPartner != null)
            {
                NavigationService.Navigate(new AddEditPartnersPage(selPartner));
            }
            else
            {
                MessageBox.Show("Не выбран партнер для редактирования!!!!");
            }
        }
    }
}

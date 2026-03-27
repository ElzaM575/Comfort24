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
    /// Логика взаимодействия для HistoryPage.xaml
    /// </summary>
    public partial class HistoryPage : Page
    {
        private Partners _partners;
        public HistoryPage(Partners partner)
        {
            InitializeComponent();
            _partners = partner;
            LoadHistory();
        }
        private void LoadHistory()
        {
            try
            {
                PartnerName.Text = _partners.NamePartner;
                PartnerInfo.Text = $"Тип: {_partners.TypeOfBusiness?.NameBusiness ?? "Не указан"} | Телефон: {_partners.Phone ?? "Не указан"}";
                TitleText.Text = $"История заказов: {_partners.NamePartner}";

                // Получаем заказы партнера
                var requests = DBConnection.comfort.Request
                    .Where(r => r.Id_partner == _partners.Id_partner)
                    .OrderByDescending(r => r.RequestDate)
                    .ToList();

                var salePoints = DBConnection.comfort.SalePoint
                   .Where(sp => sp.Id_partner == _partners.Id_partner)
                   .ToList(); 

                if (requests.Count > 0)
                {
                    var sales = requests.SelectMany(r => r.RequestDetails)
                        .Select(rd => new
                        {
                            ProductName = rd.Products.NameProduct,
                            Quantity = rd.Quantity,
                            Amount = rd.Products.MinPrice * rd.Quantity,
                            RequestDate = rd.Request.RequestDate,
                            Status = rd.Request.Status.NameStatus,
                            PointName = salePoints.Count > 0 ? salePoints.First().NamePoint : "Не указана"
                        })
                        .ToList();

                    SalesListView.ItemsSource = sales;
                    SalesListView.Visibility = Visibility.Visible;
                    EmptyMessage.Visibility = Visibility.Collapsed;

                    // Статистика
                    TotalSalesCount.Text = requests.Count.ToString();
                    TotalProductsCount.Text = sales.Sum(x => x.Quantity).ToString();
                    decimal totalAmount = sales.Sum(x => (decimal)x.Amount);
                    TotalAmount.Text = totalAmount.ToString("N2") + " руб.";
                }
                else
                {
                    SalesListView.Visibility = Visibility.Collapsed;
                    EmptyMessage.Visibility = Visibility.Visible;

                    TotalSalesCount.Text = "0";
                    TotalProductsCount.Text = "0";
                    TotalAmount.Text = "0 руб.";
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ошибка загрузки истории: " + ex.Message, "Ошибка",
                    MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void BackBtn_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.GoBack();
        }
    }

    }

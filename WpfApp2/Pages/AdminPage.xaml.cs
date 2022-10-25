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

namespace WpfApp2
{
    /// <summary>
    /// Логика взаимодействия для AdminPage.xaml
    /// </summary>
    public partial class AdminPage : Page
    {
        public AdminPage()
        {
            InitializeComponent();
            dgUsers.ItemsSource = BaseClass.tBE.UserTable.ToList(); // заполняем DataGrid записями из таблицы БД (UserTable)
        }

        private void btnShowUser_Click(object sender, RoutedEventArgs e) // кнопка для просмотра пользователей в таблице
        {
            dgUsers.Visibility = Visibility.Visible;
            btnShowUser.Visibility = Visibility.Collapsed;
            btnPrivateUser.Visibility = Visibility.Visible;
        }

        private void btnPrivateUser_Click(object sender, RoutedEventArgs e)  // кнопка для скрытия таблицы
        {
            dgUsers.Visibility = Visibility.Collapsed;
            btnShowUser.Visibility = Visibility.Visible;
            btnPrivateUser.Visibility = Visibility.Collapsed;
        }

        private void btnmain_Click(object sender, RoutedEventArgs e)
        {
            Frameclass.MainFrame.Navigate(new MainPage());
        }

        private void btnShowCats_Click(object sender, RoutedEventArgs e)
        {
            Frameclass.MainFrame.Navigate(new ShowCatsPage());
        }
    }
}

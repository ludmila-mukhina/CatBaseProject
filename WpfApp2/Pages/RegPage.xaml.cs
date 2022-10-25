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
    /// Логика взаимодействия для RegPage.xaml
    /// </summary>
    public partial class RegPage : Page
    {
        public RegPage()
        {
            InitializeComponent();
            
            // заполнение comboBox ролями пользователей
            cbRole.ItemsSource = BaseClass.tBE.RoleTable.ToList();  
            cbRole.SelectedValuePath = "idRole";  // 
            cbRole.DisplayMemberPath = "Role";
            cbRole.SelectedIndex = 1;   // отображение в comboBox конкретного элемента при запуске страницы (который будет выбран по-умолчанию)
        }

        private void btnReg_Click(object sender, RoutedEventArgs e)  // регистрация
        {
            // добаление пола (его индекса)
            int g=0;
            if (rbMen.IsChecked == true) g = 1;
            if (rbWomen.IsChecked == true) g = 2;

            // создание объекта, который соответсвует записи в БД, которую потом нужно добавить
            UserTable userTable = new UserTable()
            {
                Surname = tboxSurname.Text,
                Name = tboxName.Text,
                Birthday = Convert.ToDateTime(dpBirthday.SelectedDate),
                Login = tboxLogin.Text,
                Password = pbPassword.Password.GetHashCode(),
                idGender = g,
                idRole = cbRole.SelectedIndex + 1
            };

            BaseClass.tBE.UserTable.Add(userTable);  // добавление записи в таблицу
            BaseClass.tBE.SaveChanges();  // сохранение изменений в базе данных
            MessageBox.Show("Пользователь добавлен");
            Frameclass.MainFrame.Navigate(new MainPage());  // переход на главную страницу
        }

        private void btnBackMain_Click(object sender, RoutedEventArgs e)  // переход на главную страницу
        {
            Frameclass.MainFrame.Navigate(new MainPage());
        }
    }
}

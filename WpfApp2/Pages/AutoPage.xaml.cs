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
    /// Логика взаимодействия для AutoPage.xaml
    /// </summary>
    public partial class AutoPage : Page
    {
        public AutoPage()
        {
            InitializeComponent();
        }

        private void btnBackMain_Click(object sender, RoutedEventArgs e) // переход на главную страницу
        {
            Frameclass.MainFrame.Navigate(new MainPage());
        }

        private void Button_Click(object sender, RoutedEventArgs e) // авторизация
        {
            int p = pbRassword.Password.GetHashCode();  // нахождение хэш-кода пароля
            
            // поиск пользователя в БД по введенному логину и паролю
            UserTable autoUser = BaseClass.tBE.UserTable.FirstOrDefault(x => x.Login == tboxLogin.Text && x.Password == p);

            if (autoUser == null)  // если пользователь не найден
            {
                MessageBox.Show("Пользователя не существует");
            }
            else
            {
                switch(autoUser.idRole)  // если пользователь найден, то смотрим, какая у пользователя роль
                {
                    case 1:  // если администратор
                        Frameclass.MainFrame.Navigate(new AdminPage(autoUser)); // переход в меню администратора
                    break;
                    case 2:  // если пользователь
                        Frameclass.MainFrame.Navigate(new PersonalPage(autoUser)); // переход в личный кабинет пользователя
                        break;
                    default:
                        MessageBox.Show("Пока");
                    break;
                }
            }
        }
    }
}

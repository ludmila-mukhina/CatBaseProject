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

namespace WpfApp2
{
    /// <summary>
    /// Логика взаимодействия для WindowPerson.xaml
    /// </summary>
    public partial class WindowPerson : Window
    {
        UserTable user;  // создаем объект для хранения информации о пользователе
        public WindowPerson(UserTable user)
        {
            InitializeComponent();
            this.user = user;  // заполняем выше созданный объект информацией об авторизованном пользователе
            tbName.Text = user.Name;  // заполняем поле для ввода имени
            tbSurname.Text = user.Surname;  // заполняем поле для ввода фамилии
        } 

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            user.Name = tbName.Text;  // изменяем имя пользователя в БД
            user.Surname = tbSurname.Text;  // изменяем фамилию пользователя в БД
            BaseClass.tBE.SaveChanges();  // сохраняем изменения в БД
            this.Close();  // закрываем это окно
        }
    }
}

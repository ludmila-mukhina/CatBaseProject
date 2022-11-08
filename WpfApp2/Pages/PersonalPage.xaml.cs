using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
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
    /// Логика взаимодействия для PersonalPage.xaml
    /// </summary>
    public partial class PersonalPage : Page
    {
        UserTable user;  // создаем объект для хранения информации о пользователе
        public PersonalPage(UserTable user)
        {
            InitializeComponent();
            this.user = user;  // заполняем выше созданный объект информацией об авторизованном пользователе
            tbName.Text = user.Name;  // заполняем поле с именем
            tbSurname.Text = user.Surname;  // заполняем поле с фамилией
            Userphoto u = BaseClass.tBE.Userphoto.FirstOrDefault(x => x.idUser == user.idUser); // для загрузки картинки находим фото пользователя в таблице, где хранятся фото
            if (u != null)  // если объект не пустой, начинает переводить байтовый массив в изображение
            {
                byte[] Bar = u.photoBinary;   // считываем изображение из базы (считываем байтовый массив двоичных данных)
                BitmapImage BI = new BitmapImage();  // создаем объект для загрузки изображения
                using (MemoryStream m = new MemoryStream(Bar))  // для считывания байтового потока
                {
                    BI.BeginInit();  // начинаем считывание
                    BI.StreamSource = m;  // задаем источник потока
                    BI.CacheOption = BitmapCacheOption.OnLoad;  // переводим изображение
                    BI.EndInit();  // заканчиваем считывание
                } 
                imUser.Source = BI;  // показываем картинку на экране (imUser – имя картиник в разметке)
                imUser.Stretch = Stretch.Uniform;
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)  // открытие окна для редактирования личных данных
        {
            WindowPerson windowPerson = new WindowPerson(user);  // создание объекта окна
            windowPerson.ShowDialog(); // октрытие созданного окна (дальнейший код не будет запущен, пока окно не будет закрыто)
            Frameclass.MainFrame.Navigate(new PersonalPage(user));  // перезагрузка страницы

            
        }

        private void Button_Click_1(object sender, RoutedEventArgs e) // добавление картинки
        {
            try
            {
                Userphoto u = new Userphoto();  // создание объекта для добавления записи в таблицу, где хранится фото
                u.idUser = user.idUser;  // присваиваем значение полю idUser (id авторизованного пользователя)

                OpenFileDialog OFD = new OpenFileDialog();  // создаем диалоговое окно
                OFD.ShowDialog();  // открываем диалоговое окно
                string path = OFD.FileName;  // считываем путь выбранного изображения
                System.Drawing.Image SDI = System.Drawing.Image.FromFile(path);  // создаем объект для загрузки изображения в базу
                ImageConverter IC = new ImageConverter();  // создаем конвертер для перевода картинки в двоичный формат
                byte[] Barray = (byte[])IC.ConvertTo(SDI, typeof(byte[]));  // создаем байтовый массив для хранения картинки
                u.photoBinary = Barray;  // заполяем поле photoBinary полученным байтовым массивом
                BaseClass.tBE.Userphoto.Add(u);  // добавляем объект в таблицу БД
                BaseClass.tBE.SaveChanges();  // созраняем изменения в БД
                MessageBox.Show("Фото добавлено");
                Frameclass.MainFrame.Navigate(new PersonalPage(user)); // перезагружаем страницу

            }
            catch
            {
                MessageBox.Show("Что-то пошло не так");
            }
        }
    }
}

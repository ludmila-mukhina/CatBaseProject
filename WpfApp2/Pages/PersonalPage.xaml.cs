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

        // метод для отображения изображения в личном кабинете. первый аргумент - байтовый массив, в котором хранится изображение в БД, второй аргумент - имя изображения в разметке
        void showImage(byte[] Barray, System.Windows.Controls.Image img)
        {
                BitmapImage BI = new BitmapImage();  // создаем объект для загрузки изображения
                using (MemoryStream m = new MemoryStream(Barray))  // для считывания байтового потока
                {
                    BI.BeginInit();  // начинаем считывание
                    BI.StreamSource = m;  // задаем источник потока
                    BI.CacheOption = BitmapCacheOption.OnLoad;  // переводим изображение
                    BI.EndInit();  // заканчиваем считывание
                }
            img.Source = BI;  // показываем картинку на экране (imUser – имя картиник в разметке)
            img.Stretch = Stretch.Uniform;
        }

        public PersonalPage(UserTable user)
        {
            InitializeComponent();
            this.user = user;  // заполняем выше созданный объект информацией об авторизованном пользователе
            tbName.Text = user.Name;  // заполняем поле с именем
            tbSurname.Text = user.Surname;  // заполняем поле с фамилией
            List<Userphoto> u = BaseClass.tBE.Userphoto.Where(x => x.idUser == user.idUser).ToList(); // для загрузки картинки находим все фото пользователя в таблице, где хранятся фото
            if (u != null)  // если список с фото не пустой, начинает переводить байтовый массив в изображение
            {

                byte[] Bar = u[u.Count-1].photoBinary;   // считываем изображение из базы (считываем байтовый массив двоичных данных) - выбираем последнее добавленное изображение
                showImage(Bar, imUser);  // отображаем картинку
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
                //OFD.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.DesktopDirectory);  // выбор папки для открытия
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

        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            try
            {
                OpenFileDialog OFD = new OpenFileDialog();  // создаем диалоговое окно
                OFD.Multiselect = true;  // открытие диалогового окна с возможностью выбора нескольких элементов
                if (OFD.ShowDialog() == true)  // пока диалоговое окно открыто, будет в цикле записывать каждое выбранное изображение в БД
                {
                    foreach (string file in OFD.FileNames)  // цикл организован по именам выбранных файлов
                    {
                        Userphoto u = new Userphoto();  // создание объекта для добавления записи в таблицу, где хранится фото
                        u.idUser = user.idUser;  // присваиваем значение полю idUser (id авторизованного пользователя)
                        string path = file;  // считываем путь выбранного изображения
                        System.Drawing.Image SDI = System.Drawing.Image.FromFile(file);  // создаем объект для загрузки изображения в базу
                        ImageConverter IC = new ImageConverter();  // создаем конвертер для перевода картинки в двоичный формат
                        byte[] Barray = (byte[])IC.ConvertTo(SDI, typeof(byte[]));  // создаем байтовый массив для хранения картинки
                        u.photoBinary = Barray;  // заполяем поле photoBinary полученным байтовым массивом
                        BaseClass.tBE.Userphoto.Add(u);  // добавляем объект в таблицу БД
                    }
                    BaseClass.tBE.SaveChanges();
                    MessageBox.Show("Фото добавлены");
                }
            }
            catch
            {
                MessageBox.Show("Что-то пошло не так");
            }
        }

        int n = 0;
        private void Button_Click_3(object sender, RoutedEventArgs e)
        {
            spGallery.Visibility = Visibility.Visible;
            List<Userphoto> u = BaseClass.tBE.Userphoto.Where(x => x.idUser == user.idUser).ToList();
            if (u != null)  // если объект не пустой, начинает переводить байтовый массив в изображение
            {

                byte[] Bar = u[n].photoBinary;   // считываем изображение из базы (считываем байтовый массив двоичных данных)
                showImage(Bar, imgGallery);  // отображаем картинку
            }
        }

        private void Next_Click(object sender, RoutedEventArgs e)
        {
            List<Userphoto> u = BaseClass.tBE.Userphoto.Where(x => x.idUser == user.idUser).ToList();
            n++;
            if (Back.IsEnabled == false)
            {
                Back.IsEnabled = true;
            }
            if (u != null)  // если объект не пустой, начинает переводить байтовый массив в изображение
                {

                    byte[] Bar = u[n].photoBinary;   // считываем изображение из базы (считываем байтовый массив двоичных данных)
                    showImage(Bar, imgGallery);              
                }
            if (n == u.Count-1)
            {
                Next.IsEnabled = false;
            }
        }

        private void Back_Click(object sender, RoutedEventArgs e)
        {
            List<Userphoto> u = BaseClass.tBE.Userphoto.Where(x => x.idUser == user.idUser).ToList();
            n--;
            if (Next.IsEnabled==false)
            {
                Next.IsEnabled = true;
            }
            if (u != null)  // если объект не пустой, начинает переводить байтовый массив в изображение
            {

                byte[] Bar = u[n].photoBinary;   // считываем изображение из базы (считываем байтовый массив двоичных данных)
                BitmapImage BI = new BitmapImage();  // создаем объект для загрузки изображения
                showImage(Bar, imgGallery);
            }
            if (n == 0)
            {
                Back.IsEnabled = false;
            }
        }

        private void btnOld_Click(object sender, RoutedEventArgs e)
        {
            List<Userphoto> u = BaseClass.tBE.Userphoto.Where(x => x.idUser == user.idUser).ToList();
            byte[] Bar = u[n].photoBinary;   // считываем изображение из базы (считываем байтовый массив двоичных данных)
            showImage(Bar, imUser);  // отображаем картинку
        }
    }
}

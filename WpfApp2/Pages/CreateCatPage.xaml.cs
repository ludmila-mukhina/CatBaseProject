using Microsoft.Win32;
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
    /// Логика взаимодействия для CreateCatPage.xaml
    /// </summary>
    public partial class CreateCatPage : Page
    { 
        CatTable CAT;  // объект, в котором будет хранится данные о новом или отредактированном коте
        bool flagUpdate = false; // для определения, создаем мы новый объект или редактируем старый
        string path;  // путь к картинке

        public void uploadFields()  // метод для заполнения списков
        {
            cmbBreed.ItemsSource = BaseClass.tBE.BreedTable.ToList();
            cmbBreed.SelectedValuePath = "idBreed";
            cmbBreed.DisplayMemberPath = "Breed";

            cmbGender.ItemsSource = BaseClass.tBE.GenderTable.ToList();
            cmbGender.SelectedValuePath = "idGender";
            cmbGender.DisplayMemberPath = "Gender";

            lbTraits.ItemsSource = BaseClass.tBE.TraitTable.ToList();
            lbTraits.SelectedValuePath = "idTrait";
            lbTraits.DisplayMemberPath = "Trait";

            lbFeed.ItemsSource = BaseClass.tBE.FeedTable.ToList();
        }

        // конструктор для редактирования данных о коте ( с аргументом, который хранит информацию о коте, которого хотим отредактировать)
        public CreateCatPage(CatTable cat)
        {
            InitializeComponent();
            uploadFields(); // заполняем списки
            flagUpdate = true;  // отметка о том, что кота редактируем
            CAT = cat;  // ассоциируем выше созданный глобавльный объект с объектом в кострукторе для дальнейшего редактирования этих данных
            tbName.Text = cat.CatName;  // вывод имени кота
            cmbBreed.SelectedIndex = cat.idBreed - 1;  // вывод породы кота
            dpBirthday.SelectedDate = cat.Birthday;  // вывод даты рождения 
            cmbGender.SelectedIndex = cat.idGender - 1;  // вывод пола
            tbPassport.Text = cat.PassportTable.UniqueNumber;  // вывод паспорта
            tbColor.Text = cat.PassportTable.ColorCat;  // вывод окраса

            // находим черты характера того кота, которого мы редактируем:
            List<TraitCat> tC = BaseClass.tBE.TraitCat.Where(x => x.idCat == cat.idCat).ToList();

            // цикл для выделения черт характера кота в общем списке:
            foreach (TraitTable t in lbTraits.Items)
            {
                if (tC.FirstOrDefault(x => x.idTrait == t.idTrait) != null)
                {
                    lbTraits.SelectedItems.Add(t);
                }
            }

            // находим корма для того кота, которого мы редактируем
            List<FeedCatTable> fct = BaseClass.tBE.FeedCatTable.Where(x => x.idCat == cat.idCat).ToList();

            // цикл для отображения кормов и их количества для кота:
            foreach (FeedTable t in lbFeed.Items)
            {
                if (fct.FirstOrDefault(x => x.idFeed == t.idFeed) != null)
                {
                    t.QM = fct.Count;
                }
            }

            // вывод картинки
            if (cat.Photo != null)
            {
                BitmapImage img = new BitmapImage(new Uri(cat.Photo, UriKind.RelativeOrAbsolute));
                photoCat.Source = img;
            }
        }

        // конструктор для создания нового кота (без аргументов)
        public CreateCatPage()
        {
            InitializeComponent();
            uploadFields();  // заполняем списки
        }

        private void btnAdd_Click(object sender, RoutedEventArgs e)
        {
            try
            {

                // если флаг равен false, то создаем объект для добавления кота
                if (flagUpdate == false)
                {
                    CAT = new CatTable();
                }
                // заполняем поля таблицы CatTable
                CAT.CatName = tbName.Text;
                CAT.idBreed = cmbBreed.SelectedIndex + 1;
                CAT.Birthday = Convert.ToDateTime(dpBirthday.SelectedDate);
                CAT.idGender = cmbGender.SelectedIndex + 1;
                CAT.Photo = path;

                // если флаг равен false, то добавляем объект в базу
                if (flagUpdate == false)
                {
                    BaseClass.tBE.CatTable.Add(CAT);
                }
                // BaseClass.tBE.SaveChanges();


                // Заполнение таблицы PassportTable
                PassportTable pas = new PassportTable()
                {
                    idCat = CAT.idCat,
                    UniqueNumber = tbPassport.Text,
                    ColorCat = tbColor.Text
                };

                // если флаг равен false, то добавляем объект в базу
                if (flagUpdate == false)
                {
                    BaseClass.tBE.PassportTable.Add(pas);
                }
                //    BaseClass.tBE.SaveChanges();

                // Для заполнения таблицы TraitsCats нужно организовать цикл, так как черт характера у кота может быть несколько
                // Цикл будет организовывать по чертам характера, которые выделены в списке

                // находим список черт характера кота:
                List<TraitCat> traits = BaseClass.tBE.TraitCat.Where(x => CAT.idCat == x.idCat).ToList();

                // если список не пустой, удаляем из него все черты характера  этого кота
                if (traits.Count > 0)
                {                   
                    foreach (TraitCat t in traits)
                    {
                        BaseClass.tBE.TraitCat.Remove(t);
                    }
                }

                // перезаписываем черты кота (или добавляем черты для нового кота)
                foreach (TraitTable t in lbTraits.SelectedItems)
                {
                    TraitCat TC = new TraitCat()  // объект для записи в таблицу TraitsCat
                    {
                        idCat = CAT.idCat,
                        idTrait = t.idTrait
                    };
                    BaseClass.tBE.TraitCat.Add(TC);
                }
                //  BaseClass.tBE.SaveChanges();

                // Для заполнения таблицы Diets нужно организовать цикл, так как кормов у кота может быть несколько
                // Цикл будет организовывать по всем кормам, которые есть в списке

                // находим список с кормами для кота
                List<FeedCatTable> feed = BaseClass.tBE.FeedCatTable.Where(x => CAT.idCat == x.idCat).ToList();

                // если список не пустой, удаляем из него все корма для  этого кота
                if (feed.Count > 0)
                {
                    foreach (FeedCatTable t in feed)
                    {
                        BaseClass.tBE.FeedCatTable.Remove(t);
                    }
                }

                // перезаписываем корма для кота (или добавляем корма для нового кота)
                foreach (FeedTable f in lbFeed.Items)
                {
                    if (f.QM > 0)
                    {
                        FeedCatTable FCT = new FeedCatTable()  // объект для записи в таблицу FeedCatTable
                        {
                            idCat = CAT.idCat,
                            idFeed = f.idFeed,
                            Count = f.QM
                        };
                        BaseClass.tBE.FeedCatTable.Add(FCT);
                    }
                }
                BaseClass.tBE.SaveChanges();
                MessageBox.Show("Информация добавлена");
            }
            catch
            {
                MessageBox.Show("Что-то пошло не по плану");
            }
        }

        private void btnPhto_Click(object sender, RoutedEventArgs e)  // добавление фото кота с помощью диалогового окна
        {
            OpenFileDialog OFD = new OpenFileDialog();  // создаем объект диалогового окна
            OFD.ShowDialog();  // открываем диалоговое окно
            path = OFD.FileName;  // извлекаем полный путь к картинке
            string[] arrayPath = path.Split('\\');  // разделяем путь к картинке в массив
            path = "\\" + arrayPath[arrayPath.Length - 2] + "\\" + arrayPath[arrayPath.Length - 1];  // записываем в бд путь, начиная с имени папки
            // MessageBox.Show(path);
        }
    }
}

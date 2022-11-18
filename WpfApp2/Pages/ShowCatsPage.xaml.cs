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
    /// Логика взаимодействия для ShowCatsPage.xaml
    /// </summary>
    public partial class ShowCatsPage : Page
    {
       
        public ShowCatsPage()  // страница со списком котов
        {
            InitializeComponent();
            listCat.ItemsSource = BaseClass.tBE.CatTable.ToList();  // передаем записи из таблицы БД ListView
            
            List<BreedTable> BT = BaseClass.tBE.BreedTable.ToList();
            
            // программное заполнение выпадающего списка
            cmbBreed.Items.Add("Все породы");  // первый элемент выпадающего списка, который сбрасывает фильтрацию
            for (int i=0; i<BT.Count; i++)  // цикл для записи в выпадающий список всех пород котов из БД
            {
                cmbBreed.Items.Add(BT[i].Breed);
            }

            cmbBreed.SelectedIndex = 0;  // выбранное по умолчанию значение в списке с породами котов ("Все породы")
            cmbSort.SelectedIndex = 0;  // выбранное по умолчанию значение в списке с видами сортировки ("Без сортировки")
            
            tbCount.Text = "Количество записей: " + BaseClass.tBE.CatTable.ToList().Count;
        }

        private void tbCharacter_Loaded(object sender, RoutedEventArgs e)  // загрузка черт характера
        {
            TextBlock tb = (TextBlock)sender;  // получаем доступ к TextBlock из шаблона
            int index = Convert.ToInt32(tb.Uid);  // получаем числовой Uid элемента списка (в разметке предварительно нужно связать номер ячейки с номером кота в базе данных)
           
            // ищем в таблице, где хранятся черты характера котов те записи, которые соответсвуют определенному коту
            List<TraitCat> TC = BaseClass.tBE.TraitCat.Where(x=>x.idCat==index).ToList();
            
            
            string str = "";
            
            // выписывам все черты характера в переменную
            foreach (TraitCat tc in TC)
            {
                str += tc.TraitTable.Trait + ", ";
            }

            tb.Text = "Черты характера: " + str.Substring(0, str.Length - 2);  // вывод черт характера на экран
        }

        private void tbMoney_Loaded(object sender, RoutedEventArgs e)  // загрузка информации о том, сколько будет денег тратиться на кота
        {
            TextBlock tb = (TextBlock)sender;  // получаем доступ к TextBlock из шаблона
            int index = Convert.ToInt32(tb.Uid);  // получаем числовой Uid элемента списка (в разметке предварительно нужно связать номер ячейки с номером кота в базе данных)

            // ищем в таблице, где хранятится информация о кормах для кота, которые соответсвуют определенному коту
            List<FeedCatTable> FCT = BaseClass.tBE.FeedCatTable.Where(x=>x.idCat==index).ToList();
            
            int sum = 0;
            
            // вычисляем общее количестов денег на кота, для этого умножаем количество корма на цену корма
            foreach (FeedCatTable ftc in FCT)
            {
                sum += Convert.ToInt32(ftc.Count * ftc.FeedTable.Price);
            }

            tb.Text = "Затраты на корм в месяц: " + sum.ToString()+ " руб.";
        }

        private void Button_Click(object sender, RoutedEventArgs e) // кнопка для удаления информации о коте
        {
            Button btn = (Button)sender;  // получаем доступ к Button из шаблона
            int index = Convert.ToInt32(btn.Uid);  // получаем числовой Uid элемента списка (в разметке предварительно нужно связать номер ячейки с номером кота в базе данных)

            // создаем объект, который содержит информацию о коте, который нужно удалить
            CatTable cat = BaseClass.tBE.CatTable.FirstOrDefault(x => x.idCat == index);

            BaseClass.tBE.CatTable.Remove(cat); // удаление кота из базы            
            BaseClass.tBE.SaveChanges();  // сохранение изменений в базе данных

            Frameclass.MainFrame.Navigate(new ShowCatsPage());  // перезагрузка страницы, чтобы отобразить список без удаленного кота
        }

        private void btnupdate_Click(object sender, RoutedEventArgs e)  // кнопка для перехода к редактированию данных о коте
        {
            Button btn = (Button)sender;  // получаем доступ к Button из шаблона
            int index = Convert.ToInt32(btn.Uid);   // получаем числовой Uid элемента списка (в разметке предварительно нужно связать номер ячейки с номером кота в базе данных)

            // создаем объект, который содержит кота, информацию о котором нужно отредактировать
            CatTable cat = BaseClass.tBE.CatTable.FirstOrDefault(x => x.idCat == index);

            // переход на страницу с редактированием (на ту же самую, где и добавляли кота)
            Frameclass.MainFrame.Navigate(new CreateCatPage(cat));  // в конструктор страницы передаем объект, который был создан строкой выше
        }

        private void btnCreateCat_Click(object sender, RoutedEventArgs e)  // переход на страницу  для создания новой записи о коте во всех таблицах, с ним связанных
        {
            Frameclass.MainFrame.Navigate(new CreateCatPage());
        }


        void Filter()  // метод для одновременной фильтрации, поиска и сортировки
        {
            List<CatTable> catList = new List<CatTable>();  // пустой список, который далее будет заполнять элементами, удавлетворяющими условиям фильтрации, поиска и сортировки
            
            string breed = cmbBreed.SelectedValue.ToString();  // выбранное пользователем название породы
            int index = cmbBreed.SelectedIndex;
            
            // поиск значений, удовлетворяющих условия фильтра
            if (index!=0)
            {
                catList = BaseClass.tBE.CatTable.Where(x => x.BreedTable.Breed == breed).ToList();
            }
            else  // если выбран пункт "Все породы", то сбрасываем фильтрацию:
            {
                catList = BaseClass.tBE.CatTable.ToList();                
            }
            

            // поиск совпадений по именам котов
            if (!string.IsNullOrWhiteSpace(tbSearch.Text))  // если строка не пустая и если она не состоит из пробелов
            {
                catList = catList.Where(x => x.CatName.ToLower().Contains(tbSearch.Text.ToLower())).ToList();
            }


            // выбор элементов только с фото
            if (cbPhoto.IsChecked == true)
            {
                catList = catList.Where(x=>x.Photo!=null).ToList();
            }

            // сортировка
            switch(cmbSort.SelectedIndex)
            {
                case 1:
                    {
                        catList.Sort((x, y) => x.CatName.CompareTo(y.CatName));
                    }
                    break;
                case 2:
                    {
                        catList.Sort((x, y) => x.CatName.CompareTo(y.CatName));
                        catList.Reverse();
                    }
                    break;
            }
           
            listCat.ItemsSource = catList;
            if (catList.Count == 0)
            {
                MessageBox.Show("нет записей");
            }
            tbCount.Text = "Количество записей "+catList.Count;
        }

        // далее во всех обработчиках событий применяем один и тот же метод Filter, который позволяет находить условия, удовлетворяющие всем сразу выбранным параметрам
        private void cmbBreed_SelectionChanged(object sender, SelectionChangedEventArgs e) 
        {
            Filter();
        }

        private void tbSearch_TextChanged(object sender, TextChangedEventArgs e)
        {
            Filter();
        }

        private void cbPhoto_Checked(object sender, RoutedEventArgs e)
        {
            Filter();
        }
    }
}

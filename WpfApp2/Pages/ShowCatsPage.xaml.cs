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
            TextBlock tb = (TextBlock)sender;
            int index = Convert.ToInt32(tb.Uid);
            List<FeedCatTable> FCT = BaseClass.tBE.FeedCatTable.Where(x=>x.idCat==index).ToList();
            int sum = 0;
            foreach (FeedCatTable ftc in FCT)
            {
                sum += Convert.ToInt32(ftc.Count * ftc.FeedTable.Price);
            }
            tb.Text = "Затраты на корм в месяц: " + sum.ToString()+ " руб.";
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Button btn = (Button)sender;
            int index = Convert.ToInt32(btn.Uid);
            CatTable cat = BaseClass.tBE.CatTable.FirstOrDefault(x => x.idCat == index);
            BaseClass.tBE.CatTable.Remove(cat);            
            BaseClass.tBE.SaveChanges();
            Frameclass.MainFrame.Navigate(new ShowCatsPage());
        }

        private void btnupdate_Click(object sender, RoutedEventArgs e)
        {
            Button btn = (Button)sender;
            int index = Convert.ToInt32(btn.Uid);
            CatTable cat = BaseClass.tBE.CatTable.FirstOrDefault(x => x.idCat == index);
            Frameclass.MainFrame.Navigate(new CreateCatPage(cat));
        }
    }
}

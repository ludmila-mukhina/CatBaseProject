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
        public void uploadFields()
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
        CatTable CAT;
        bool flagUpdate = false;
        public CreateCatPage(CatTable cat)
        {
            InitializeComponent();
            uploadFields();
            flagUpdate = true;
            CAT = cat;
            tbName.Text = cat.CatName;
            cmbBreed.SelectedIndex = cat.idBreed - 1;
            dpBirthday.SelectedDate = cat.Birthday;
            cmbGender.SelectedIndex = cat.idGender - 1;
            tbPassport.Text = cat.PassportTable.UniqueNumber;
            tbColor.Text = cat.PassportTable.ColorCat;
            List<TraitCat> tC = BaseClass.tBE.TraitCat.Where(x => x.idCat == cat.idCat).ToList();
            foreach (TraitTable t in lbTraits.Items)
            {
                if (tC.FirstOrDefault(x => x.idTrait == t.idTrait) != null)
                {
                    lbTraits.SelectedItems.Add(t);
                }
            }
            List<FeedCatTable> fct = BaseClass.tBE.FeedCatTable.Where(x => x.idCat == cat.idCat).ToList();
            foreach (FeedTable t in lbFeed.Items)
            {
                if (fct.FirstOrDefault(x => x.idFeed == t.idFeed) != null)
                {
                    t.QM = fct.Count;
                }
            }
            if (cat.Photo != null)
            {
                BitmapImage img = new BitmapImage(new Uri(cat.Photo, UriKind.RelativeOrAbsolute));
                photoCat.Source = img;
            }


        }
        public CreateCatPage()
        {
            InitializeComponent();
            uploadFields();
        }
        string path;
        private void btnAdd_Click(object sender, RoutedEventArgs e)
        {
            if (flagUpdate==false)
            {
                CAT = new CatTable();
            }

            CAT.CatName = tbName.Text;
            CAT.idBreed = cmbBreed.SelectedIndex + 1;
            CAT.Birthday = Convert.ToDateTime(dpBirthday.SelectedDate);
            CAT.idGender = cmbGender.SelectedIndex + 1;
            CAT.Photo = path;
            if (flagUpdate == false)
            {
                BaseClass.tBE.CatTable.Add(CAT);
            }
            // BaseClass.tBE.SaveChanges();

            PassportTable pas = new PassportTable()
            {
                idCat = CAT.idCat,
                UniqueNumber = tbPassport.Text,
                ColorCat = tbColor.Text
            };
            if (flagUpdate == false)
            {
                BaseClass.tBE.PassportTable.Add(pas);
            }
            //    BaseClass.tBE.SaveChanges();

            List<TraitCat> traits = BaseClass.tBE.TraitCat.Where(x => CAT.idCat == x.idCat).ToList();
            if (traits.Count > 0)
            {
                foreach (TraitCat t in traits)
                {
                    BaseClass.tBE.TraitCat.Remove(t);
                }
            }
            foreach (TraitTable t in lbTraits.SelectedItems)
            {
                TraitCat TC = new TraitCat()
                {
                    idCat = CAT.idCat,
                    idTrait = t.idTrait
                };
                BaseClass.tBE.TraitCat.Add(TC);
            }

            //  BaseClass.tBE.SaveChanges();

            List<FeedCatTable> feed = BaseClass.tBE.FeedCatTable.Where(x => CAT.idCat == x.idCat).ToList();
            if (feed.Count > 0)
            {
                foreach (FeedCatTable t in feed)
                {
                    BaseClass.tBE.FeedCatTable.Remove(t);
                }
            }

            foreach (FeedTable f in lbFeed.Items)
            {
                if (f.QM > 0)
                {
                    FeedCatTable FCT = new FeedCatTable()
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

        private void btnPhto_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog OFD = new OpenFileDialog();
            OFD.ShowDialog();
            path = OFD.FileName;
            string[] arrayPath = path.Split('\\');
            path = "\\" + arrayPath[arrayPath.Length - 2] + "\\" + arrayPath[arrayPath.Length - 1];
            // MessageBox.Show(path);
        }
    }
}

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
        public CreateCatPage()
        {
            InitializeComponent();
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

        private void btnAdd_Click(object sender, RoutedEventArgs e)
        {
            CatTable newCat = new CatTable()
            {
                CatName = tbName.Text,
                idBreed = cmbBreed.SelectedIndex + 1,
                Birthday = Convert.ToDateTime(dpBirthday.SelectedDate),
                idGender = cmbGender.SelectedIndex+1
            };

            BaseClass.tBE.CatTable.Add(newCat);
            BaseClass.tBE.SaveChanges();

            PassportTable pas = new PassportTable()
            {
                idCat = newCat.idCat,
                UniqueNumber = tbPassport.Text,
                ColorCat = tbColor.Text
            };

            BaseClass.tBE.PassportTable.Add(pas);
            BaseClass.tBE.SaveChanges();

           foreach (TraitTable t in lbTraits.SelectedItems)
            {
                TraitCat TC = new TraitCat()
                {
                    idCat = newCat.idCat,
                    idTrait = t.idTrait
                };
                BaseClass.tBE.TraitCat.Add(TC);
            }

            BaseClass.tBE.SaveChanges();


        }
    }
}

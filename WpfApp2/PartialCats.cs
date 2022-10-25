using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace WpfApp2
{
    public partial class CatTable
    {
        public string NameBreed
        {
            get
            {
                return CatName + " | " + BreedTable.Breed;
            }
        }

        public SolidColorBrush GenderColor
        {
            get
            {
                switch(idGender)
                {
                    case 1:
                        return Brushes.LightCyan;
                    case 2:
                        return Brushes.Ivory;
                    default:
                        return Brushes.White;
                }
            }
        }
    }
}

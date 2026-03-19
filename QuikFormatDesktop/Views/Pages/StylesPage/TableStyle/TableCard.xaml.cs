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

namespace QuikFormatDesktop.View.Pages
{
    /// <summary>
    /// Логика взаимодействия для TableCard.xaml
    /// </summary>
    public partial class TableCard : UserControl
    {
        public TableCard()
        {
            InitializeComponent();
        }

        private void ClrPcker_Background_SelectedColorChanged(object sender, RoutedPropertyChangedEventArgs<Color?> e)
        {
            if (ClrPcker_Background.SelectedColor.HasValue)
            {
                Color selectedColor = ClrPcker_Background.SelectedColor.Value;
            }
        }
    }
}

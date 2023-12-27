using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Controls;

namespace Image_Sorter.Controls
{
    public partial class CustomPathItem : UserControl
    {
        public CustomPathItem()
        {
            DataContext = this;
            InitializeComponent();
        }

        private void btnClose_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            var mainGrid = ((MainWindow)System.Windows.Application.Current.MainWindow).customPathControl.mainGrid;
            var customPathItems = ((MainWindow)System.Windows.Application.Current.MainWindow).customPathControl.customPathItems;
            foreach (CustomPathItem customPathItem in customPathItems)
            {
                if (customPathItem == this)
                {
                    mainGrid.Children.Remove(customPathItem);
                    tboxPathItem.Text = "erjgfdgjfhhd"; 
                }
            }
        }
    }
}

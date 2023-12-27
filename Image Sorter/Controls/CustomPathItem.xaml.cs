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
            //remove CustomPathItem from the main grid
            var mainWindow = ((MainWindow)System.Windows.Application.Current.MainWindow);
            var customPathItems = mainWindow.customPathControl.customPathItems;
            for (int i = 0; i < customPathItems.Count(); i++)
            {
                CustomPathItem item = customPathItems[i];
                if (item == this)
                {
                    mainWindow.customPathControl.customPathItems.RemoveAt(i);
                    mainWindow.customPathControl.UpdateGrid();
                    break;
                }
            }
        }
    }
}

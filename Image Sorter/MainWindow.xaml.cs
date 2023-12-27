using System.Windows;

namespace Image_Sorter
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void btnSortImages_Click(object sender, RoutedEventArgs e)
        {
            DispalyPathFormat();
        }

        private void DispalyPathFormat()
        {
            var customPathItems = customPathControl.customPathItems;

            string Path = "";
            foreach (var item in customPathItems)
            {
                Path += item.tboxPathItem.Text + "/";
            }

            tbPathFormat.Text = Path;
        }
    }
}
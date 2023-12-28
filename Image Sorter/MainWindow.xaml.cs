using System.Windows;
using System.Windows.Media;

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

            if (fpsSourceFolder.IsNullOrEmpty()) 
            {
                fpsSourceFolder.ControlBorder.BorderBrush = Brushes.Crimson;
                return;
            }
            fpsSourceFolder.ControlBorder.BorderBrush = Brushes.Black;

            if (fpsDestinationFolder.IsNullOrEmpty()) 
            {
                fpsDestinationFolder.ControlBorder.BorderBrush = Brushes.Crimson;
                return;
            }
            fpsDestinationFolder.ControlBorder.BorderBrush = Brushes.Black;

            string sourcePath = (string)fpsSourceFolder.labelFolderPath.Content;
            string destinationPath = (string)fpsDestinationFolder.labelFolderPath.Content;

            ImageDataExtracter imageDataExtracter = new ImageDataExtracter(sourcePath, destinationPath);
            imageDataExtracter.CopyFiles();
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
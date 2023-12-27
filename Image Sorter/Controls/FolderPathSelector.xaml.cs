using Microsoft.Win32;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Controls;

namespace Image_Sorter.Controls
{
    public partial class FolderPathSelector : UserControl, INotifyPropertyChanged
    {
        public FolderPathSelector()
        {
            DataContext = this;
            InitializeComponent();
        }

        private string defaultText;

        public event PropertyChangedEventHandler? PropertyChanged;

        public string DefaultText
        {
            get 
            { 
                return defaultText;
            }
            set 
            { 
                defaultText = value;
                onPropertyChanged();
            }
        }


        private void btnChoosePath_Click(object sender, RoutedEventArgs e)
        {
            OpenFolderDialog openFolderDialog = new OpenFolderDialog();
            //start at 'This PC'
            openFolderDialog.InitialDirectory = "::{20D04FE0-3AEA-1069-A2D8-08002B30309D}";
            if (openFolderDialog.ShowDialog() == null)
            {
                return;
            }

            tblockDefaultText.Visibility = Visibility.Hidden;
            labelFolderPath.Content = openFolderDialog.FolderName;
            if (string.IsNullOrEmpty(openFolderDialog.FolderName)) 
            {
                tblockDefaultText.Visibility = Visibility.Visible;
            }
            

        }

        private void onPropertyChanged([CallerMemberName] string propertyName = null) => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}

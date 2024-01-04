using System.Windows;
using System.Windows.Controls;

namespace Image_Sorter.Controls
{
    public partial class CustomPathControl : UserControl
    {
        public List<CustomPathItem> customPathItems = new List<CustomPathItem>();
        public CustomPathControl()
        {
            InitializeComponent();  
        }

        private void btnAddPathItem_Click(object sender, RoutedEventArgs e)
        {
            CustomPathItem customPath = new CustomPathItem();
            customPathItems.Add(customPath);

            UpdateGrid();
        }

        public void UpdateGrid()
        {
            mainGrid.Children.Clear();
            mainGrid.ColumnDefinitions.Clear();

            //add CustomPathItems in Grid
            for (int i = 0; i < customPathItems.Count; i++)
            {
                mainGrid.ColumnDefinitions.Add(new ColumnDefinition());
                mainGrid.ColumnDefinitions[i].Width = new GridLength(1, GridUnitType.Star);
                mainGrid.ColumnDefinitions[i].MaxWidth = 200;
                Grid.SetColumn(customPathItems[i], i);

               
                mainGrid.Children.Add(customPathItems[i]);
            }

            //add button column definition
            mainGrid.ColumnDefinitions.Add(new ColumnDefinition());
            mainGrid.ColumnDefinitions[customPathItems.Count].Width = new GridLength(50.0);
            Grid.SetColumn(btnAddPathItem, customPathItems.Count);
            mainGrid.Children.Add(btnAddPathItem);
        }
    }
}

using System.Windows;
using System.Windows.Controls;

namespace Image_Sorter.Controls
{
    public partial class CustomPathControl : UserControl
    {
        public CustomPathControl()
        {
            InitializeComponent();  
        }

        private void btnAddPathItem_Click(object sender, RoutedEventArgs e)
        {
            CustomPathItem customPath = new CustomPathItem();

            int columnDefinitionsCount = mainGrid.ColumnDefinitions.Count;
            mainGrid.ColumnDefinitions.Add(new ColumnDefinition());

            mainGrid.ColumnDefinitions[columnDefinitionsCount-1].Width = new GridLength(1, GridUnitType.Star);
            mainGrid.ColumnDefinitions[columnDefinitionsCount-1].MaxWidth = 100;
            mainGrid.ColumnDefinitions[columnDefinitionsCount].Width = new GridLength(50.0);

            Grid.SetColumn(customPath, columnDefinitionsCount-1); 
            Grid.SetColumn(btnAddPathItem, columnDefinitionsCount);
            mainGrid.Children.Add(customPath);
        }
    }
}

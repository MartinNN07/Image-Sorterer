﻿using System.Windows;
using System.Windows.Input;
using System.Windows.Media;
using static System.Runtime.InteropServices.JavaScript.JSType;

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
            if (fpsSourceFolder.IsNullOrEmpty()) 
            {
                fpsSourceFolder.ControlBorder.BorderBrush = Brushes.Crimson;
                return;
            }
            fpsSourceFolder.ControlBorder.BorderBrush = (Brush)(new BrushConverter().ConvertFrom("#594d82")); 
            if (fpsDestinationFolder.IsNullOrEmpty()) 
            {
                fpsDestinationFolder.ControlBorder.BorderBrush = Brushes.Crimson;
                return;
            }
            fpsDestinationFolder.ControlBorder.BorderBrush = (Brush)(new BrushConverter().ConvertFrom("#594d82"));

            string sourcePath = (string)fpsSourceFolder.labelFolderPath.Content;
            string destinationPath = (string)fpsDestinationFolder.labelFolderPath.Content;

            ImageDataExtracter imageDataExtracter = new ImageDataExtracter(sourcePath, destinationPath);
            imageDataExtracter.CopyFiles();
        }

        private void btnClose_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void btnMinimize_Click(object sender, RoutedEventArgs e)
        {
            WindowState = WindowState.Minimized;
        }

        private void controlHeader_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ChangedButton == MouseButton.Left)
                this.DragMove();
        }
    }
}
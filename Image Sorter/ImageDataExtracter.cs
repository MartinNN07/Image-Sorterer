using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using Image_Sorter.Controls;
using MetadataExtractor;
using MetadataExtractor.Formats.Exif;
using static Image_Sorter.MainWindow;

namespace Image_Sorter
{
    internal class ImageDataExtracter
    {
        public enum EXIFTags
        {
            NOTEXIF = 0,
            DATE_TAKEN_ID = 0x9003,
            CAMERA_NAME_ID = 0x110,
            F_NUMBER_ID = 0x829d,
            EXPOSURE_TIME_ID = 0x829a,
            ISO_SPEED_ID = 0x8827
        }

        private struct FileData
        {
            public FileData(FileInfo fileInfo, string dateTime)
            {
                FileInfo = fileInfo;
                Date = dateTime;
            }

            public FileInfo FileInfo { get; }
            public string Date { get; }

            internal void Deconstruct(out FileInfo file, out string date)
            {
                file = FileInfo;
                date = Date;
            }
        }

        private string m_SourcePath, m_DestinationPath;
        private List<FileData> files = new List<FileData>();
        private const int DATE_TAKEN_ID = 0x9003;
        public ImageDataExtracter(string sourcePath, string destinationPath)
        {
            m_SourcePath = sourcePath;
            m_DestinationPath = destinationPath;
        }
        private string ExtractEXIFData(FileInfo file, EXIFTags exifTag)
        {

            IEnumerable<MetadataExtractor.Directory> directories = ImageMetadataReader.ReadMetadata(file.FullName);

            var subIfdDirectory = directories.OfType<ExifSubIfdDirectory>().FirstOrDefault();
            var ifd0Directory = directories.OfType<ExifIfd0Directory>().FirstOrDefault();


            string exifProperty = "";

            if (exifTag == EXIFTags.CAMERA_NAME_ID)
                exifProperty = ifd0Directory?.GetDescription((int)exifTag);

            else
                exifProperty = subIfdDirectory?.GetDescription((int)exifTag);

            if (exifProperty != null)
                return exifProperty;

            return string.Empty;
        }

        private string CreateDestinationPath(FileInfo file)
        {
            if (file == null)
                return string.Empty;

            var mainWindow = ((MainWindow)System.Windows.Application.Current.MainWindow);
            var customPathItems = mainWindow.customPathControl.customPathItems;

            string path = "";
            foreach (var item in customPathItems)
            {
                EXIFTags tag = new EXIFTags();
                switch (item.tboxPathItem.Text)
                {
                    case "[DATE TAKEN]":
                        tag = EXIFTags.DATE_TAKEN_ID;
                        break;

                    case "[CAMERA NAME]":
                        tag = EXIFTags.CAMERA_NAME_ID;
                        break;

                    case "[F-NUMBER]":
                        tag = EXIFTags.F_NUMBER_ID;
                        break;

                    case "[EXPOSURE TIME]":
                        tag = EXIFTags.EXPOSURE_TIME_ID;
                        break;

                    case "[ISO SPEED]":
                        tag = EXIFTags.ISO_SPEED_ID;
                        break;

                    default:
                        tag = EXIFTags.NOTEXIF;
                        break;

                }

                string newFolderName = string.Empty;
                if (tag == EXIFTags.NOTEXIF)
                    newFolderName = item.tboxPathItem.Text;
                
                else if (tag == EXIFTags.DATE_TAKEN_ID)
                    newFolderName = FormatDate(ExtractEXIFData(file, tag));

                else
                    newFolderName = ExtractEXIFData(file, tag);

                if (tag != EXIFTags.DATE_TAKEN_ID)
                    newFolderName = FormatFolderName(newFolderName);

                path = Path.Combine(path, newFolderName);
            }

             return path;

        }

        private string FormatFolderName(string folderName)
        {
            folderName = folderName.Replace("/", "-");
            folderName = folderName.Replace("\\", "-");
            folderName = folderName.Replace(":", "-");
            folderName = folderName.Replace("*", "-");
            folderName = folderName.Replace("|", "-");
            folderName = folderName.Replace("<", "-");
            folderName = folderName.Replace(">", "-");
            folderName = folderName.Replace("?", "-");

            return folderName;
        }
        public void CopyFiles()
        {
            DirectoryInfo dir = new DirectoryInfo(m_SourcePath);
            FileInfo[] imageFiles = dir.GetFiles("*.jpg");

            foreach (var image in imageFiles)
            {
                string destinationFolderPath = Path.Combine(m_DestinationPath, CreateDestinationPath(image));
                string destinationFilePath = Path.Combine(destinationFolderPath, image.Name);

                System.IO.Directory.CreateDirectory(destinationFolderPath);

                if (File.Exists(destinationFilePath))
                {
                    File.SetAttributes(destinationFilePath, FileAttributes.Normal);
                }

                image.CopyTo(destinationFilePath, true);

            }
        }
        private string FormatDate(string date)
        {
            DateTime _date = DateTime.ParseExact(date, "yyyy:MM:dd HH:mm:ss", null);
            string monthName = _date.ToString("MMMM");
            monthName = char.ToUpper(monthName[0]) + monthName.Substring(1);
            string formattedString = Path.Combine(_date.ToString("yyyy"), _date.ToString($"yyyy-MM ({monthName})"));
            

            return formattedString;
        }

    }
}

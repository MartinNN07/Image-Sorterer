using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using MetadataExtractor;
using MetadataExtractor.Formats.Exif;

namespace Image_Sorter
{
    internal class ImageDataExtracter
    {
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

        private void ExtractEXIFData()
        {
            DirectoryInfo dir = new DirectoryInfo(m_SourcePath);
            FileInfo[] imageFiles = dir.GetFiles("*.jpg");

            foreach (FileInfo file in imageFiles)
            {
                IEnumerable<MetadataExtractor.Directory> directories = ImageMetadataReader.ReadMetadata(file.FullName);

                var subIfdDirectory = directories.OfType<ExifSubIfdDirectory>().FirstOrDefault();
                var dateTime = subIfdDirectory?.GetDescription(DATE_TAKEN_ID);

                if (dateTime != null) 
                    files.Add(new FileData(file, dateTime));

            }
        }

        public void CopyFiles()
        {
            this.ExtractEXIFData();
            foreach ((FileInfo file, string date) in files)
            {
                DateTime currDate = ToDate(date);

                string folderName = currDate.ToString("Y");

                string destinationFolderPath = Path.Combine(m_DestinationPath, folderName);
                string destinationFilePath = Path.Combine(destinationFolderPath, file.Name);

                System.IO.Directory.CreateDirectory(destinationFolderPath);

                if (File.Exists(destinationFilePath))
                {
                    File.SetAttributes(destinationFilePath, FileAttributes.Normal);
                }

                file.CopyTo(destinationFilePath, true);

            }
        }
        private DateTime ToDate(string date)
        {
            DateTime _date = DateTime.ParseExact(date, "yyyy:MM:dd HH:mm:ss", null);
            return _date;
        }

    }
}

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
            public FileData(FileInfo fileName, string dateTime)
            {
                FileName = fileName;
                Date = dateTime;
            }

            public FileInfo FileName { get; }
            public string Date { get; }
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
            foreach (FileData file in files)
            {
                DateTime currDate = ToDate(file.Date);

                string folderName = currDate.ToString("Y");

                string destinationFolder = m_DestinationPath + '\\' + folderName;
                string destinationFile = destinationFolder + '\\' + file.FileName.Name;

                System.IO.Directory.CreateDirectory(destinationFolder);
                File.Copy(file.FileName.FullName, destinationFile, true);

            }
        }
        private DateTime ToDate(string date)
        {
            DateTime _date = DateTime.ParseExact(date, "yyyy:MM:dd HH:mm:ss", null);
            return _date;
        }

    }
}

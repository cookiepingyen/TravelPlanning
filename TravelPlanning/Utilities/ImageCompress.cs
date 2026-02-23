using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace TravelPlanning.Utilities
{
    public class ImageCompress
    {
        public static BitmapImage Compress(BitmapImage bitmapImage, int quality = 15)
        {
            // 1. 建立 JpegBitmapEncoder
            JpegBitmapEncoder encoder = new JpegBitmapEncoder();

            // 2. 設定品質 (1-100)
            // 數值越高畫質越好，常見值如 75-80 可兼顧品質與檔案大小 [4]
            encoder.QualityLevel = quality;

            // 3. 將影像資料加入編碼器
            encoder.Frames.Add(BitmapFrame.Create(bitmapImage));


            MemoryStream memoryStream = new MemoryStream();
            encoder.Save(memoryStream);
            memoryStream.Position = 0;

            BitmapImage bitmap = new BitmapImage();
            bitmap.BeginInit();
            bitmap.CacheOption = BitmapCacheOption.OnLoad;
            bitmap.StreamSource = memoryStream;
            bitmap.EndInit();
            bitmap.Freeze();


            return bitmap;
        }


        public static void Save(BitmapImage bitmapImage, string filePath)
        {
            writePreCheck(filePath);
            var encoder = new JpegBitmapEncoder();
            encoder.Frames.Add(BitmapFrame.Create(bitmapImage));

            using (FileStream fs = new FileStream(filePath, FileMode.Create))
            {
                encoder.Save(fs);
            }
        }


        private static void writePreCheck(string path)
        {
            if (!PathExist(path) && !CreateDirPath(path))
            {
                throw new Exception("建立資料夾失敗");
            }
        }

        private static bool PathExist(string path)
        {
            string[] array = path.Split('\\');
            string[] value = array.Take(array.Length - 1).ToArray();
            string path2 = string.Join("\\", value);
            return Directory.Exists(path2);
        }

        private static bool CreateDirPath(string path)
        {
            string text = "";
            string[] array = path.Split('\\');
            string[] value = array.Take(array.Length - 1).ToArray();
            text = string.Join("\\", value);
            DirectoryInfo directoryInfo = Directory.CreateDirectory(text);
            return directoryInfo.Exists;
        }
    }
}

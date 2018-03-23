using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebPDemo
{
    class Program
    {
        static Stopwatch sw = new Stopwatch();
        static void Main(string[] args)
        {
            DirectoryInfo d = new DirectoryInfo(@".\pic\");
            FileSystemInfo[] dirs = d.GetFileSystemInfos();
            foreach (var item in dirs)
            {
                try {
                    long a = System.Environment.TickCount;
                    //var str=encodeWebP(Image.FromFile(item.FullName),item.FullName);
                    var str = encodeNormal(Image.FromFile(item.FullName), item.FullName);
                    a = System.Environment.TickCount - a;
                    Console.WriteLine(str+"。。。"+a);
                }
                catch
                {

                }
            }
            //Console.WriteLine(f.CreationTime);
            Console.ReadLine();
        }
        /// <summary>
        /// 将实际位置中的照片转化为byte[]类型写入数据库中
        /// </summary>
        /// <param name="strFile">string图片地址</param>
        /// <returns>byte[]</returns>
        public static byte[] GetBytesByImagePath(string strFile)
        {
            byte[] photo_byte = null;
            using (FileStream fs =
            new FileStream(strFile, FileMode.Open, FileAccess.Read))
            {
                using (BinaryReader br = new BinaryReader(fs))
                {
                    photo_byte = br.ReadBytes((int)fs.Length);
                }
            }

            return photo_byte;
        }
        public static string encodeWebP(Image pic,string name)
        {
            sw.Restart();
            var bmp = new Bitmap(pic.Width, pic.Height, PixelFormat.Format32bppArgb);
            var getImg = sw.Elapsed.Milliseconds;
            using (var g = Graphics.FromImage(bmp))
            {
                g.DrawImage(pic, 0, 0, pic.Width, pic.Height);
            }
            var cgImg = sw.ElapsedMilliseconds;
            using (FileStream fs = System.IO.File.Create(name.Replace("pic","out")+".webp"))
            {
                new Imazen.WebP.SimpleEncoder().Encode(bmp, fs, 75);
            }
            GC.Collect();
            var webp = sw.ElapsedMilliseconds;
            return string.Format("耗时，提取{0}ms，转化{1}ms，压缩成webp{2}ms，总计{3}ms",getImg,cgImg-getImg,webp-cgImg,webp);
        }
        public static string encodeNormal(Image pic, string name)
        {
            sw.Restart();
            var bmp = new Bitmap(pic.Width, pic.Height, PixelFormat.Format32bppArgb);
            var getImg = sw.Elapsed.Milliseconds;
            using (var g = Graphics.FromImage(bmp))
            {
                g.DrawImage(pic, 0, 0, pic.Width, pic.Height);
            }
            var cgImg = sw.ElapsedMilliseconds;
            ImgResize.GetPicThumbnail(bmp, name.Replace("pic", "outpng") + ".jpg",75);
            GC.Collect();
            var webp = sw.ElapsedMilliseconds;
            return string.Format("耗时，提取{0}ms，转化{1}ms，压缩成算法{2}ms，总计{3}ms", getImg, cgImg - getImg, webp - cgImg, webp);
        }
    }
}

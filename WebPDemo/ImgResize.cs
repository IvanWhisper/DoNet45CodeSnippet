using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Text;

namespace WebPDemo
{
    public class ImgResize
    {
        /// <summary>
        /// 压缩图片
        /// </summary>  
        /// <param name="sFile">原图片</param>    
        /// <param name="dFile">压缩后保存位置</param>    
        /// <param name="dHeight">高度</param>    
        /// <param name="dWidth">宽度</param>    
        /// <param name="flag">压缩质量(数字越小压缩率越高) 1-100</param>    
        /// <returns></returns>    
        public static bool GetPicThumbnail(string sFile, string dFile, int dHeight, int dWidth, int flag)
        {
            System.Drawing.Image iSource = System.Drawing.Image.FromFile(sFile);
            ImageFormat tFormat = iSource.RawFormat;
            int sW = 0, sH = 0;

            //按比例缩放  
            Size tem_size = new Size(iSource.Width, iSource.Height);

            if (tem_size.Width > dHeight || tem_size.Width > dWidth)
            {
                if ((tem_size.Width * dHeight) > (tem_size.Width * dWidth))
                {
                    sW = dWidth;
                    sH = (dWidth * tem_size.Height) / tem_size.Width;
                }
                else
                {
                    sH = dHeight;
                    sW = (tem_size.Width * dHeight) / tem_size.Height;
                }
            }
            else
            {
                sW = tem_size.Width;
                sH = tem_size.Height;
            }

            Bitmap ob = new Bitmap(dWidth, dHeight);
            Graphics g = Graphics.FromImage(ob);

            g.Clear(Color.WhiteSmoke);
            g.CompositingQuality = CompositingQuality.HighQuality;
            g.SmoothingMode = SmoothingMode.HighQuality;
            g.InterpolationMode = InterpolationMode.HighQualityBicubic;

            g.DrawImage(iSource, new Rectangle((dWidth - sW) / 2, (dHeight - sH) / 2, sW, sH), 0, 0, iSource.Width, iSource.Height, GraphicsUnit.Pixel);

            g.Dispose();
            //以下代码为保存图片时，设置压缩质量    
            EncoderParameters ep = new EncoderParameters();
            long[] qy = new long[1];
            qy[0] = flag;//设置压缩的比例1-100    
            EncoderParameter eParam = new EncoderParameter(System.Drawing.Imaging.Encoder.Quality, qy);
            ep.Param[0] = eParam;
            try
            {
                ImageCodecInfo[] arrayICI = ImageCodecInfo.GetImageEncoders();
                ImageCodecInfo jpegICIinfo = null;
                for (int x = 0; x < arrayICI.Length; x++)
                {
                    if (arrayICI[x].FormatDescription.Equals("JPEG"))
                    {
                        jpegICIinfo = arrayICI[x];
                        break;
                    }
                }
                if (jpegICIinfo != null)
                {
                    ob.Save(dFile, jpegICIinfo, ep);//dFile是压缩后的新路径    
                }
                else
                {
                    ob.Save(dFile, tFormat);
                }
                return true;
            }
            catch
            {
                return false;
            }
            finally
            {
                iSource.Dispose();
                ob.Dispose();
            }
        }
        /// <summary>
        /// 压缩图片
        /// </summary>
        /// <param name="img">图片</param>
        /// <param name="dFile">压缩后保存位置</param>    
        /// <param name="dHeight">高度</param>    
        /// <param name="dWidth">宽度</param>    
        /// <param name="flag">压缩质量(数字越小压缩率越高) 1-100</param>    
        /// <returns></returns>
        public static bool GetPicThumbnail(Image img, string outPath, int flag)
        {
            System.Drawing.Image iSource = img;
            ImageFormat tFormat = iSource.RawFormat;

            //以下代码为保存图片时，设置压缩质量  
            EncoderParameters ep = new EncoderParameters();
            long[] qy = new long[1];
            qy[0] = flag;//设置压缩的比例1-100  
            EncoderParameter eParam = new EncoderParameter(System.Drawing.Imaging.Encoder.Quality, qy);
            ep.Param[0] = eParam;
            try
            {
                ImageCodecInfo[] arrayICI = ImageCodecInfo.GetImageEncoders();
                ImageCodecInfo jpegICIinfo = null;
                for (int x = 0; x < arrayICI.Length; x++)
                {
                    if (arrayICI[x].FormatDescription.Equals("JPEG"))
                    {
                        jpegICIinfo = arrayICI[x];
                        break;
                    }
                }
                if (jpegICIinfo != null)
                {
                    iSource.Save(outPath, jpegICIinfo, ep);//dFile是压缩后的新路径  
                }
                else
                {
                    iSource.Save(outPath, tFormat);
                }
                return true;
            }
            catch
            {
                return false;
            }
            finally
            {
                iSource.Dispose();
                iSource.Dispose();
            }
        }
        /// <summary>    
        /// 生成缩略图 静态方法    
        /// </summary>    
        /// <param name="pathImageFrom"> 源图的路径(含文件名及扩展名) </param>    
        /// <param name="pathImageTo"> 生成的缩略图所保存的路径(含文件名及扩展名)    
        /// 注意：扩展名一定要与生成的缩略图格式相对应 </param>    
        /// <param name="width"> 欲生成的缩略图 "画布" 的宽度(像素值) </param>    
        /// <param name="height"> 欲生成的缩略图 "画布" 的高度(像素值) </param>    
        public static void GetThumbnail(Image imageFrom, string pathImageFrom, string pathImageTo, int width, int height)
        {
            try
            {
                if (imageFrom == null)
                {
                    if (File.Exists(pathImageFrom))
                        imageFrom = Image.FromFile(pathImageFrom);
                    else
                        return;
                }
            }
            catch
            {
                return;
            }
            // 源图宽度及高度    
            int imageFromWidth = imageFrom.Width;
            int imageFromHeight = imageFrom.Height;
            // 生成的缩略图实际宽度及高度    
            int bitmapWidth = width;
            int bitmapHeight = height;
            // 生成的缩略图在上述"画布"上的位置    
            int X = 0;
            int Y = 0;
            // 根据源图及欲生成的缩略图尺寸,计算缩略图的实际尺寸及其在"画布"上的位置    
            if (bitmapHeight * imageFromWidth > bitmapWidth * imageFromHeight)
            {
                bitmapHeight = imageFromHeight * width / imageFromWidth;
                Y = (height - bitmapHeight) / 2;
            }
            else
            {
                bitmapWidth = imageFromWidth * height / imageFromHeight;
                X = (width - bitmapWidth) / 2;
            }
            // 创建画布    
            Bitmap bmp = new Bitmap(width, height);
            Graphics g = Graphics.FromImage(bmp);
            // 用白色清空    
            g.Clear(Color.White);
            // 指定高质量的双三次插值法。执行预筛选以确保高质量的收缩。此模式可产生质量最高的转换图像。    
            g.InterpolationMode = InterpolationMode.HighQualityBicubic;
            // 指定高质量、低速度呈现。    
            g.SmoothingMode = SmoothingMode.HighQuality;
            // 在指定位置并且按指定大小绘制指定的 Image 的指定部分。    
            g.DrawImage(imageFrom, new Rectangle(X, Y, bitmapWidth, bitmapHeight), new Rectangle(0, 0, imageFromWidth, imageFromHeight), GraphicsUnit.Pixel);

            //经测试 .jpg 格式缩略图大小与质量等最优    
            bmp.Save(pathImageTo, ImageFormat.Jpeg);

            //释放资源      
            imageFrom.Dispose();
            bmp.Dispose();
            g.Dispose();
        }

        /// <summary>
        /// 保持比例压缩图片
        /// </summary>
        /// <param name="originalImage">图片</param>
        /// <param name="toWidth">1024</param>
        /// <param name="toHeight">768</param>
        /// <param name="saveFileName">压缩后保存位置</param>
        /// <param name="qualityflg">压缩质量(数字越小压缩率越高) 1-100</param>
        /// <returns>Bitmap</returns>
        public static Bitmap GetPicThumbnailAt(Bitmap originalImage, string saveFileName, int qualityflg, bool issave, int toWidth = 1024, int toHeight = 768)
        {
            Bitmap compressImage = null;
            //如果尺寸不够返回保存原图
            if (originalImage.Width < toWidth && originalImage.Height < toHeight)
            {
                if (!string.IsNullOrEmpty(saveFileName) && issave)
                {
                    originalImage.Save(saveFileName);
                    //originalImage.Dispose();
                }
                return originalImage;
            }
            //根据图片大小获取新图片从原图片截取的区域
            int x = 0, y = 0, w, h;
            Dictionary<string, int> imgsize = AdjustSize(toWidth, toHeight, originalImage.Width, originalImage.Height);
            w = imgsize["Width"];
            h = imgsize["Height"];
            //Bitmap bm = new Bitmap(toWidth, toHeight);
            Bitmap bm = new Bitmap(w, h);
            Graphics g = Graphics.FromImage(bm);
            //g.SmoothingMode = SmoothingMode.HighQuality;
            g.SmoothingMode = SmoothingMode.HighQuality;
            g.InterpolationMode = InterpolationMode.HighQualityBicubic;
            //g.Clear(Color.White);
            //清除整个绘图面并以透明背景色填充 
            g.Clear(Color.Transparent);
            //g.DrawImage(originalImage, new Rectangle(x, y, w, h), 0, 0, originalImage.Width, originalImage.Height, GraphicsUnit.Pixel);

            g.DrawImage(originalImage, new Rectangle(x, y, w, h), 0, 0, originalImage.Width, originalImage.Height, GraphicsUnit.Pixel);

            long[] quality = new long[1];
            quality[0] = qualityflg;
            EncoderParameters encoderParams = new EncoderParameters();
            EncoderParameter encoderParam = new EncoderParameter(System.Drawing.Imaging.Encoder.Quality, quality);
            encoderParams.Param[0] = encoderParam;
            ImageCodecInfo[] arrayICI = ImageCodecInfo.GetImageEncoders();//获得包含有关内置图像编码解码器的信息的ImageCodecInfo 对象。
            ImageCodecInfo jpegICI = null;
            for (int i = 0; i < arrayICI.Length; i++)
            {
                if (arrayICI[i].FormatDescription.Equals("JPEG"))
                {
                    jpegICI = arrayICI[i];//设置JPEG编码
                    break;
                }
            }
            if (jpegICI != null)
            {
                MemoryStream mystream = new MemoryStream();
                bm.Save(mystream, jpegICI, encoderParams);
                //Image img = Image.FromStream(mystream);
                //compressImage = (Bitmap)img;//new Bitmap(mystream);
                Image img = Image.FromStream(mystream);
                //img.Save(saveFileName.Substring(0, saveFileName.LastIndexOf("."))+"4.jpg");
                //Bitmap bmap = new Bitmap(mystream);
                compressImage = new Bitmap(w, h);
                g = Graphics.FromImage(compressImage);
                //g.SmoothingMode = SmoothingMode.HighQuality;
                g.SmoothingMode = SmoothingMode.HighQuality;
                g.InterpolationMode = InterpolationMode.HighQualityBilinear;//HighQualityBicubic;
                //g.Clear(Color.White);
                //清除整个绘图面并以透明背景色填充 
                g.Clear(Color.Transparent);
                //g.DrawImage(originalImage, new Rectangle(x, y, w, h), 0, 0, originalImage.Width, originalImage.Height, GraphicsUnit.Pixel);
                g.DrawImage(img, new Rectangle(x, y, w, h), 0, 0, originalImage.Width, originalImage.Height, GraphicsUnit.Pixel);

                mystream.Close();
                mystream.Dispose();
                mystream = null;
                if (!string.IsNullOrEmpty(saveFileName) && issave)
                {
                    //bm.Save(Server.MapPath(path + "/thumb_" + filename), jpegICI, encoderParams);
                    bm.Save(saveFileName, jpegICI, encoderParams);
                }
            }
            originalImage.Dispose();
            g.Dispose();
            //compressImage = bm.Clone(new Rectangle(x, y, w, h),PixelFormat.Format24bppRgb);
            bm.Dispose();

            return compressImage;
        }
        /// <summary>
        /// 保持比例图像缩放简易算法
        /// </summary>
        /// <param name="spcWidth"></param>
        /// <param name="spcHeight"></param>
        /// <param name="orgWidth"></param>
        /// <param name="orgHeight"></param>
        /// <returns></returns>
        public static Dictionary<string, int> AdjustSize(int spcWidth, int spcHeight, int orgWidth, int orgHeight)
        {
            Dictionary<string, int> size = new Dictionary<string, int>();
            // 原始宽高在指定宽高范围内，不作任何处理  
            if (orgWidth <= spcWidth && orgHeight <= spcHeight)
            {
                size["Width"] = orgWidth;
                size["Height"] = orgHeight;
            }
            else
            {
                // 取得比例系数  
                float w = orgWidth / (float)spcWidth;
                float h = orgHeight / (float)spcHeight;
                // 宽度比大于高度比  
                if (w > h)
                {
                    size["Width"] = spcWidth;
                    size["Height"] = (int)(w >= 1 ? Math.Round(orgHeight / w) : Math.Round(orgHeight * w));
                }
                // 宽度比小于高度比  
                else if (w < h)
                {
                    size["Height"] = spcHeight;
                    size["Width"] = (int)(h >= 1 ? Math.Round(orgWidth / h) : Math.Round(orgWidth * h));
                }
                // 宽度比等于高度比  
                else
                {
                    size["Width"] = spcWidth;
                    size["Height"] = spcHeight;
                }
            }
            return size;
        }
    }
}

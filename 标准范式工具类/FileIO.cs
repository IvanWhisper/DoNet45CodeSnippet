using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace 标准范式工具类
{
    public class FileIO
    {
        /// <summary>
        /// 文件读取地址
        /// </summary>
        private string readPath;
        /// <summary>
        /// 文件写入地址
        /// </summary>
        private string writePath;
        /// <summary>
        /// 缓冲字节数组长度
        /// </summary>
        private byte[] buffer;
        /// <summary>
        /// 字节流长度
        /// </summary>
        private int bufferSize;

        public string ReadPath
        {
            get
            {
                return readPath;
            }

            set
            {
                readPath = value;
            }
        }

        public string WritePath
        {
            get
            {
                return writePath;
            }

            set
            {
                writePath = value;
            }
        }

        public FileIO()
        {

        }
        public FileIO(int bufferSizeMax)
        {
            bufferSize = bufferSizeMax;
            buffer = new byte[bufferSize];
        }
        /// <summary>
        /// 读写小文件
        /// </summary>
        public void ReadStreamFromFile()
        {
            System.IO.FileStream stream = null;
            stream = new System.IO.FileStream(ReadPath, System.IO.FileMode.Open);
            FileStream fsWrite = File.OpenWrite(WritePath);

            long fileLength = stream.Length;//文件流的长度
            double a = (double)(fileLength) / (double)bufferSize;
            int readCount = (int)Math.Ceiling((double)(fileLength) / (double)bufferSize); //需要对文件读取的次数
            try
            {
                for(int i=0;i< readCount;i++)
                {
                    int curbufferSize =(int)( bufferSize < (fileLength - fsWrite.Length) ? bufferSize : (fileLength - fsWrite.Length));
                    Console.WriteLine(curbufferSize);
                    stream.Read(buffer, 0, curbufferSize);
                    //分readCount次读取这个文件流，每次从上次读取的结束位置开始读取bufferSize个字节
                    //这里加入接收和处理数据的逻辑
                    fsWrite.Write(buffer, 0, curbufferSize);
                    long[] data = new long[2];
                    data[0] = fileLength;
                    data[1] = fsWrite.Length;
                }
            }
            catch
            {

            }
            finally
            {
                if (stream != null)
                    stream.Dispose();
                fsWrite.Close();
                //MessageBox.Show(readCount.ToString());
            }
        }
        /// <summary>
        /// 读写大文件
        /// </summary>
        private void ReadStreamFromLargeFile()
        {
            string filePath = @".\in.sql";
            int bufferSize = 1024000; //每次读取的字节数
            byte[] buffer = new byte[bufferSize];
            System.IO.FileStream stream = null;

            stream = new System.IO.FileStream(filePath, System.IO.FileMode.Open);

            FileStream fsWrite = File.OpenWrite(@".\out.sql");

            long fileLength = stream.Length;//文件流的长度
            double a = (double)(fileLength) / (double)bufferSize;
            int readCount = (int)Math.Ceiling((double)(fileLength) / (double)bufferSize); //需要对文件读取的次数
            int tempCount = 0;//当前已经读取的次数

            //MessageBox.Show(readCount.ToString());

            try
            {
                do
                {
                    stream.Read(buffer, 0, bufferSize);
                    //分readCount次读取这个文件流，每次从上次读取的结束位置开始读取bufferSize个字节
                    //这里加入接收和处理数据的逻辑
                    fsWrite.Write(buffer, 0, bufferSize);
                    tempCount++;
                    //
                }
                while (tempCount < readCount);
            }
            catch
            {

            }
            finally
            {
                if (stream != null)
                    stream.Dispose();
                fsWrite.Close();
                //MessageBox.Show(readCount.ToString());
            }
        }
    }
}

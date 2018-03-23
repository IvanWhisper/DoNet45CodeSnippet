using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace 后台CMD回调执行
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            string cmd = "echo 你好";//耗时的语句
            textBox.Text = cmd;
            string path = @" abc";
            StartCmd(path, cmd);
            label.Content = File.Exists(@".\textfile.txt");
        }
        public void StartCmd(String workingDirectory, String command)
        {
            Process p = new Process();
            p.OutputDataReceived += new DataReceivedEventHandler(OutputHandler);
            p.StartInfo.FileName = "cmd.exe";
            // p.StartInfo.WorkingDirectory = workingDirectory;
            p.StartInfo.UseShellExecute = false;
            p.StartInfo.RedirectStandardInput = true;
            p.StartInfo.RedirectStandardOutput = true;
            p.StartInfo.RedirectStandardError = true;
            p.StartInfo.CreateNoWindow = true;
            p.Start();
            p.BeginOutputReadLine();
            p.StandardInput.WriteLine(command);
            p.StandardInput.WriteLine("exit");
        }

        private void OutputHandler(object sender, DataReceivedEventArgs e)
        {
            //throw new NotImplementedException();
            this.Dispatcher.Invoke(() => {

                listBox.Items.Add(e.Data);
                if (e.Data == null)
                    return;
                if (e.Data.Contains("exit"))
                {
                    MessageBox.Show("ok");
                    label1.Content = File.Exists(@".\textfile.txt");
                    if (!IsFileInUse(@".\textfile.txt"))
                    {
                        MessageBox.Show("未占用");
                    }

                }
            });
        }
        public bool IsFileInUse(string fileName)
        {
            bool inUse = true;

            FileStream fs = null;
            try
            {
                fs = new FileStream(fileName, FileMode.Open, FileAccess.Read,
                FileShare.None);
                inUse = false;
            }
            catch
            {
            }
            finally
            {
                if (fs != null)
                    fs.Close();
            }
            return inUse;//true表示正在使用,false没有使用  
        }
    }
}

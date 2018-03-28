using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
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

namespace 死锁研究
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            msg(Thread.CurrentThread.ManagedThreadId.ToString());
        }

        private async void button_Click(object sender, RoutedEventArgs e)
        {
            msg(Thread.CurrentThread.ManagedThreadId.ToString());
            await Task.Delay(1000).ConfigureAwait(false);
            msg(Thread.CurrentThread.ManagedThreadId.ToString());
        }
        public void msg(string id,string str=null)
        {
            this.Dispatcher.Invoke(()=> {
                listBox.Items.Add(string.Format("线程ID[{0}],{1}", id,str));
            });
        }
        public async Task Doasync()
        {
            msg(Thread.CurrentThread.ManagedThreadId.ToString());
            await Task.Delay(1000);
            msg(Thread.CurrentThread.ManagedThreadId.ToString());
        }

        private void button1_Click(object sender, RoutedEventArgs e)
        {
            Doasync();
        }
        public async Task DoasyncFalse()
        {
            msg(Thread.CurrentThread.ManagedThreadId.ToString());
            await Task.Delay(1000).ConfigureAwait(false);
            msg(Thread.CurrentThread.ManagedThreadId.ToString());
        }

        private async void button2_Click(object sender, RoutedEventArgs e)
        {
            msg(Thread.CurrentThread.ManagedThreadId.ToString(),"开始");
            await DoasyncFalse().ConfigureAwait(false);
            msg(Thread.CurrentThread.ManagedThreadId.ToString(), "yield");
            await Task.Yield();
            msg(Thread.CurrentThread.ManagedThreadId.ToString(), "结束");
        }
    }
}

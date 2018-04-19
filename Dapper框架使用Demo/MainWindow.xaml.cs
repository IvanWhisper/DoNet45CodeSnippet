using Dapper;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
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

namespace Dapper框架使用Demo
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : Window
    {
        int count=0;
        //参考 https://github.com/StackExchange/Dapper
        string conn = "Server=127.0.0.1;Database=;Uid=;Pwd=;Charset=utf8";
        public MainWindow()
        {
            InitializeComponent();
            //Query 查询
            using (var connection = new MySqlConnection(conn))
            {
                var admin = connection.Query<Admin>("select  * from admin where FUserName= @FUserName", new { FUserName = "000000" });
                dataGrid.ItemsSource = admin;
            }
    }

        private void button_Click(object sender, RoutedEventArgs e)
        {
            //Execute 执行
            using (var connection = new MySqlConnection(conn))
            {
                var result = connection.Execute("insert into admin set FUserName=@FUserName,ApiUrl=@ApiUrl", new[] { new { FUserName = "1", ApiUrl = "222" }, new { FUserName = "@", ApiUrl = "2" } });
            }
            //WithTran 事务
            string sql1 = "delete from admin where FUserName<>'000000'";
            string sql2 = "ErrorSQL delete from admin where FUserName<>'000000'";

            using (var connection = new MySqlConnection(conn))
            {
                connection.Open();
                MySqlTransaction transaction =connection.BeginTransaction();
                try
                {
                    connection.Execute(sql1, null, transaction);
                    connection.Execute(sql2, null, transaction);
                    transaction.Commit();
                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                }
                finally
                {
                    transaction.Dispose();
                    connection.Clone();
                }


            }
        }
    }
}

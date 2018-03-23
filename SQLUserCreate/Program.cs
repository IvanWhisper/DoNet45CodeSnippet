using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SQLUserCreate
{
    class Program
    {
        static void Main(string[] args)
        {
            //建用户
            var cuser1sql = "GRANT USAGE ON *.* TO 'test1'@'localhost' IDENTIFIED BY '1234' WITH GRANT OPTION";
            var cuser2sql = "GRANT USAGE ON *.* TO 'test2'@'localhost' IDENTIFIED BY '1234' WITH GRANT OPTION";
            var cuser3sql = "GRANT USAGE ON *.* TO 'test3'@'localhost' IDENTIFIED BY '1234' WITH GRANT OPTION";
            var cuser4sql = "GRANT USAGE ON *.* TO 'test4'@'localhost' IDENTIFIED BY '1234' WITH GRANT OPTION";
            //授权
            var guser1sql = "GRANT SELECT ON yundaclient.*TO 'test1'@'localhost' IDENTIFIED BY '1234'";
            var guser2sql = "GRANT SELECT, INSERT ON yundaclient.*TO 'test1'@'localhost' IDENTIFIED BY '1234'";
            var guser3sql = "GRANT SELECT, INSERT, UPDATE ON yundaclient.*TO 'test1'@'localhost' IDENTIFIED BY '1234'";
            var guser4sql = "GRANT SELECT, INSERT, UPDATE, DELETE, CREATE, DROP ON yundaclient.*TO 'test1'@'localhost' IDENTIFIED BY '1234'";
            //回收权限
            var ruser1sql= "revoke all ON yundaclient.* from 'test1' @'localhost'";
            Console.WriteLine(MySqlHelper.ExecuteNonQuery(ruser1sql));
            //建用户
            //Console.WriteLine(MySqlHelper.ExecuteNonQuery(cuser1sql));
            //Console.WriteLine(MySqlHelper.ExecuteNonQuery(cuser2sql));
            //Console.WriteLine(MySqlHelper.ExecuteNonQuery(cuser3sql));
            //Console.WriteLine(MySqlHelper.ExecuteNonQuery(cuser4sql));
            //授权
            //Console.WriteLine(MySqlHelper.ExecuteNonQuery(guser1sql));
            //Console.WriteLine(MySqlHelper.ExecuteNonQuery(guser2sql));
            //Console.WriteLine(MySqlHelper.ExecuteNonQuery(guser3sql));
            //Console.WriteLine(MySqlHelper.ExecuteNonQuery(guser4sql));

            Console.ReadLine();

        }
    }
}

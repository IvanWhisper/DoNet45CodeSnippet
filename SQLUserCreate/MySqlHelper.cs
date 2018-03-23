using System.Data;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;

namespace SQLUserCreate
{
	public class MySqlHelper {
		private static string ConnectionStr = "server=localhost;User Id=root;password=123456;Database=client";
        private static MySqlConnection conForExecuteDataSetV2 = new MySqlConnection(ConnectionStr);
        private static MySqlCommand comForExecuteDataSetV2 = new MySqlCommand();
        private static MySqlConnection longCon = new MySqlConnection(ConnectionStr);
        private static MySqlCommand longCommand = new MySqlCommand();

        //Database=information_schema
        private static string ConnectionStrInfoSchema = "server=localhost;User Id=root;password=123456;Database=client";

        static MySqlHelper()
        {
            longCon.Open();
            longCommand.Connection = longCon;
            longCommand.CommandType = CommandType.Text;
        }
        public static int ExecuteNonQuery(string cmdText) {
            try
            {
                using (var conn = new MySqlConnection(ConnectionStr))
                {
                    conn.Open();
                    using (var cmd = new MySqlCommand())
                    {
                        cmd.Connection = conn;
                        cmd.CommandType = CommandType.Text;
                        cmd.CommandText = cmdText;
                        try
                        {
                            return cmd.ExecuteNonQuery();
                        }
                        catch(Exception ex)
                        {
                            return -100;
                        }
                    }
                }
            }
            catch(Exception ex)
            {
                return -100;
            }
		}
        public static int ExecuteNonQueryLong(string cmdText)
        {
            try
            {
                longCommand.CommandText = cmdText;
                try
                {
                    return longCommand.ExecuteNonQuery();
                }
                catch (Exception ex)
                {
                    return -100;
                }
            }
            catch (Exception ex)
            {
                return -100;
            }
        }
        public static DataSet ExecuteDataSet(string cmdText) {
			using(var conn = new MySqlConnection(ConnectionStr)) {
				conn.Open();
				using(var cmd = new MySqlCommand()) {
					cmd.Connection = conn;
					cmd.CommandType = CommandType.Text;
					cmd.CommandText = cmdText;
					using(var adapter = new MySqlDataAdapter()) {
						adapter.SelectCommand = cmd;
						var ds = new DataSet();
						adapter.Fill(ds);
						return ds;
					}
				}
			}
		}

        public static object ExecuteScalar(string cmdText)
        {
            object result = null;
            using (var conn = new MySqlConnection(ConnectionStr))
            {
                conn.Open();
                using (var cmd = new MySqlCommand())
                {
                    cmd.Connection = conn;
                    cmd.CommandType = CommandType.Text;
                    cmd.CommandText = cmdText;
                    result = cmd.ExecuteScalar();
                }
            }
            return result;
        }


        /// <summary>
        /// 执行多语句，数据回滚
        /// </summary>
        /// <param name="CommandTextList"></param>
        /// <returns>尝试执行成功的返回影响行数，不成功的返回异常，以“#”分割</returns>
        public static List<string> ExecuteNonQueryWithTrans(List<string> CommandTextList)
        {
            List<string> ResultStr = new List<string>();
            if (CommandTextList.Count > 0)
            {
                MySqlConnection mysqlconnection = new MySqlConnection(ConnectionStr);
                mysqlconnection.Open();
                MySqlTransaction mysqltransaction = mysqlconnection.BeginTransaction();
                MySqlCommand mysqlcommand = new MySqlCommand();
                mysqlcommand.Connection = mysqlconnection;
                mysqlcommand.Transaction = mysqltransaction;
                try
                {
                    foreach (string commText in CommandTextList)
                    {
                        mysqlcommand.CommandText = commText;
                        int i = mysqlcommand.ExecuteNonQuery();
                        ResultStr.Add(i.ToString());
                    }
                    ResultStr.Add("true");
                    mysqltransaction.Commit();
                }
                catch (Exception ex)
                {
                    ResultStr.Add(ex.ToString());
                    ResultStr.Add("false");
                    mysqltransaction.Rollback();
                }
                finally
                {
                    mysqlconnection.Close();
                    mysqltransaction.Dispose();
                    mysqlconnection.Dispose();
                }
            }
            return ResultStr;
        }


        /// <summary>
        /// 执行多语句，数据回滚
        /// </summary>
        /// <param name="CommandTextList"></param>
        /// <returns>尝试执行成功的返回影响行数，不成功的返回异常，以“#”分割</returns>
        public static int ExecuteNonQueryWithTransTotal(List<string> CommandTextList)
        {
            int resultCount = 0;
            if (CommandTextList.Count > 0)
            {
                MySqlConnection mysqlconnection = new MySqlConnection(ConnectionStr);
                mysqlconnection.Open();
                MySqlTransaction mysqltransaction = mysqlconnection.BeginTransaction();
                MySqlCommand mysqlcommand = new MySqlCommand();
                mysqlcommand.Connection = mysqlconnection;
                mysqlcommand.Transaction = mysqltransaction;
                try
                {
                    foreach (string commText in CommandTextList)
                    {
                        mysqlcommand.CommandText = commText;
                        int i = mysqlcommand.ExecuteNonQuery();
                        resultCount += i;
                    }
                    mysqltransaction.Commit();
                }
                catch (Exception ex)
                {
                    mysqltransaction.Rollback();
                }
                finally
                {
                    mysqlconnection.Close();
                    mysqltransaction.Dispose();
                    mysqlconnection.Dispose();
                }
            }
            return resultCount;
        }



        /// <summary>
        /// 向表中添加列，已经包含检测是否重复列的功能
        /// </summary>
        /// <param name="tableName">表名</param>
        /// <param name="columnName">列名</param>
        /// <param name="columnType">列的类型，比如 varchar (255)</param>
        public static void AddColumn(string tableName, string columnName, string columnType)
        {
            try
            {
                using (var conn = new MySqlConnection(ConnectionStrInfoSchema))
                {
                    conn.Open();
                    using (var cmd = new MySqlCommand())
                    {
                        cmd.Connection = conn;
                        cmd.CommandType = CommandType.Text;
                        cmd.CommandText = "select COLUMN_NAME from COLUMNS where COLUMN_NAME = '" + columnName + "' and TABLE_NAME = '" + tableName + "'";

                        var ds = new DataSet();
                        using (var adapter = new MySqlDataAdapter())
                        {
                            adapter.SelectCommand = cmd;

                            adapter.Fill(ds);
                        }

                        if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                        {

                        }
                        else
                        {
                            string cmdText = "alter table " + tableName + " add " + columnName + " " + columnType;
                            MySqlHelper.ExecuteNonQuery(cmdText);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
            }
        }

        /// <summary>
        /// 更新现有的索引
        /// </summary>
        /// <param name="indexName">索引的名称</param>
        /// <param name="tableName">表名</param>
        /// <param name="columnList">要包含到里面的列,</param>
        public static void UpdateIndex(string indexName, string tableName, string columnList)
        {
            try
            {
                using (var conn = new MySqlConnection(ConnectionStr))
                {
                    conn.Open();
                    using (var cmd = new MySqlCommand())
                    {
                        try
                        {
                            cmd.Connection = conn;
                            cmd.CommandType = CommandType.Text;
                            string cmdText = "show index from " + tableName + " where Key_name = '" + indexName + "'";
                            cmd.CommandText = cmdText;

                            var ds = new DataSet();
                            using (var adapter = new MySqlDataAdapter())
                            {
                                adapter.SelectCommand = cmd;

                                adapter.Fill(ds);
                            }

                            if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count == 1 || ds.Tables.Count > 0 && ds.Tables[0].Rows.Count == 2)
                            {
                                cmd.CommandText = "alter table " + tableName + " drop index " + indexName;
                                cmd.ExecuteNonQuery();

                                cmd.CommandText = "create unique index " + indexName + " on " + tableName + " (" + columnList + ") ";
                                cmd.ExecuteNonQuery();
                            }
                            else if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count == 0)
                            {
                                cmd.CommandText = "create unique index " + indexName + " on " + tableName + " (" + columnList + ") ";
                                cmd.ExecuteNonQuery();
                            }



                        }
                        catch (Exception ex)
                        {
                        }

                    }
                }
            }
            catch (Exception ex)
            {
            }
        }



    }
}

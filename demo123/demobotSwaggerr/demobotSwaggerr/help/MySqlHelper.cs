using MySql.Data.MySqlClient;
using System;
using System.Data;
using System.Data.Common;
 
namespace MysqlClient
{
	public abstract class MySqlHelper
	{
		private static string dataHost =  "127.0.0.1";

		private static string port = "3306";

		private static string userid =  "root";

		private static string Password =  "123456";

		private static string device_manage =  "chatDemo";

		public static string Conn = "Database='" + device_manage + "';Data Source='" + dataHost + "';User Id='" + userid + "';Password='" + Password + "';charset='utf8';pooling=true;Allow Zero Datetime=True";

		public static int ExecuteNonQuery(string connectionString, CommandType cmdType, string cmdText, params MySqlParameter[] commandParameters)
		{
		 
			MySqlCommand val = (MySqlCommand)(object)new MySqlCommand();
			MySqlConnection val2 = (MySqlConnection)(object)new MySqlConnection(connectionString);
			try
			{
				PrepareCommand(val, val2, null, cmdType, cmdText, commandParameters);
				int result = ((DbCommand)(object)val).ExecuteNonQuery();
				((DbParameterCollection)(object)val.Parameters).Clear();
				return result;
			}
			finally
			{
				((IDisposable)val2)?.Dispose();
			}
		}

		public static int ExecuteNonQuery(MySqlConnection connection, CommandType cmdType, string cmdText, params MySqlParameter[] commandParameters)
		{
		 
			MySqlCommand val = (MySqlCommand)(object)new MySqlCommand();
			PrepareCommand(val, connection, null, cmdType, cmdText, commandParameters);
			int result = ((DbCommand)(object)val).ExecuteNonQuery();
			((DbParameterCollection)(object)val.Parameters).Clear();
			return result;
		}

		public static int ExecuteNonQuery(MySqlTransaction trans, CommandType cmdType, string cmdText, params MySqlParameter[] commandParameters)
		{
			 
			MySqlCommand val = (MySqlCommand)(object)new MySqlCommand();
			PrepareCommand(val, trans.Connection, trans, cmdType, cmdText, commandParameters);
			int result = ((DbCommand)(object)val).ExecuteNonQuery();
			((DbParameterCollection)(object)val.Parameters).Clear();
			return result;
		}

		public static MySqlDataReader ExecuteReader(string connectionString, CommandType cmdType, string cmdText, params MySqlParameter[] commandParameters)
		{
			 
			MySqlCommand val = (MySqlCommand)(object)new MySqlCommand();
			MySqlConnection val2 = (MySqlConnection)(object)new MySqlConnection(connectionString);
			try
			{
				PrepareCommand(val, val2, null, cmdType, cmdText, commandParameters);
				MySqlDataReader result = val.ExecuteReader(CommandBehavior.CloseConnection);
				((DbParameterCollection)(object)val.Parameters).Clear();
				return result;
			}
			catch
			{
				((DbConnection)(object)val2).Close();
				throw;
			}
		}

		public static DataSet GetDataSet(string connectionString, CommandType cmdType, string cmdText, params MySqlParameter[] commandParameters)
		{
			 
			MySqlCommand val = (MySqlCommand)(object)new MySqlCommand();
			MySqlConnection val2 = (MySqlConnection)(object)new MySqlConnection(connectionString);
			try
			{
				PrepareCommand(val, val2, null, cmdType, cmdText, commandParameters);
				MySqlDataAdapter val3 = (MySqlDataAdapter)(object)new MySqlDataAdapter();
				val3.SelectCommand=(val);
				DataSet dataSet = new DataSet();
				((DataAdapter)(object)val3).Fill(dataSet);
				((DbParameterCollection)(object)val.Parameters).Clear();
				((DbConnection)(object)val2).Close();
				return dataSet;
			}         

			catch (Exception ex)
			{
				throw ex;
			}
		}

		public static object ExecuteScalar(string connectionString, CommandType cmdType, string cmdText, params MySqlParameter[] commandParameters)
		{
			//IL_0001: Unknown result type (might be due to invalid IL or missing references)
			//IL_0007: Expected O, but got Unknown
			//IL_0008: Unknown result type (might be due to invalid IL or missing references)
			//IL_000e: Expected O, but got Unknown
			MySqlCommand val = (MySqlCommand)(object)new MySqlCommand();
			MySqlConnection val2 = (MySqlConnection)(object)new MySqlConnection(connectionString);
			try
			{
				PrepareCommand(val, val2, null, cmdType, cmdText, commandParameters);
				object result = ((DbCommand)(object)val).ExecuteScalar();
				((DbParameterCollection)(object)val.Parameters).Clear();
				return result;
			}
			finally
			{
				((IDisposable)val2)?.Dispose();
			}
		}

		public static object ExecuteScalar(MySqlConnection connection, CommandType cmdType, string cmdText, params MySqlParameter[] commandParameters)
		{
			//IL_0001: Unknown result type (might be due to invalid IL or missing references)
			//IL_0007: Expected O, but got Unknown
			MySqlCommand val = (MySqlCommand)(object)new MySqlCommand();
			PrepareCommand(val, connection, null, cmdType, cmdText, commandParameters);
			object result = ((DbCommand)(object)val).ExecuteScalar();
			((DbParameterCollection)(object)val.Parameters).Clear();
			return result;
		}

		private static void PrepareCommand(MySqlCommand cmd, MySqlConnection conn, MySqlTransaction trans, CommandType cmdType, string cmdText, MySqlParameter[] cmdParms)
		{
			if (((DbConnection)(object)conn).State != ConnectionState.Open)
			{
				((DbConnection)(object)conn).Open();
			}
			cmd.Connection=(conn);
			((DbCommand)(object)cmd).CommandText = cmdText;
			if (trans != null)
			{
				cmd.Transaction=(trans);
			}
			((DbCommand)(object)cmd).CommandType = cmdType;
			if (cmdParms != null)
			{
				foreach (MySqlParameter val in cmdParms)
				{
					cmd.Parameters.Add(val);
				}
			}
		}
	}
}

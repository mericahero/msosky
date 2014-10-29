using System;
using System.Collections.Specialized;
using System.Configuration;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;

namespace Utility
{
	public class DB
	{
		public static DB oDB;

		private static string _conStr;

		public static string ConStr
		{
			get
			{
				if (DB._conStr == "")
				{
					DB._conStr = ConfigurationManager.AppSettings["dhtDB"];
				}
				return DB._conStr;
			}
		}

		static DB()
		{
			DB._conStr = "";
			DB.oDB = new DB(DB.ConStr);
		}

		public DB(string constr)
		{
			DB._conStr = constr;
		}

		public int ExecuteNonQuery(string sql)
		{
			int num;
			SqlCommand command = new SqlCommand(sql, GetConnection())
			{
				CommandTimeout = 0
			};
			try
			{
				num = command.ExecuteNonQuery();
			}
			finally
			{
				command.Connection.Close();
			}
			return num;
		}

		public object ExecuteScalar(string sql)
		{
			object obj2;
			SqlCommand command = new SqlCommand(sql, GetConnection());
			try
			{
				obj2 = command.ExecuteScalar();
			}
			finally
			{
				command.Connection.Close();
			}
			return obj2;
		}

		public static object ExecuteScalar(string sql, SqlConnection oConn)
		{
			return (new SqlCommand(sql, oConn)).ExecuteScalar();
		}

		public SqlConnection GetConnection()
		{
			SqlConnection connection2 = new SqlConnection(DB.ConStr);
			connection2.Open();
			return connection2;
		}

		public SqlConnection GetNotOpenConnection()
		{
			return new SqlConnection(DB.ConStr);
		}

		public DataSet GetSQLDataSet(string strsql)
		{
			SqlDataAdapter adapter = new SqlDataAdapter(strsql, GetNotOpenConnection());
			DataSet dataSet = new DataSet();
			adapter.Fill(dataSet);
			return dataSet;
		}

		public DataRowCollection GetSQLRows(string strsql)
		{
			SqlDataAdapter adapter = new SqlDataAdapter(strsql, GetNotOpenConnection());
			DataTable dataTable = new DataTable();
			adapter.Fill(dataTable);
			return dataTable.Rows;
		}

		public static DataRowCollection GetSQLRows(string strsql, SqlConnection oConn)
		{
			SqlDataAdapter adapter = new SqlDataAdapter(strsql, oConn);
			DataTable dataTable = new DataTable();
			adapter.Fill(dataTable);
			return dataTable.Rows;
		}

		public DataRowCollection GetSQLRows(string strsql, int n)
		{
			string[] str = new string[] { "set rowcount ", n.ToString(), " ", strsql, " set rowcount 0" };
			return GetSQLRows(string.Concat(str));
		}

		public DataRowCollection GetSQLRows(string strsql, int start, int n)
		{
			string[] str = new string[] { "set rowcount ", (start + n).ToString(), " ", strsql, " set rowcount 0" };
			strsql = string.Concat(str);
			SqlDataAdapter adapter = new SqlDataAdapter(strsql, GetNotOpenConnection());
			DataSet dataSet = new DataSet();
			adapter.Fill(dataSet, start, n, "t");
			return dataSet.Tables[0].Rows;
		}

		public DataRow GetSQLSingleRow(string strsql)
		{
			DataRow item;
			SqlDataAdapter adapter = new SqlDataAdapter(strsql, GetNotOpenConnection());
			DataTable dataTable = new DataTable();
			if (adapter.Fill(dataTable) != 0)
			{
				item = dataTable.Rows[0];
			}
			else
			{
				item = null;
			}
			return item;
		}

		public static DataRow GetSQLSingleRow(string strsql, SqlConnection oConn)
		{
			DataRow item;
			SqlDataAdapter adapter = new SqlDataAdapter(strsql, oConn);
			DataTable dataTable = new DataTable();
			if (adapter.Fill(dataTable) != 0)
			{
				item = dataTable.Rows[0];
			}
			else
			{
				item = null;
			}
			return item;
		}

		public DataTable GetSQLTab(string strsql)
		{
			SqlDataAdapter adapter = new SqlDataAdapter(strsql, GetNotOpenConnection());
			DataTable dataTable = new DataTable();
			adapter.Fill(dataTable);
			return dataTable;
		}
	}
}
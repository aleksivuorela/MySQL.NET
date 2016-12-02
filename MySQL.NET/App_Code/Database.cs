using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Web;

/// <summary>
/// Class for handling insert, select, update and delete queries to MySQL database
/// Uses parametrized queries to prevent SQL injection
/// </summary>
public class Database
{
    private string cs;
    private List<string> parameters;
    private MySqlCommand cmd;
    private DataTable dt;
    private int affectedRows;

    public Database()
    {
        cs = ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString;
        parameters = new List<string>();
    }

    private void Init(string squery, string[] bindings = null)
    {
        using (MySqlConnection conn = new MySqlConnection(cs))
        {
            conn.Open();
            cmd = new MySqlCommand(squery, conn);

            Bind(bindings);
            // Prevents SQL injection
            if (parameters.Count > 0)
            {
                parameters.ForEach(delegate (string parameter)
                {
                    string[] sparameters = parameter.ToString().Split('\x7F');
                    cmd.Parameters.AddWithValue(sparameters[0], sparameters[1]);
                });
            }

            squery = squery.ToLower();
            if (squery.Contains("select"))
            {
                dt = ExecDatatable();
            }
            if (squery.Contains("delete") || squery.Contains("update") || squery.Contains("insert"))
            {
                affectedRows = ExecNonquery();
            }
            parameters.Clear();
            cmd.Dispose();
        } // Connection closed
    }

    /// <summary>
    /// Bind one parameter
    /// </summary>
    public void Bind(string field, string value)
    {
        parameters.Add("@" + field + "\x7F" + value);
    }

    /// <summary>
    /// Bind multiple parameters
    /// Usage: { param1, value1, param2, value2, ... } 
    /// </summary>
    public void Bind(string[] fields)
    {
        if (fields != null)
        {
            for (int i = 0; i < fields.Length; i++)
            {
                Bind(fields[i], fields[i + 1]);
                i += 1;
            }
        }
    }

    private DataTable ExecDatatable()
    {
        DataTable dt = new DataTable();
        try
        {
            MySqlDataReader reader = cmd.ExecuteReader();
            dt.Load(reader);
        }
        catch (MySqlException)
        {
            throw;
        }
        return dt;
    }

    private int ExecNonquery()
    {
        int affected = 0;
        try
        {
            affected = cmd.ExecuteNonQuery();
        }
        catch (MySqlException)
        {
            throw;
        }
        return affected;
    }

    /// <summary>
    /// Use for SELECT
    /// </summary>
    public DataTable Query(string query, string[] bindings = null)
    {
        Init(query, bindings);
        return dt;
    }

    /// <summary>
    /// Use for UPDATE, INSERT and DELETE
    /// </summary>
    public int nQuery(string query, string[] bindings = null)
    {
        Init(query, bindings);
        return affectedRows;
    }

    /// <summary>
    /// Use for quick SELECT ALL FROM <table>
    /// </summary>
    public DataTable Table(string table, string[] bindings = null)
    {
        Init("SELECT * FROM " + table, bindings);
        return dt;
    }
}
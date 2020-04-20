using System;
using System.Data;
using System.Windows.Forms;
using System.Collections.Generic;
using Mono.Data.Sqlite;

namespace CardManager
{
    public static class DatabaseHelper
    {
    

    public static SqliteCommand CreateCommand(string statement, SqliteConnection connection)
        {
            return new SqliteCommand            
            {
                CommandText = statement,
                CommandType = CommandType.Text,
                Connection = connection
            };
        }

      public static bool ExecuteNonCommand(SqliteCommand command)
      {
        try
        {
            command.ExecuteNonQuery();
            return true;
        }
        catch (Exception ex)
        {
            MessageBox.Show(ex.Message);
            return false;
        }
       }

      public static List<string[]> ExecuteStringCommand(SqliteCommand command, int columncount)
      {
        try
        {
            var values = new List<string[]>();
            SqliteDataReader reader = command.ExecuteReader();
            while (reader.Read())
            {
                var row = new List<string>();
                for (int i = 0; i < reader.FieldCount; i++)
                {
                    row.Add(reader[i].ToString());
                }
                values.Add(row.ToArray());
            }
            reader.Close();
            return values;

        }
        catch (Exception ex)
        {
            MessageBox.Show(ex.Message);
            return new List<string[]>();
        }
      }

      public static int ExecuteIntCommand(SqliteCommand command)
      {
          try
          {
              return Convert.ToInt32(command.ExecuteScalar());
          }
          catch (Exception ex)
          {
              MessageBox.Show(ex.Message);
              return -1;
          }
      }

    }
}

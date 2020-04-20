using System;
using System.Collections.Generic;
using Mono.Data.Sqlite;
using System.Windows.Forms;
using System.IO;
using Ionic.Zip;

namespace CardManager
{
    public static class SQLiteCommands
    {
        public static void RenameKey<TKey, TValue>(this IDictionary<TKey, TValue> dic,
                                      TKey fromKey, TKey toKey)
        {
            TValue value = dic[fromKey];
            dic.Remove(fromKey);
            dic[toKey] = value;
        }

        public static bool UpdateCardId(string cardId, string updatedId, SqliteConnection connection)
        {
            try
            {
                var command = new SqliteCommand("UPDATE datas SET id=@updatedId WHERE id=@cardId", connection);
                var command2 = new SqliteCommand("UPDATE texts SET id=@updatedId WHERE id=@cardId", connection);

                command.Parameters.Add(new SqliteParameter("@updatedId", updatedId));
                command2.Parameters.Add(new SqliteParameter("@updatedId", updatedId));

                command.Parameters.Add(new SqliteParameter("@cardId", cardId));
                command2.Parameters.Add(new SqliteParameter("@cardId", cardId));

                DatabaseHelper.ExecuteNonCommand(command);
                DatabaseHelper.ExecuteNonCommand(command2);
                return true;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "ERROR", MessageBoxButtons.OK);
                return false;
            }
        }

        public static void Extract(string path, string zipfile, string card)
        {
           

                var options = new ReadOptions { StatusMessageWriter = System.Console.Out };
                using (ZipFile zip = ZipFile.Read(zipfile, options))
                {

                    foreach (ZipEntry entry in zip)
                        if (entry.FileName == card)
                        entry.Extract(path, ExtractExistingFileAction.OverwriteSilently);
                    
              
                }
            

        }

        public static void ExtractEntries(string path, string zipfile)
        {

            var options = new ReadOptions { StatusMessageWriter = System.Console.Out };
            using (ZipFile zip = ZipFile.Read(zipfile, options))
            {

                foreach (ZipEntry entry in zip)
                        entry.Extract(path, ExtractExistingFileAction.OverwriteSilently);


            }

        }


        public static bool SaveCard
            (int updateid,int cardid,int ot,int cardalias,int atk,int def,
            int setcode,int type, int lvl,int race,int attribute, int category)
        {
            try
            {
                SqliteCommand command;
                if (updateid != cardid)
                {
                    command = new SqliteCommand("INSERT INTO datas(id,ot,alias,setcode,type,atk,def,level,race,attribute,category)" +
                    " VALUES (@id, @ot, @alias, @setcode, @type, @atk, @def, @level, @race, @attribute, @category)" +
                    " ON DUPLICATE KEY UPDATE id=@id, ot=@ot, alias=@alias, setcode=@setcode, type=@type, atk=@atk, def=@def, level=@level, race=@race, attribute=@attribute, category=@category"
                    );
                }
                else
                {
                    command = new SqliteCommand("UPDATE id=@id, ot=@ot, alias=@alias, setcode=@setcode, type=@type, atk=@atk, def=@def, level=@level, race=@race, attribute=@attribute, category=@category WHERE id=@loadedid");
                }

                command.Parameters.Add(new SqliteParameter("@loadedid", updateid));
                command.Parameters.Add(new SqliteParameter("@id", cardid));
                command.Parameters.Add(new SqliteParameter("@ot", ot));
                command.Parameters.Add(new SqliteParameter("@alias", cardalias));
                command.Parameters.Add(new SqliteParameter("@setcode", setcode));
                command.Parameters.Add(new SqliteParameter("@type", type));
                command.Parameters.Add(new SqliteParameter("@atk", atk));
                command.Parameters.Add(new SqliteParameter("@def", def));
                command.Parameters.Add(new SqliteParameter("@level", lvl));
                command.Parameters.Add(new SqliteParameter("@race", race));
                command.Parameters.Add(new SqliteParameter("@attribute", attribute));
                command.Parameters.Add(new SqliteParameter("@category", category));
                DatabaseHelper.ExecuteNonCommand(command);
                return true;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "ERROR", MessageBoxButtons.OK);
                return false;
            }
        }

        public static bool ExecuteSqlCommand(string commandstring)
        {
            try
            {
                var command = new SqliteCommand(commandstring);
                DatabaseHelper.ExecuteNonCommand(command);
                return true;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "ERROR", MessageBoxButtons.OK);
                return false;
            }
        }
    }
}

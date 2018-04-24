using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;
using System.Data;
using System.Windows.Forms;

namespace Integrator
{
    class Integrator
    {
        private SqlConnection con1 { get; set; }
        private SqlConnection con2 { get; set; }

        public Integrator(String url1, String url2)
        {
            con1 = new SqlConnection(url1);
            con2 = new SqlConnection(url2);
        }
        //it shows rows which our data has
        private void ShowData(DataSet dane)
        {
            foreach (DataTable table in dane.Tables)
            {
                Console.WriteLine(table.TableName);
                DataRowCollection wiersze = table.Rows;
                DataColumnCollection columny = table.Columns;
                foreach (DataRow row in wiersze)
                {
                    StringBuilder buff = new StringBuilder();
                    foreach (DataColumn col in columny)
                    {
                        buff.Append(row[col] + " | ");
                    }
                    Console.WriteLine(buff.ToString());
                }
                try
                {
                    Console.WriteLine(table.PrimaryKey[0].ColumnName);
                }
                catch(IndexOutOfRangeException e)
                {
                    Console.WriteLine("W tabeli nie ma kluczy głównych ");
                }
                
            }
        }
        private void AddPrimaryKeysToDataSet(DataRowCollection wiersze, ref DataSet dane)
        {
            DataColumn[] keys = null;
            foreach (DataRow row in wiersze)
            {
                foreach (DataTable tab in dane.Tables)
                {
                    if (tab.TableName.Equals(row["table_name"]))
                    {
                        foreach (DataColumn col in tab.Columns)
                        {
                            if (col.ColumnName.Equals(row["column_name"]))
                            {
                                DataColumn[] buff = new DataColumn[1];
                                buff[0] = col;
                                tab.PrimaryKey = buff;
                                break;
                            }
                        }
                    }
                }
            }
        }

        public void Integrate()
        {
            con1.Open();
            con2.Open();

            //con1.BeginTransaction();
            DataTable tables = con1.GetSchema("Tables"); //optymistycznie zakładamy
            // ze tabelki w obu bazach sa takie same, ale easy
            // zrobie sprawdzanko
            List<String> namesOfTables = new List<string>();

            foreach (DataRow row in tables.Rows)
            {
                string tablename = (string)row[2];
                Console.WriteLine(tablename);
                namesOfTables.Add(tablename);
            }
            //bierzemy klucze:
            DataTable dt = con1.GetSchema("IndexColumns");
            DataRowCollection wiersze = dt.Rows;
            foreach (DataRow row in wiersze)
            {
                StringBuilder buff = new StringBuilder();
                buff.Append(row["table_name"] + " | ");
                buff.Append(row["column_name"] + " | ");
                buff.Append(row["ordinal_position"] + " | ");
                buff.Append(row["KeyType"] + " | ");
                buff.Append(row["index_name"] + " | ");
                buff.Append(row["constraint_name"] + " | ");
                Console.WriteLine(buff.ToString());
            }
            //bierzemy klucze obce:
            //na razie darujemy sobie to
            
            DataSet data1 = new DataSet();
            DataSet data2 = new DataSet();

            //dobrze działa, spokojnie
            for (int i = 0; i < namesOfTables.Count; i++)
            {
                //bierzemy dane:
                String command = "Select * from " + namesOfTables[i];
                SqlDataAdapter adp1 = new SqlDataAdapter(command, con1);
                SqlDataAdapter adp2 = new SqlDataAdapter(command, con2);
                adp1.Fill(data1, namesOfTables[i]);
                adp2.Fill(data2, namesOfTables[i]);
            }
            //dopisanie kluczy do kolumn
            AddPrimaryKeysToDataSet(wiersze, ref data1);
            AddPrimaryKeysToDataSet(wiersze, ref data2);
            /*DropPrimaryKeysFromDataSet(data1);
            DropPrimaryKeysFromDataSet(data2);*/
            this.ShowData(data1);

            DataSet new_data1 = new DataSet();
            DataSet new_data2 = new DataSet();
            //pierwsze przejście
            foreach(DataTable table in data1.Tables)
            {
                new_data1.Tables.Add(table.Copy());
            }
            foreach (DataTable table in data2.Tables)
            {
                new_data2.Tables.Add(table.Copy());
            }

            //drugi to już właściwa integracja, na razie jest prostacka
            //dziala bo wczytywanie jest skopane, nie bedzie dzialac
            //jak wczytywanie bedzie dobrze
            //już nie działa :-))
            foreach (DataTable table2 in data2.Tables)
            {
                foreach(DataTable tab in new_data1.Tables)
                {
                    if (table2.TableName.Equals(tab.TableName))
                    {
                        foreach(DataRow row in table2.Rows)
                        {
                            try
                            {
                                tab.Rows.Add(row.ItemArray);
                            }
                            catch (System.Data.ConstraintException e)
                            {

                            }
                        }
                    }
                }
            }
            foreach (DataTable table in data1.Tables)
            {
                foreach (DataTable tab in new_data2.Tables)
                {
                    if (table.TableName.Equals(tab.TableName))
                    {
                        foreach (DataRow row in table.Rows)
                        {
                            try
                            {
                                tab.Rows.Add(row.ItemArray);
                            }
                            catch (System.Data.ConstraintException e)
                            {

                            }
                        }
                    }
                }
            }
            this.ShowData(new_data1);
            this.ShowData(new_data2);

            con1.Close();
            con2.Close();
        }
    }
}

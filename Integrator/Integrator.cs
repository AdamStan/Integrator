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
        public DataSet data;

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
                    Console.Write(table.PrimaryKey[0].ColumnName);
                }
                catch(IndexOutOfRangeException e)
                {
                    Console.WriteLine("W tabeli nie ma kluczy głównych ");
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
            DataColumnCollection columny = dt.Columns;
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

            //wyciągnięcie wszystkich tabelek i wsadzenie do secików (to powinna być metoda)
            //jesli chcemy to robic dla roznych baz (w sensie, ze nie muszą mieć takich
            //samych tabel) to trzeba rzucać i łapać wyjątki, na razie mi się nie chce
            //nie działa dobrze, nie zczytuje kluczy i constraintów
            for (int i = 0; i < namesOfTables.Count; i++)
            {
                //bierzemy dane:
                String command = "Select * from " + namesOfTables[i];
                SqlDataAdapter adp1 = new SqlDataAdapter(command, con1);
                SqlDataAdapter adp2 = new SqlDataAdapter(command, con2);
                adp1.Fill(data1, namesOfTables[i]);
                adp2.Fill(data2, namesOfTables[i]);
            }
            this.ShowData(data1);

            data = new DataSet();
            //pierwsze przejście, 
            foreach(DataTable table in data1.Tables)
            {
                data.Tables.Add(table.Copy());
            }
            
            //drugi to już właściwa integracja, na razie jest prostacka
            //dziala bo wczytywanie jest skopane, nie bedzie dzialac
            //jak wczytywanie bedzie dobrze
            foreach (DataTable table2 in data2.Tables)
            {
                foreach(DataTable tab in data.Tables)
                {
                    if (table2.TableName.Equals(tab.TableName))
                    {
                        foreach(DataRow row in table2.Rows)
                        {
                            tab.Rows.Add(row.ItemArray);
                        }
                    }
                }
            }

            this.ShowData(data);
            
            con1.Close();
            con2.Close();
        }
    }
}

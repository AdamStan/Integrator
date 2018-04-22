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
        private void ShowData()
        {
            foreach (DataTable table in data.Tables)
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

            DataSet data1 = new DataSet();
            DataSet data2 = new DataSet();

            //wyciągnięcie wszystkich tabelek i wsadzenie do secików (to powinna być metoda)
            //jesli chcemy to robic dla roznych baz (w sensie, ze nie muszą mieć takich
            //samych tabel) to trzeba rzucać i łapać wyjątki, na razie mi się nie chce
            for (int i = 0; i < namesOfTables.Count; i++)
            {
                String command = "Select * from " + namesOfTables[i];
                SqlDataAdapter adp1 = new SqlDataAdapter(command, con1);
                SqlDataAdapter adp2 = new SqlDataAdapter(command, con2);
                DataTable dt1 = new DataTable();
                DataTable dt2 = new DataTable();
                adp1.Fill(dt1);
                adp2.Fill(dt2);
                dt1.TableName = namesOfTables[i];
                dt2.TableName = namesOfTables[i];
                data1.Tables.Add(dt1);
                data2.Tables.Add(dt2);
            }

            data = new DataSet();
            //pierwsze przejście, 
            foreach(DataTable table in data1.Tables)
            {
                data.Tables.Add(table.Copy());
            }
            
            //drugi to już właściwa integracja, na razie jest prostacka
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

            this.ShowData();
            
            con1.Close();
            con2.Close();
        }
    }
}

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

            for(int i = 0; i < namesOfTables.Count; i++)
            {
                String command = "Select * from " + namesOfTables[i];
                SqlDataAdapter adp1 = new SqlDataAdapter(command, con1);
                SqlDataAdapter adp2 = new SqlDataAdapter(command, con2);
                DataTable dt1 = new DataTable();
                DataTable dt2 = new DataTable();
                adp1.Fill(dt1);
                adp2.Fill(dt2);
                data1.Tables.Add(dt1);
                data2.Tables.Add(dt2);
            }

            
            foreach(DataTable table in data1.Tables)
            {
                DataRowCollection wiersze = table.Rows;
                DataColumnCollection columny = table.Columns;
                foreach (DataRow row in wiersze)
                {
                    foreach (DataColumn col in columny)
                    {
                        Console.Write(row[col] + ", ");
                    }
                    Console.WriteLine(";");
                }
            }

            Console.WriteLine();
            data = new DataSet();

            data.Merge(data1);
            data.Merge(data2); //nie dodaje bo te same kolumny primary key


            //SqlCommand 
            //SqlDataAdapter  
            //dateset'y i jakoś to integrować

            con1.Close();
            con2.Close();
        }
    }
}

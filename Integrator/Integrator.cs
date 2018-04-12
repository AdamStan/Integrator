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

            data = new DataSet();

            foreach (String name in namesOfTables)
            {
                String command = "Select * from " + name;
                SqlDataAdapter adp1 = new SqlDataAdapter(command, con1);
                SqlDataAdapter adp2 = new SqlDataAdapter(command, con2);
                adp1.Fill(data);
                adp2.Fill(data);
            }

            //DataView = new DataGridView();
            //DataView.DataSource = set;
            //DataView.DataMember = "Customers";

            Console.WriteLine();
            //SqlCommand 
            //SqlDataAdapter  
            //dateset'y i jakoś to integrować

            con1.Close();
            con2.Close();
        }
    }
}

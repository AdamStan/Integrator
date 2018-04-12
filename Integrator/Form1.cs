using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Integrator
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void Integruj(object sender, EventArgs e)
        {
            
            DataTable dtAll = Program.integrator.data.Tables[0].Copy();
            for (var i = 1; i < Program.integrator.data.Tables.Count; i++)
            {
                dtAll.Merge(Program.integrator.data.Tables[i]);
            }
            dataGridView1.AutoGenerateColumns = true;
            dataGridView1.DataSource = dtAll;
        }
    }
}

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
            Program.integrator.Integrate();
            //generowanie raporciku
            // + pokazanie jakies w apce
        }

        private void GetConnection(object sender, EventArgs e)
        {
            button1.Enabled = false;
            button2.Enabled = true;
        }

        private void GetDisconnect(object sender, EventArgs e)
        {
            button1.Enabled = true;
            button2.Enabled = false;
        }

        private void Quit(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}

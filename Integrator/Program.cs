using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Integrator
{
    static class Program
    {
        /// <summary>
        /// Główny punkt wejścia dla aplikacji.
        /// </summary>
        [STAThread]
        static void Main()
        {
            
            Integrator i =
                new Integrator("Data Source=(LocalDB)\\MSSQLLocalDB;AttachDbFilename=D:\\" +
                    "Projekty\\C#\\Integrator\\Integrator\\Database1.mdf;Integrated Security=True", 
                    "Data Source=(LocalDB)\\MSSQLLocalDB;AttachDbFilename=D:\\Projekty\\C#\\" +
                    "Integrator\\Integrator\\Database2.mdf;Integrated Security=True");
            i.Integrate();
            
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new Form1());
        }
        static void getConnect()
        {

        }
    }
}

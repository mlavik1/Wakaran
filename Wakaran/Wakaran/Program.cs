using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Windows.Forms;

namespace Wakaran
{

    class Program
    {

        [STAThread]
        static void Main(string[] args)
        {
            MainForm mainForm = new MainForm();

            Application.EnableVisualStyles();
            Application.Run(mainForm);

            while (true)
            {
                
            }
        }
    }
}

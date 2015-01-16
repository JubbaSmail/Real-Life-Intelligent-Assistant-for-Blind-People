using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace Main_Server
{
    static class Program
    {
        public static MainFrm main_frm;
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            main_frm = new MainFrm();
            Application.Run(main_frm);
        }
    }
}
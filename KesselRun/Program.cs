using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

namespace KesselRun
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            GameBoard board = new GameBoard();

            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new GameBoardForm(board));
        }
    }
}

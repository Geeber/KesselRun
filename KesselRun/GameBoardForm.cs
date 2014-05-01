using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace KesselRun
{
    public partial class GameBoardForm : Form
    {

        public GameBoardForm(GameBoard board)
        {
            InitializeComponent();
            gamePanel.Initialize(board);
        }

        private void restartToolStripMenuItem_Click(object sender, EventArgs e)
        {
            gamePanel.Reset();
        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void aboutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Kessel Run\n" +
                            "A puzzle for the Microsoft Intern PuzzleDay 2008\n" +
                            "by Kevin Litwack");
        }

        private void rulesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            new RulesForm().Show();
        }

    }
}

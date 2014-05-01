using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Windows.Forms;

namespace KesselRun
{
    public partial class RulesForm : Form
    {
        public RulesForm()
        {
            InitializeComponent();

            try
            {
                Stream ruleFileStream = Assembly.GetAssembly(this.GetType()).GetManifestResourceStream("KesselRun.rules.rtf");
                RulesTextBox.LoadFile(ruleFileStream, RichTextBoxStreamType.RichText);
                ruleFileStream.Close();
            }
            catch (System.IO.IOException)
            {
                RulesTextBox.Text = "Rules file (rules.rtf) not found";
            }
        }
    }
}

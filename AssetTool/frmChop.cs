using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace AssetTool
{
    public partial class frmChop : Form
    {
        public frmChop()
        {
            InitializeComponent();
        }

        public int ChopAmount { get; set; }

        private void button1_Click(object sender, EventArgs e)
        {
            if (int.TryParse(txtChop.Text, out int v))
            {
                ChopAmount = v;
                this.Close();
            }
            else
            {
                MessageBox.Show("Enter a number");
            }

        }
    }
}

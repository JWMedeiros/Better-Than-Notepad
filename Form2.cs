using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Project_1
{
    public partial class Form2 : Form
    {
        //bool textChange = false;
        public Form2()
        {
            int num = 0;
            InitializeComponent();
            foreach (char c in txtBox1.Text)
            {
                num++;
            }
            toolStripStatusLabel1.Text = "Current characters in the file: "+num.ToString();
        }

        //Unused
        private void Form2_Load(object sender, EventArgs e)
        {

        }

        //Initializes form1s closing function (closes active mdi child)
        private void Form2_FormClosing(object sender, FormClosingEventArgs e)
        {
            ((Form1)this.MdiParent).closeToolStripMenuItem_Click(sender,e);
        }

        //Used to determine characters in the current form
        private void txtBox1_TextChanged(object sender, EventArgs e)
        {
            int num = 0;
            foreach (char c in txtBox1.Text)
            {
                num++;
            }
            toolStripStatusLabel1.Text = "Current characters in the file: " + num.ToString();
        }

        //Unused
        private void txtBox1_Leave(object sender, EventArgs e)
        {
        }
        
        //Unused
        private void statusStrip1_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {

        }
    }

        

}

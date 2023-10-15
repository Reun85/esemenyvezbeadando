using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace RobotPigs.WFA
{
    public partial class NumberInp : Form
    {
        private Game _g;
        public NumberInp(Game g)
        {
            _g = g;
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            
            String inp = textBox1.Text;
            try
            {
                int parsed = Convert.ToInt32(inp);

                _g.IsValidSize(parsed) ;
                _g.N = parsed;
                DialogResult = DialogResult.OK;
                
            }
            
            catch (ArgumentException ex)
            {
                MessageBox.Show(ex.Message, "Harcos robotmalacok csatája", MessageBoxButtons.OK);
            }
            catch (Exception)
            {
                MessageBox.Show("Nem érvényes szám.", "Harcos robotmalacok csatája", MessageBoxButtons.OK);
            }


        }
    }
}

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WindowsFormsApplication2
{
    public partial class Form2 : Form
    {
        baseDatos db = new baseDatos("Database=valeodv;Data Source=localhost;user Id=root;Password=ima1;");
        public Form2()
        {
            InitializeComponent();
        }

        private void Form2_Load(object sender, EventArgs e)
        {

        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            db.filtroetiqueta(dataGridView1, "piezasok", textBox1.Text);
            db.filtroetiqueta(dataGridView2, "piezasnook", textBox1.Text);
        }
    }
}

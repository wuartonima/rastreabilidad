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
    public partial class Form3 : Form
    {
        baseDatos db = new baseDatos("Database=valeodv;Data Source=localhost;user Id=root;Password=ima1;");
        public Form3()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if(db.verificarPass(textBox1.Text))
            { Form1.f1.borrador(); Close(); }
            else { MessageBox.Show("clave incorrecta"); }

        }

        private void cambio_Click(object sender, EventArgs e)
        {
            if (!db.verificarPass(claveanterior.Text)) { MessageBox.Show("clave erronea"); }
            else
            {
                db.cambiarPass(nueva.Text, claveanterior.Text);
                Close();
            }                    
        }
    }
}

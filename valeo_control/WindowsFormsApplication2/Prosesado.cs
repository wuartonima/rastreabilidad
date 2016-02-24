using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WindowsFormsApplication2
{
    class Prosesado
    {
        baseDatos basedatos = new baseDatos("Database=valeodv;Data Source=localhost;user Id=root;Password=ima1;");
        DateTime fecha = DateTime.Today;

        public List<MetroFramework.Controls.MetroTextBox> linea80 = new List<MetroFramework.Controls.MetroTextBox>();
        public List<MetroFramework.Controls.MetroTextBox> linea100 = new List<MetroFramework.Controls.MetroTextBox>();
        public List<MetroFramework.Controls.MetroTextBox> linea110 = new List<MetroFramework.Controls.MetroTextBox>();
        public List<Button> linea80btn = new List<Button>();
        public List<Button> linea100btn = new List<Button>();
        public List<Button> linea110btn = new List<Button>();
        public int oklinea80 = 2, noklinea80 = 4, oklinea100 = 6, noklinea100 = 7, oklinea110 = 8;
        int contador = 0;
        bool iterativo = false;
        public bool resultado { get; set; }
        public bool prueva { get; set; }
        public Prosesado()
        {
            resultado = false;
            prueva = false;
        }
        public void identificar(string cadenatcp)
        {
            string[] separada = cadenatcp.Split(':');
            if (separada[0].Contains("plc1")) { plc_1(separada[1]); }
            if (separada[0].Contains("plc2")) { plc_2(separada[1]); }


        }
        public void plc_1(string comando)
        {
            string[] spliteada = comando.Split(',');
            if (spliteada[0].Contains("error")) { MessageBox.Show("etiqueta no leida"); }
            else {
                if (spliteada[1].Contains("nok")) { pl1nok(spliteada[0], spliteada[2]); noklinea80++; }
                else {
                    oklinea80++;
                    Form1.f1.corrimiento(80, spliteada[0], true);
                    basedatos.insertarFila("sccm", spliteada[0] + "," + spliteada[2] + ","
                                            + fecha.ToString("D").Replace("/", "-"));

                }
            }

        }
        public void pl1nok(string etiqueta, string fuga)
        {
            Form1.f1.corrimiento(80, etiqueta, false);
            basedatos.insertarFila("piezasnook", fecha.ToString("D").Replace("/", "-") + ","
                                   + DateTime.Now.ToString("T") + "," + etiqueta + "," + fuga
                                   + "," + "nok," + "nok," + "nok");
        }
        public void plc_2(string comando)
        {
            string[] spliteada = comando.Split(',');
            if (spliteada[0].Contains("error")) { MessageBox.Show("etiqueta no leida linea 100"); } else {
                if (spliteada[1].Contains("consulta")) { consulta(spliteada[0]); }
                if (spliteada[1].Contains("registro")) { registro(spliteada[0], spliteada[2]); } }
        }
        public void consulta(string etiqueta)
        {
            bool[] est80 = Form1.f1.consultar(etiqueta, Form1.f1.linea80, Form1.f1.linea80btn);
            bool[] est100 = Form1.f1.consultar(etiqueta, Form1.f1.linea100, Form1.f1.linea100btn);

            if (est80[1]) { prueva = true; resultado = true; }
            else { prueva = true; resultado = false; }

        }
        public void registro(string etiqueta, string resultado)
        {

            bool result = resultado.Contains("ok");

            bool repitioPrueba = false;

            bool[] est100 = Form1.f1.consultar(etiqueta, Form1.f1.linea100, Form1.f1.linea100btn);
            if (est100[0] && est100[2]) { repitioPrueba = true; }

            Form1.f1.corrimiento(100, etiqueta, result);

            string sccm = basedatos.consultarDato("sccm", "sccms", etiqueta,"etiqueta");


            if (!repitioPrueba)//no repitio prueba
            {
                if (result)
                {
                    oklinea100++;
                    basedatos.insertarFila("piezasok", fecha.ToString("D").Replace("/", "-") + ","
                                           + DateTime.Now.ToString("T") + "," + etiqueta + "," + sccm
                                           + "," + "ok," + "ok," + "");
                }
                else
                {
                    noklinea100++;
                    basedatos.insertarFila("piezasnook", fecha.ToString("D").Replace("/", "-") + ","
                                            + DateTime.Now.ToString("T") + "," + etiqueta + "," + sccm
                                            + "," + "ok," + "nok," + "nok");
                }
            }
            else
            {
                if (result)//repitio prueba y ahora es ok
                {

                    basedatos.borrar("piezasnook", "codigobarras", etiqueta, fecha.ToString("D").Replace("/", "-") + ","
                                            );
                    oklinea100++;
                    basedatos.insertarFila("piezasok", fecha.ToString("D").Replace("/", "-") + ","
                                           + DateTime.Now.ToString("T") + "," + etiqueta + "," + sccm
                                           + "," + "ok," + "ok," + "");
                }
            }




        }
        public void show()
        { contador++;
            if (iterativo) { iterativo = false; } else { iterativo = true; }
            Form1.f1.corrimiento(80, contador.ToString(), iterativo);
        }
        



        }



    }


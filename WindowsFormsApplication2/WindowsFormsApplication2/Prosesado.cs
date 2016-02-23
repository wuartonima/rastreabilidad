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
        Form1 main = new Form1();
        public List<MetroFramework.Controls.MetroTextBox> linea80 = new List<MetroFramework.Controls.MetroTextBox>();
        public List<MetroFramework.Controls.MetroTextBox> linea100 = new List<MetroFramework.Controls.MetroTextBox>();
        public List<MetroFramework.Controls.MetroTextBox> linea110 = new List<MetroFramework.Controls.MetroTextBox>();
        public List<Button> linea80btn = new List<Button>();
        public List<Button> linea100btn = new List<Button>();
        public List<Button> linea110btn = new List<Button>();
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
                if (spliteada[1].Contains("nok")) { pl1nok(spliteada[0], spliteada[2]); }
                else {
                    
                    main.corrimiento(80, spliteada[0], true);
                    basedatos.insertarFila("sccm", spliteada[0] + "," + spliteada[2] + "," 
                                            + fecha.ToString("D").Replace("/", "-"));
                   
                }
            }

        }
        public void pl1nok(string etiqueta, string fuga)
        {
           main.corrimiento(80, etiqueta, false);
            basedatos.insertarFila("piezasnook", fecha.ToString("D").Replace("/", "-") + "," 
                                   + DateTime.Now.ToString("T") + "," + etiqueta + "," + fuga 
                                   + "," + "nok," + "nok," + "nok");
        }
        public void plc_2(string comando)
        {
            string[] spliteada = comando.Split(',');
            if (spliteada[0].Contains("error")) { MessageBox.Show("etiqueta no leida linea 100"); } else { 
            if (spliteada[1].Contains("consulta")) { consulta(spliteada[0]);}
            if (spliteada[1].Contains("registro")) { registro(spliteada[0],spliteada[2]); }}
        }
        public void consulta(string etiqueta)
        {
            bool[] result = main.consultar(etiqueta,main.linea80,main.linea80btn) ;
            Form1.
           bool[] result2 = main.consultar(etiqueta, main.linea100, main.linea100btn);
            if (result[1])  {
               prueva =true;resultado = true;
           }
            else
            {
                prueva =true; resultado = false;

            }

        }
        public void registro(string etiqueta,string resultado)
        {
            
            bool result = resultado.Contains("ok");
            main.corrimiento(100, etiqueta, result);

            string sccm = basedatos.consultarDato("sccm", "sccms", etiqueta);
            if (result){
                basedatos.insertarFila("piezasok", fecha.ToString("D").Replace("/", "-") + ","
                                       + DateTime.Now.ToString("T") + "," + etiqueta + "," + sccm
                                       + "," + "ok," + "ok," + "");          
            }
            else{
                basedatos.insertarFila("piezasnook", fecha.ToString("D").Replace("/", "-") + ","
                                        + DateTime.Now.ToString("T") + "," + etiqueta + "," + sccm
                                        + "," + "ok," + "nok," + "nok");
            }


        }
        public void show()
        {
            MessageBox.Show("todo ok");
        }
     
    }
}

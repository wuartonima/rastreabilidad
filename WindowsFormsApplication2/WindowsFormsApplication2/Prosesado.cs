using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WindowsFormsApplication2
{
    class Prosesado
    {
        baseDatos basedatos = new baseDatos("Database=valeodb;Data Source=localhost;user Id=root;Password=ima1;");
        DateTime fecha = DateTime.Today;
        public bool resultado { get; set; }
        public bool prueva { get; set; }
        string polo;
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
            if (spliteada[0].Contains("error")) { MessageBox.Show("etiqueta no leida"); } else{
                if (spliteada[1].Contains("nok")) { pl1nok(spliteada[0], spliteada[2]); } else{
                    basedatos.insertarFila("sccm", spliteada[0] + "," + spliteada[2] + "," 
                                            + fecha.ToString("D").Replace("/", "-"));
                }
            }

        }
        public void pl1nok(string etiqueta, string fuga)
        {
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
            bool result = basedatos.existeDato("piezasnook", "codigobarras", etiqueta);
            if (result)  {
                prueva = true;resultado = false;
            }
            else
            {
                prueva = true; resultado = true;

            }

        }
        public void registro(string etiqueta,string resultado)
        {
            bool result = resultado.Contains("ok");
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

    }
}

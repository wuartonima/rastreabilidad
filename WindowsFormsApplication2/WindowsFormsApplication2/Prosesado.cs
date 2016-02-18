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

        public Prosesado()
        {
           
        }
        public void identificar(string cadenatcp)
        {
            string[] separada=cadenatcp.Split(':');
            if (separada[0].Contains("plc1")) { plc_1(separada[1]); }
            if (separada[0].Contains("plc2")) { plc_2(separada[1]); }


        }
        public void plc_1(string comando)
        {
            string[] spliteada = comando.Split(',');
            if (spliteada[0].Contains("error")) { MessageBox.Show("etiqueta no leida");  }else
            {
                return comando+",
            }

        }
        public void plc_2(string comando)
        {

        }

    }
}

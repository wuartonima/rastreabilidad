using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;
using System.Data;

namespace WindowsFormsApplication2
{
    class baseDatos
    {
        public string conexion { get; set; }
        MySqlConnection con;
        MySqlCommand coman = new MySqlCommand();


        public baseDatos(string cadenaconexion) //constructor de la clase se ejecuta al crear un objecto referenciado
        {
            conexion = cadenaconexion;
            con = new MySqlConnection(conexion);
            coman.Connection = con;
           
        }

        public bool existeDato(string tabladb,string tipodato, string valor)// consultar si existe un dato
        {
            con.Open();
            bool existe = false;
            coman.CommandText ="SELECT "+tipodato+" FROM "+ tabladb +" WHERE "+tipodato+"="+"'"+valor+"'";
            MySqlDataReader read;
            read = coman.ExecuteReader();
            if (read.Read())
            {
                existe = true;
            }
            con.Close();
            return existe;
        }
        public void insertarFila(string tabla,string valores)//insertar fila 
        {          
            con.Open();
           
            coman.CommandText= "insert into " + tabla + " values( "+valores+")";
            coman.ExecuteReader();
            con.Close();
        }
        


    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;
using System.Data;
using System.Windows.Forms;

namespace WindowsFormsApplication2
{
    class baseDatos
    {
        public string conexion {get; set;}
        public int contador = 0;
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
        public string consultarDato(string tabladb, string tipodato, string valor)
        {
            con.Open();
            string dato="";

            coman.CommandText = "SELECT " + tipodato + " FROM " + tabladb + " WHERE " + tipodato + "=" + "'" + valor + "'";
            MySqlDataReader read;
            read = coman.ExecuteReader();
            if (read.Read())
            {
                dato = read.GetString(tipodato);
            }
            con.Close();
            return dato;
           
           
        }

        public void filtrofecha(DataGridView tabla,string tabladb,string f1,string f2, string h1, string h2)
        {
            string consulta="";
            con.Open();
            try
            {
                consulta = "SELECT * FROM " + tabladb + " WHERE fecha between " +"'"+f1+ "'" + " and " + "'" + f2 + "'" + "AND hora between " + "'" + h1 + "'" + " and " + "'" + h2 + "'" + ";";
               
                MySqlDataAdapter data = new MySqlDataAdapter(consulta, con);
                DataSet ds = new DataSet();
                data.Fill(ds, tabladb);
                tabla.DataSource = ds;
                tabla.DataMember = tabladb;
            }
            catch {  }
            con.Close();
        }
        
         public bool verificarPass(string password){
            
            return consultarDato("usuario","contraseña","accesoValeo") == password;
            
        }
        
        public void cambioPassword(string passActual,string passNuevo){
            
            if (consultarDato("usuario","contraseña","accesoValeo") == passActual){
                //Actualizar Registo
            }
            
        }
        
        public void cambiarPass(string nuevoPass, string actualPass){
            
            con.Open();
            
            coman.CommandText =  "update usuario set contraseña = " + nuevoPass + "where contraseña = " + actualPass + "limit 1";
            MySqlDataReader read;
            read = coman.ExecuteReader();
            
            con.Close();
            
        }

    }
}


    }
}

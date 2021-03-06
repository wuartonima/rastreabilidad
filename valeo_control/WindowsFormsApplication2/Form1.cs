﻿using MetroFramework.Forms;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Threading;
using MetroFramework;
using Microsoft.Office;

namespace WindowsFormsApplication2
{
    public partial class Form1 : MetroForm
    {
        #region variables
        baseDatos basedatos = new baseDatos("Database=valeodv;Data Source=localhost;user Id=root;Password=ima1;");
        private static Socket _serverSocket;
        private static readonly List<Socket> _clientSockets = new List<Socket>();
        private const int _BUFFER_SIZE = 2048;
        private const int _PORT = 2000;
        private static readonly byte[] _buffer = new byte[_BUFFER_SIZE];
        public static string temp { get; set; }
        int contador = 0;
        public string anterior = "";
        public List<MetroFramework.Controls.MetroTextBox> linea80 = new List<MetroFramework.Controls.MetroTextBox>();
        public List<MetroFramework.Controls.MetroTextBox> linea100 = new List<MetroFramework.Controls.MetroTextBox>();
        public List<MetroFramework.Controls.MetroTextBox> linea110 = new List<MetroFramework.Controls.MetroTextBox>();
        public List<Button> linea80btn = new List<Button>();
        public List<Button> linea100btn = new List<Button>();
        public List<Button> linea110btn = new List<Button>();
        public static Form1 f1;
        bool it = false;

        Prosesado logica;
        #endregion
        public Form1()
        {           
            InitializeComponent();
            SetupServer();
            Form1.f1 = this;
       
        }
     
        #region comunicacion
        private void metroButton5_Click(object sender, EventArgs e)
        {
            Form2 buscar = new Form2();
            buscar.Show();
        }
        private  void SetupServer()
        {
            IPAddress localAddr = IPAddress.Parse("192.168.0.141");      
            _serverSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            _serverSocket.Bind(new IPEndPoint(IPAddress.Any, _PORT));
            _serverSocket.Listen(5);
            _serverSocket.BeginAccept(AcceptCallback, null);
        }
        private  void CloseAllSockets()
        {
            foreach (Socket socket in _clientSockets)
            {
                socket.Shutdown(SocketShutdown.Both);
                socket.Close();
            }

            _serverSocket.Close();
        }

        private  void AcceptCallback(IAsyncResult AR)
        {
            Socket socket;

            try
            {
                socket = _serverSocket.EndAccept(AR);
            }
            catch (ObjectDisposedException) // I cannot seem to avoid this (on exit when properly closing sockets)
            {
                return;
            }

            _clientSockets.Add(socket);
            socket.BeginReceive(_buffer, 0, _BUFFER_SIZE, SocketFlags.None, ReceiveCallback, socket);
           
            _serverSocket.BeginAccept(AcceptCallback, null);
        }

        public void ReceiveCallback(IAsyncResult AR)
        {
            Socket current = (Socket)AR.AsyncState;
            int received;

            try
            {
                received = current.EndReceive(AR);
            }
            catch (SocketException)
            {
               
                current.Close(); 
                _clientSockets.Remove(current);
                return;
            }

            byte[] recBuf = new byte[received];
            Array.Copy(_buffer, recBuf, received);
            string text = Encoding.ASCII.GetString(recBuf);///importante
            contador++;
            temp = text.Substring(0,4)+ contador;
            
            logica.identificar(text);
            if(logica.prueva && logica.resultado)
            {
                byte[] data = Encoding.ASCII.GetBytes("a");
                current.Send(data);
            }
            if (logica.prueva && logica.resultado==false)
            {
                byte[] data = Encoding.ASCII.GetBytes("na");
                current.Send(data);
            }
            logica.prueva = false;logica.resultado = false;
            
            current.BeginReceive(_buffer, 0, _BUFFER_SIZE, SocketFlags.None, ReceiveCallback, current);
        }


        #endregion

     

        private void metroButton1_Click_1(object sender, EventArgs e)
        {
            basedatos.filtrofecha(metroGrid1, "piezasnook", metroDateTime1.Value.ToString("yyyy-MM-dd"),
                                  metroDateTime2.Value.ToString("yyyy-MM-dd"),
                                  dateTimePicker1.Value.ToString("HH:mm:ss"),
                                  dateTimePicker2.Value.ToString("HH:mm:ss"));
            basedatos.filtrofecha(metroGrid2, "piezasok", metroDateTime1.Value.ToString("yyyy-MM-dd"),
                                  metroDateTime2.Value.ToString("yyyy-MM-dd"),
                                  dateTimePicker1.Value.ToString("HH:mm:ss"),
                                  dateTimePicker2.Value.ToString("HH:mm:ss"));
        }
        public void corrimiento(int linea, string Newetiqueta, bool result )
        {
            var color=Color.Lime;
            if (result == false){ color = Color.Red;}
            switch (linea)
            {
                case 80:
                    p3l80.Text = p2l80.Text; p2l80.Text = p1l80.Text; p1l80.Text = Newetiqueta;
                    lb803.BackColor = lb802.BackColor; lb802.BackColor = lb801.BackColor; lb801.BackColor = color;
                    break;
                case 100:
                    p3l100.Text = p2l100.Text; p2l100.Text = p1l100.Text; p1l100.Text = Newetiqueta;
                    lb1003.BackColor = lb1002.BackColor; lb1002.BackColor = lb1001.BackColor; lb1001.BackColor = color;
                    break;
                case 110:
                    p3l110.Text = p2l110.Text;p2l110.Text = p1l110.Text;p1l110.Text = Newetiqueta;
                    lb1103.BackColor = lb1102.BackColor; lb1102.BackColor = lb1101.BackColor; lb1101.BackColor = color;
                    break;
                default:
                    break;
            }

        }
        public bool[] consultar(string etiqueta, List<MetroFramework.Controls.MetroTextBox> lineatx,List<Button> lineabtn)
        {
            bool[] result = { false, false,false };///result[0]= si existe etiqueta, result[1]=es ok?
            try {
                result[0] = lineatx.Exists(element => element.Text.Contains(etiqueta));
                if (result[0])
                {
                    result[1] = lineabtn[lineatx.FindIndex(element => element.Text.Contains(etiqueta))].BackColor == Color.Lime;
                    result[2] = lineatx.FindIndex(element => element.Text.Contains(etiqueta)) == 0;
                }
            }
            catch { }
            return result;             
        }
        private void Form1_Load(object sender, EventArgs e)
        {
            linea80.Add(p1l80);linea80.Add(p2l80);linea80.Add(p3l80);
            linea100.Add(p1l100); linea100.Add(p2l100); linea100.Add(p3l100);
            linea110.Add(p1l110); linea100.Add(p2l110); linea100.Add(p3l110);
            linea80btn.Add(lb801); linea80btn.Add(lb802); linea80btn.Add(lb803);
            linea100btn.Add(lb1001); linea80btn.Add(lb1002); linea80btn.Add(lb1003);
            linea110btn.Add(lb1101); linea110btn.Add(lb1102); linea110btn.Add(lb1103);
            logica = new Prosesado();
        }
        private void timer1_Tick(object sender, EventArgs e)
        {
            oklinea80.Text = logica.oklinea80.ToString();
            noklinea80.Text = logica.noklinea80.ToString();
            oklinea100.Text = logica.oklinea100.ToString();
            noklinea100.Text = logica.noklinea100.ToString();
            oklinea110.Text = logica.oklinea110.ToString(); 

         
        }
        public void borrador()
        {
           logica.oklinea80=0;
           logica.noklinea80=0;
           logica.oklinea100=0;
           logica.noklinea100=0;
           logica.oklinea110=0;


        }
        public void excel(DataGridView grd , DataGridView grd2)
        {
            SaveFileDialog fichero = new SaveFileDialog();
            fichero.Filter = "Excel (*.xls)|*.xls";
            if (fichero.ShowDialog() == DialogResult.OK)
            {
                Microsoft.Office.Interop.Excel.Application aplicacion;
                Microsoft.Office.Interop.Excel.Workbook libros_trabajo;
                Microsoft.Office.Interop.Excel.Worksheet hoja_trabajo;
                aplicacion = new Microsoft.Office.Interop.Excel.Application();
                libros_trabajo = aplicacion.Workbooks.Add();        
                hoja_trabajo =
                 (Microsoft.Office.Interop.Excel.Worksheet)libros_trabajo.Worksheets.get_Item(1);

                hoja_trabajo.Cells[1, 1] = "piezasok";               
                for (int x = 0; x <= grd.ColumnCount - 1; x++)
                {
                    hoja_trabajo.Cells[2, x + 1]=  grd.Columns[x].HeaderText ;
                }

                for (int i = 0; i < grd.Rows.Count - 1; i++)
                {
                    for (int j = 0; j < grd.Columns.Count; j++)
                    {
                        hoja_trabajo.Cells[i + 3, j + 1] = grd.Rows[i].Cells[j].Value.ToString();
                    }
                }
                hoja_trabajo.Cells[grd.Rows.Count+3, 1] = "piezasnok";
                for (int x = 0; x <= grd2.ColumnCount - 1; x++)
                {
                    hoja_trabajo.Cells[grd.Rows.Count + 4, x + 1] = grd2.Columns[x].HeaderText;
                }

                for (int i = 0; i < grd2.Rows.Count - 1; i++)
                {
                    for (int j = 0; j < grd2.Columns.Count; j++)
                    {
                        hoja_trabajo.Cells[i + grd.Rows.Count + 5, j + 1] = grd2.Rows[i].Cells[j].Value.ToString();
                    }
                }
                libros_trabajo.SaveAs(fichero.FileName,
                    Microsoft.Office.Interop.Excel.XlFileFormat.xlWorkbookNormal);
                libros_trabajo.Close(true);
                aplicacion.Quit();
            }
        }
        private void metroButton2_Click(object sender, EventArgs e)
        {
            excel(metroGrid1,metroGrid2);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Form3 f3 = new Form3();
            f3.Show();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            contador++; 
            if (it) { it = false; } else{ it = true; }
            corrimiento(80, contador.ToString(), it);
        }
    }
}

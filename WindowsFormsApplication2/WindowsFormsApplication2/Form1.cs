using MetroFramework.Forms;
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

namespace WindowsFormsApplication2
{
    public partial class Form1 : MetroForm
    {
        #region variables
        baseDatos basedatos = new baseDatos("Database=valeodb;Data Source=localhost;user Id=root;Password=ima1;");
        private static Socket _serverSocket;
        private static readonly List<Socket> _clientSockets = new List<Socket>();
        private const int _BUFFER_SIZE = 2048;
        private const int _PORT = 2000;
        private static readonly byte[] _buffer = new byte[_BUFFER_SIZE];
        public static string temp { get; set; }
        int contador = 0;
        Prosesado logica = new Prosesado();
        #endregion
        public Form1()
        {
            InitializeComponent();
            SetupServer();
            
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
               
                current.Close(); // Dont shutdown because the socket may be disposed and its disconnected anyway
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

        private void timer1_Tick(object sender, EventArgs e)
        {
            prueva.Text = temp;
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void label9_Click(object sender, EventArgs e)
        {

        }
    }
}

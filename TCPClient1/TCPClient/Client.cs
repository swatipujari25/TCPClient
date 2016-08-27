using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Net.Sockets;
using System.Threading;

namespace TCPClient
{
    public partial class Client : Form
    {       

        public Client()
        {
            InitializeComponent();
        }
        public delegate void AppendDataDelegate();
        AppendDataDelegate dataDelegate = null;
        string recvMsg = string.Empty;

        System.Net.Sockets.TcpClient clientSocket = new System.Net.Sockets.TcpClient();
        NetworkStream serverStream = default(NetworkStream);
        string readData = null;


        private void BindDataToTextBox()
        {
            rtxDisplay.AppendText(readData);
        }

        private void InvokeTextBox()
        {
            rtxDisplay.Invoke(dataDelegate);
        }

        private void btnConnect_Click(object sender, EventArgs e)
        {
            //byte[] outStream = System.Text.Encoding.ASCII.GetBytes(txtUser.Text+ "$");
            //serverStream.Write(outStream, 0, outStream.Length);
            //serverStream.Flush();

            readData = "Conected to Chat Server ...";
            msg();

            if (!clientSocket.Connected)
            {
                clientSocket.Connect("127.0.0.1", 8000);
            }
            serverStream = clientSocket.GetStream();

            byte[] outStream = System.Text.Encoding.ASCII.GetBytes(txtUser.Text );
            serverStream.Write(outStream, 0, outStream.Length);
            serverStream.Flush();

            Thread ctThread = new Thread(getMessage);
            ctThread.Start();
        }

        private void btnSend_Click(object sender, EventArgs e)
        {           

            if (!clientSocket.Connected)
            {
                clientSocket.Connect("127.0.0.1", 8000);
            }
            serverStream = clientSocket.GetStream();

            byte[] outStream = System.Text.Encoding.ASCII.GetBytes(txtData.Text + "\r\n");
            serverStream.Write(outStream, 0, outStream.Length);
            serverStream.Flush();

            
        }

        private void getMessage()
        {
            while (true)
            {
                serverStream = clientSocket.GetStream();
                int buffSize = 0;
                byte[] inStream = new byte[10025];
                buffSize = clientSocket.ReceiveBufferSize;
                serverStream.Read(inStream, 0, buffSize);
                string returndata = System.Text.Encoding.ASCII.GetString(inStream);
                readData = "" + returndata;
                msg();
            }
        }

        private void msg()
        {
            //if (this.InvokeRequired)
            //    this.Invoke(new MethodInvoker(msg));
            //else
            //    //rtxDisplay.Text = textBox1.Text + Environment.NewLine + " >> " + readData;
            //    // rtxDisplay.AppendText( Environment.NewLine + " >> " + readData);
            //    recvMsg = Environment.NewLine + " >> " + readData;
            InvokeTextBox();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            dataDelegate = new AppendDataDelegate(BindDataToTextBox);
            
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            //timer1.Start();
            //if (!string.IsNullOrEmpty(recvMsg))
            //{
            //    dataDelegate();
            //    recvMsg = string.Empty;
            //}
        } 
    }
}

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using System.Collections;

namespace NetworkProgram02_server
{
    public partial class Form1 : Form
    {
        TcpListener server;
        Socket[] client;
        Thread Th_Svr;
        Thread Th_Clt;
        Hashtable HT = new Hashtable();
        int i;
        const int port = 1234;
        const string addr = "127.0.0.1";
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }
        private void Send(string Str,int idx)
        {
            byte[] B = Encoding.Default.GetBytes(Str);
            try
            {
                client[idx].Send(B, 0, B.Length, SocketFlags.None);
            }
            catch (Exception)
            {
                MessageBox.Show("遠端主機已關閉");
                for(int i=0;i<2;i++)
                    client[i].Close();
                Application.Exit();
            }
        }
        private void Listen()
        {
            Socket Sck = client[i];
            int id = i;
            Thread th = Th_Clt;
            while (true)
            {
                try
                {
                    byte[] B = new byte[1023];
                    int inLen = Sck.Receive(B);
                    string Msg = Encoding.Default.GetString(B, 0, inLen);
                    /* 0 是下棋 1是有人勝利
                     * 訊息格式 0 x y #x,y是矩陣座標
                               1 user #user 是 client 1或client 2
                     */
                    string Cmd = Msg.Substring(0, 1);
                    for (int i = 0; i < 2; i++)
                    {
                        if (i != id)
                            Send(Msg,i);
                    }
                    if (Cmd == "1")
                    {
                        Application.Exit();
                    }
                }
                catch (Exception)
                {

                }
            }
        }
        private void Serversub()
        {
            IPEndPoint EP = new IPEndPoint(IPAddress.Parse(addr), port);
            server = new TcpListener(EP);
            server.Start(2);
            for (i = 0; i < 2; i++)
            {
                client[i] = server.AcceptSocket();
                Th_Clt = new Thread(Listen);
                Th_Clt.IsBackground = true;
                Th_Clt.Start();
            }
        }
        private void button1_Click(object sender, EventArgs e)
        {
            CheckForIllegalCrossThreadCalls = false;
            Th_Svr = new Thread(Serversub);
            Th_Svr.IsBackground = true;
            Th_Svr.Start();
            button1.Enabled = false;
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            Application.ExitThread();
        }
    }
}

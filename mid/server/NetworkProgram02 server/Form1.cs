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
        Socket[] client=new Socket[2];
        Thread Th_Svr;
        Thread Th_Clt;
        Hashtable HT = new Hashtable();
        int i;
        const int port = 1234;
        const string ip = "127.0.0.1";
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
            int id = i-1;
            Thread th = Th_Clt;
            //ListBox1.Items.Add(id);
            while (true)
            {
                try
                {
                    byte[] B = new byte[1023];
                    int inLen = client[id].Receive(B);
                    string Msg = Encoding.Default.GetString(B, 0, inLen);
                    /*
                     * 訊息格式 x y #x,y是矩陣座標
                               x=255,y=255 代表遊戲結束
                     */
                    for (int j = 0; j < 2; j++)
                    {
                        if (j != id)
                            Send(Msg,j);
                    }
                }
                catch (Exception)
                {

                }
            }
        }
        private void Serversub()
        {
            IPEndPoint EP = new IPEndPoint(IPAddress.Parse(ip), port);
            server = new TcpListener(EP);
            server.Start(100);
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

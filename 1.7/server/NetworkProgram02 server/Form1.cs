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
using System.Runtime.InteropServices;

namespace NetworkProgram02_server
{
    public partial class Form1 : Form
    {   
        //TCP建立
        TcpListener server;
        Socket[] client=new Socket[2];
        Thread Th_Svr;
        Thread Th_Clt;
        Hashtable HT = new Hashtable();

        //UCP建立
        IPEndPoint ipep = new IPEndPoint(IPAddress.Parse("127.0.0.1"), 1235);
        UdpClient uc = new UdpClient();
        IPEndPoint ipep2 = new IPEndPoint(IPAddress.Parse("127.0.0.1"), 1236);
        //UdpClient uc2 = new UdpClient();

        int i;
        int count = 0;
        const int port = 1234;
        const string ip = "127.0.0.1";
        bool nowtypeblack = true;
        public Form1()
        {
            InitializeComponent();
            /*
            AllocConsole();
            Console.CancelKeyPress += new
                ConsoleCancelEventHandler(Console_CancelKeyPress);
            //Console.Beep();

            ConsoleColor oriColor = Console.ForegroundColor;
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine(
                "* Don't close this console window or the application will also close.");
            Console.WriteLine();

            Console.WriteLine("這是伺服器...\n");

            Console.ForegroundColor = oriColor;
            System.Console.WriteLine("Hello, World!");*/
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
               // MessageBox.Show("遠端主機已關閉");
                for(int i=0;i<2;i++)
                    client[i].Close();
                Th_Svr = new Thread(Serversub);
                Th_Svr.Start();
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
                    if (Msg == "black" || Msg == "white")
                    {
                        System.Console.WriteLine("Hello!");
                        count++;
                        if (count == 2)
                        {
                            WinMessage();
                            count = 0;
                        }

                    }
                    else
                    {
                        System.Console.WriteLine("number!");

                        for (int j = 0; j < 2; j++)
                        {
                            if (j != id)
                            {
                                Send(Msg, j);
                            }

                        }
                        UDPServer(Msg);//要擺在Send後面
                    }
                }
                catch (Exception)
                {
                    return;
                }
            }
        }
        private void Serversub()//接收Client端
        {
            for (i = 0; i < 2; i++)
            {
                client[i] = server.AcceptSocket();
                Send((i + 1).ToString(), i);//用來判斷client先後
                Th_Clt = new Thread(Listen);//宣告Th_Clt是一個新的執行緒(Listen)
                Th_Clt.IsBackground = true;
                Th_Clt.Start();
            }
        }
        public void UDPServer(string Str)//擷取TCP傳送內容，使用UDP廣播至udp_client
        {

            int x = 0, y = 0;
            for (i = 0; i < Str.Length; i++) //把訊息中的x,y提取出來
            {
                if (Str[i] == ' ')
                {
                    x = int.Parse(Str.Substring(0, i));
                    break;
                }
            }
            y = int.Parse(Str.Substring(i + 1));

            //將x,y轉換成棋盤座標
            int X = 16 - y;
            char Y = (Char)(x + 64);
            Str = Y + " " + X.ToString();

            /*for (int i = 0; i < 10; i++)
            {
                byte[] b = System.Text.Encoding.UTF8.GetBytes("這是'客戶端'傳送的訊息 ~ " + i);
                uc.Send(b, b.Length, ipep);
                Console.WriteLine(System.Text.Encoding.UTF8.GetString(uc.Receive(ref ipep)));
            }*/
            //byte[] B = Encoding.Default.GetBytes(Str);

            try
            {
                if (nowtypeblack)//判斷黑還是白在下，將訊息傳給兩個udp_client
                {
                    byte[] B = System.Text.Encoding.UTF8.GetBytes("黑子下在 :" + Str);
                    uc.Send(B, B.Length, ipep);
                    uc.Send(B, B.Length, ipep2);
                    nowtypeblack = false;
                    //Console.WriteLine(Str);
                }
                else
                {
                    byte[] B = System.Text.Encoding.UTF8.GetBytes("白子下在 :" + Str);
                    uc.Send(B, B.Length, ipep);
                    uc.Send(B, B.Length, ipep2);
                    nowtypeblack = true;
                   // Console.WriteLine(Str);
                }
                //Console.WriteLine(System.Text.Encoding.UTF8.GetString(uc.Receive(ref ipep)));
            }
            catch (Exception)
            {
                return;
            }
        }
        public void WinMessage()//由Listen告知已經結束，廣播誰輸誰贏
        {
            if (!nowtypeblack)
            {
                //Console.WriteLine("黑...\n");
                byte[] B = System.Text.Encoding.UTF8.GetBytes("black");
                uc.Send(B, B.Length, ipep);
                uc.Send(B, B.Length, ipep2);
                nowtypeblack = true;
            }
            else
            {
               // Console.WriteLine("白...\n");
                byte[] B = System.Text.Encoding.UTF8.GetBytes("white");
                uc.Send(B, B.Length, ipep);
                uc.Send(B, B.Length, ipep2);
                nowtypeblack = true;
            }
        }
        private void button1_Click(object sender, EventArgs e)
        {
            CheckForIllegalCrossThreadCalls = false;
            Th_Svr = new Thread(Serversub);
            Th_Svr.IsBackground = true;
            IPEndPoint EP = new IPEndPoint(IPAddress.Parse(ip), port);
            server = new TcpListener(EP);
            server.Start(100);
            Th_Svr.Start();
            button1.Enabled = false;
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            Application.ExitThread();
        }
        /*
        static void Console_CancelKeyPress(object sender,
                ConsoleCancelEventArgs e)
        {
            e.Cancel = true;

        }
        [DllImport("kernel32.dll", SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        static extern bool AllocConsole();

        [DllImport("Kernel32")]
        public static extern void FreeConsole();
        */
    }
}

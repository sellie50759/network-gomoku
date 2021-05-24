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
namespace GOMOKU
{
    public partial class Form1 : Form
    {
        private Game game = new Game();
        private AI aigame = new AI();
        bool AlreadyWin;
        Thread rec;
        //Thread win;//處理敵人回合勝利時，rec關不掉的問題
        const string ip = "127.0.0.1";
        const int port = 1234;
        string msg;
        Socket T;
        const string user="sellie";
        int gamemode = 0;//0 AI 1 雙人遊戲 2 網路雙人遊戲
        // private Board b = new Board();
        bool issend = false;//自己送出去了沒有
        bool who = true;
        bool error = false;//連線時是否發生錯誤
        //bool flag = false;//是否是對方的回合
        private delegate void SafeCallDelegate(piece p);
        Thread udprec;
        bool BorW = true;
        public Form1()
        {
            InitializeComponent();
            
            AllocConsole();
            Console.CancelKeyPress += new
                ConsoleCancelEventHandler(Console_CancelKeyPress);
            //Console.Beep();
            
            ConsoleColor oriColor = Console.ForegroundColor;
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine(
                "等待連結...");
            /*
            Console.WriteLine();

            Console.WriteLine("這是伺服器...\n");

            Console.ForegroundColor = oriColor;
            System.Console.WriteLine("Hello, World!");
            */

            gamemode = 0;
            AlreadyWin = false;
            label1.Visible = false;
            timer1.Start();
            //this.Controls.Add(new White(50, 35));
            //this.Controls.Add(new Black(50, 35));
        }
        private void  UdpClient()
        {
            int portnum;
            try
            {   
                if(BorW)
                {
                    Console.WriteLine("您是黑方...");
                    Console.WriteLine("-----------");
                    portnum = 1235;

                }
                else
                {
                    Console.WriteLine("您是白方...");
                    Console.WriteLine("-----------");
                    portnum = 1236;
                }

                IPEndPoint ipep = new IPEndPoint(IPAddress.Parse("127.0.0.1"), portnum);
                UdpClient uc = new UdpClient(ipep.Port);

                int i = 1;
                Console.WriteLine("第" + (i++).ToString() + "回合");
                Console.WriteLine("-----------");
                while (true)
                {
                    string ss = System.Text.Encoding.UTF8.GetString(uc.Receive(ref ipep));
                    if (ss == "black")
                    {
                        Console.WriteLine("黑色贏了!");
                        Console.WriteLine("-----------");
                        Console.WriteLine("第" + (i++).ToString() + "回合");
                        Console.WriteLine("-----------");
                    }
                    else if (ss == "white")
                    {
                        Console.WriteLine("白色贏了!");
                        Console.WriteLine("-----------");
                        Console.WriteLine("第" + (i++).ToString() + "回合");
                        Console.WriteLine("-----------");
                    }
                    else
                    {
                        Console.WriteLine(ss);
                    }
                    //byte[] b = System.Text.Encoding.UTF8.GetBytes("這是'伺服器'回傳的訊息 ~ " + i++);
                    //uc.Send(b, b.Length, ipep);
                }
            }
            catch(Exception)
            {
                return; 
            }
        }
        private void Send(string Str)
        {
            byte[] B = Encoding.Default.GetBytes(Str);
            try
            {
                T.Send(B, 0, B.Length, SocketFlags.None);
            }
            catch (Exception)
            {
                MessageBox.Show("遠端主機已關閉");
                T.Close();
                Application.Exit();
            }
        }
        private string Recieve()
        {
            byte[] B = new byte[1023];
            int inLen = T.Receive(B);
            string Msg = Encoding.Default.GetString(B, 0, inLen);
            return Msg;
        }
        private void ThreadRecieve()
        {
            byte[] B = new byte[1023];
            int inLen = T.Receive(B);
            msg = Encoding.Default.GetString(B, 0, inLen);
        }
        private void connect_server()
        {
                string IP = ip;
                int Port = port;
                IPEndPoint EP = new IPEndPoint(IPAddress.Parse(IP), Port);
                T = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                try
                {
                    T.Connect(EP);
                }
                catch (Exception)
                {
                    MessageBox.Show("無法連上伺服器");
                    error = true;
                    return;
                }
        }
        /*private void Form1_MouseDown(object sender, MouseEventArgs e)
        {              
                piece p =game.placepiece(e.X, e.Y); //傳鼠標座標給function
                if(p!=null)
                {
                    this.Controls.Add(p);//拿到的棋子資訊就顯示在視窗

                    //看Game裡的Winner傳出的勝利者是誰
                    if (game.Winner == Ptype.BLACK)
                    {
                        MessageBox.Show("黑色獲勝");                  
                    }
                    else if (game.Winner == Ptype.WHITE)
                    {
                        MessageBox.Show("白色獲勝");
                    }
                }  
        }*/
        private void Form1_MouseDown(object sender, MouseEventArgs e)
        {
            piece p;
            if (gamemode==0)
            {
                p = aigame.placepiece(e.X, e.Y,true);
                WinMessage(p);
                if (AlreadyWin==false&&p !=null)
                {
                    p = aigame.AINextPiece();
                    WinMessage(p);
                }
                if(AlreadyWin)
                {
                    reset();
                }
            }
            else if(gamemode==1)
            {
                p = game.placepiece(e.X, e.Y,true); //傳鼠標座標給function
                WinMessage(p);
            }
            else {
                if (issend == false)
                {
                    p = game.placepiece(e.X, e.Y, true);
                    Point temp = game.GetMatrixCoordinate(e.X, e.Y);
                    if (temp != Board.Returnnomatch())
                    {
                        Send(temp.X.ToString() + ' ' + temp.Y.ToString());
                        WinMessage(p);
                        issend = true;
                    }
                }
                /*
                if (isrec)
                {
                    string Msg = Message;
                    int x = 0, y, i;
                    for (i = 0; i < Msg.Length; i++) //把訊息中的x,y提取出來
                    {
                        if (Msg[i] == ' ')
                        {
                            x = int.Parse(Msg.Substring(0, i));
                            break;
                        }
                    }
                    y = int.Parse(Msg.Substring(i + 1));
                    p = game.placepiece(x, y, false);
                    WinMessage(p);
                    isrec = false;
                    issend = false;
                }
                */
            }
        }
        private void OppenentRound()
        {
            while (true)
            {
                piece p;
                string Msg;
                try
                {
                     Msg= Recieve();
                }
                catch (Exception)
                {
                    return;
                }
                int x = 0, y, i;
                for (i = 0; i < Msg.Length; i++) //把訊息中的x,y提取出來
                {
                    if (Msg[i] == ' ')
                    {
                        x = int.Parse(Msg.Substring(0, i));
                        break;
                    }
                }
                y = int.Parse(Msg.Substring(i + 1));
                p = game.placepiece(x, y, false);
                WinMessage(p);
                issend = false;
                Thread.Sleep(150);
                //flag = false;
            }
            ;
        }
        private void AddObject(piece p)
        {
            if (this.InvokeRequired)
            {
                var d = new SafeCallDelegate(AddObject);//https://docs.microsoft.com/zh-tw/dotnet/desktop/winforms/controls/how-to-make-thread-safe-calls-to-windows-forms-controls?view=netframeworkdesktop-4.8
                this.Invoke(d, new object[] { p });
            }
            else
            {
                this.Controls.Add(p);
            }
        }
        private void WinMessage(piece p)
        {
            if (p != null)
            {   

                if(gamemode==0)
                {
                    Point PP = aigame.Pictureboxpoint();
                    if (who)
                    {
                        pictureBox1.Left = PP.X;
                        pictureBox1.Top = PP.Y;
                        pictureBox1.BringToFront();
                        who = false;
                    }
                    else
                    {
                        pictureBox2.Left = PP.X;
                        pictureBox2.Top = PP.Y;
                        pictureBox2.BringToFront();
                        who = true;
                    }
                }
                else if(gamemode==1||gamemode==2)
                {
                    Point PP = game.Pictureboxpoint();
                    if (who)
                    {
                        pictureBox1.Left = PP.X;
                        pictureBox1.Top = PP.Y;
                        pictureBox1.BringToFront();
                        who = false;
                    }
                    else
                    {
                        pictureBox2.Left = PP.X;
                        pictureBox2.Top = PP.Y;
                        pictureBox2.BringToFront();
                        who = true;
                    }
                }
                AddObject(p);
                if (gamemode==0)
                {
                    if (aigame.Winner == Ptype.BLACK)
                    {
                        MessageBox.Show("黑色獲勝");
                        AlreadyWin = true;
                    }
                    else if (aigame.Winner == Ptype.WHITE)
                    {
                        MessageBox.Show("白色獲勝");
                        AlreadyWin = true;
                    }
                }
                else if (gamemode == 1)
                {
                    //看Game裡的Winner傳出的勝利者是誰
                    if (game.Winner == Ptype.BLACK)
                    {
                        MessageBox.Show("黑色獲勝");
                        AlreadyWin = true;
                    }
                    else if (game.Winner == Ptype.WHITE)
                    {
                        MessageBox.Show("白色獲勝");
                        AlreadyWin = true;
                    }
                }
                else if (gamemode == 2)
                {
                    //看Game裡的Winner傳出的勝利者是誰
                    if (game.Winner == Ptype.BLACK)
                    {
                        MessageBox.Show("黑色獲勝");
                        AlreadyWin = true;
                        Send("black");
                    }
                    else if (game.Winner == Ptype.WHITE)
                    {
                        MessageBox.Show("白色獲勝");
                        AlreadyWin = true;
                        Send("white");
                    }
                }
            }
        }
        private void reset()
        {
            AlreadyWin = false;
            pictureBox1.Left=-100;
            pictureBox1.Top=-100;
            pictureBox2.Left = -100;
            pictureBox2.Top = -100;
            who = true;
            if (gamemode==0)
            {
                for(int i = 0; i < 17; i++)
                {
                    for (int j = 0; j < 17; j++)
                    {
                        piece rr = aigame.re(i, j);
                        this.Controls.Remove(rr);
                    }
                }
                aigame = new AI();
            }
            else if(gamemode==1||gamemode==2)
            {
                for (int i = 0; i < 17; i++)
                {
                    for (int j = 0; j < 17; j++)
                    {
                        piece rr = game.re(i, j);
                        this.Controls.Remove(rr);
                    }
                }
                game = new Game();
                if (gamemode == 2)
                {
                    T.Close();
                    Thread.Sleep(150);//為了讓rec關閉
                    connect_server();
                    Thread temp = new Thread(ThreadRecieve);
                    temp.IsBackground = true;
                    temp.Start();
                    temp.Join();
                    if (msg == "1")
                        issend = false;
                    else issend = true;
                    CheckForIllegalCrossThreadCalls = false;
                    who = true;
                    rec = new Thread(OppenentRound);
                    rec.IsBackground = true;
                    rec.Start();
                    gamemode = 2;
                    //flag = false;
                }
            }
        }
        private void Form1_MouseMove(object sender, MouseEventArgs e)
        {
            //label1.Text = e.X.ToString() + " " + e.Y.ToString();//顯示滑鼠游標 測試用
            if (gamemode==0)
            {
                //Point where =aigame.Wherenode(e.X, e.Y);
                //label1.Text = where.X.ToString() + " " + where.Y.ToString();//顯示滑鼠游標 測試用
                if (aigame.CanPlace(e.X, e.Y))//判定該位置可不可以放 用換鼠標的方式提示
                {
                    this.Cursor = Cursors.Hand;
                    //label1.Text = game.returnFive().ToString();
                }
                else
                {
                    this.Cursor = Cursors.Default;
                    //label1.Text = game.returnFive().ToString();
                }
            }
            else if(gamemode==1)
            {
                if (game.CanPlace(e.X, e.Y))//判定該位置可不可以放 用換鼠標的方式提示
                {
                    this.Cursor = Cursors.Hand;
                    //label1.Text = game.returnFive().ToString();
                }
                else
                {
                    this.Cursor = Cursors.Default;
                    //label1.Text = game.returnFive().ToString();
                }
            }
            else
            {
                if (game.CanPlace(e.X, e.Y)&&issend==false)//判定該位置可不可以放 用換鼠標的方式提示
                {
                    this.Cursor = Cursors.Hand;
                    //label1.Text = game.returnFive().ToString();
                }
                else
                {
                    this.Cursor = Cursors.Default;
                    //label1.Text = game.returnFive().ToString();
                }
            }
        }
        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void 遊戲模式ToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        private void 與AI對戰ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            reset();
            gamemode = 0;

        }


        private void 雙人對戰ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            reset();
            gamemode = 1;
            Console.ReadKey();
        }

        private void 網路雙人對戰ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            reset();
            connect_server();
            if (error==false)
            {
                string Msg = Recieve();
                if (Msg == "1")
                {   
                    BorW = true;
                    issend = false;
                }     
                else
                {
                    BorW = false;
                    issend = true;
                }
                CheckForIllegalCrossThreadCalls = false;
                rec = new Thread(OppenentRound);
                rec.IsBackground = true;
                rec.Start();

                udprec = new Thread(UdpClient);
                udprec.IsBackground = true;
                udprec.Start();

                gamemode = 2;
                menuStrip1.Enabled = false;
                
              
            }
            else error = false;
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            if (AlreadyWin)
                reset();
        }
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
    }
}

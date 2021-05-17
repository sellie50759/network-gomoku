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
namespace GOMOKU
{
    public partial class Form1 : Form
    {
        private Game game = new Game();
        private AI aigame = new AI();
        bool AlreadyWin;
        const string ip = "127.0.0.1";
        const int port = 1234;
        Socket T;
        const string user="sellie";
        int gamemode = 0;//0 AI 1 雙人遊戲 2 網路雙人遊戲
        // private Board b = new Board();

        bool who = true;
        //bool flag = false;//是否是對方的回合
        public Form1()
        {
            InitializeComponent();
            gamemode = 0;
            AlreadyWin = false;
            label1.Visible = false;

            //this.Controls.Add(new White(50, 35));
            //this.Controls.Add(new Black(50, 35));
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
                p = game.placepiece(e.X, e.Y, true);
                Point temp = game.GetMatrixCoordinate(e.X, e.Y);
                Send(temp.X.ToString() + ' ' + temp.Y.ToString());
                WinMessage(p);
                string Msg = Recieve();
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
            }
        }
        private void OppenentRound()
        {
            piece p;
            string Msg = Recieve();
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
            //flag = false;
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
                this.Controls.Add(p);//拿到的棋子資訊就顯示在視窗
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
                else if(gamemode==1||gamemode==2)
                {
                    //看Game裡的Winner傳出的勝利者是誰
                    if (game.Winner == Ptype.BLACK)
                    {
                        MessageBox.Show("黑色獲勝");
                        reset();
                    }
                    else if (game.Winner == Ptype.WHITE)
                    {
                        MessageBox.Show("白色獲勝");
                        reset();
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
                    connect_server();
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
            else if(gamemode==1||gamemode==2)
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
        }

        private void 網路雙人對戰ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            reset();
            connect_server();
            gamemode = 2;
        }
    }
}

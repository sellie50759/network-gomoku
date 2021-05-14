using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace GOMOKU
{
    public partial class Form1 : Form
    {
        private Game game = new Game();
        private AI aigame = new AI();
        bool isAI,AlreadyWin;
        // private Board b = new Board();

        bool who = true;
        public Form1()
        {
            InitializeComponent();
            isAI = true;
            AlreadyWin = false;
            label1.Visible = false;

            //this.Controls.Add(new White(50, 35));
            //this.Controls.Add(new Black(50, 35));
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
            if (isAI)
            {
                p = aigame.placepiece(e.X, e.Y);
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
            else
            {
                p = game.placepiece(e.X, e.Y); //傳鼠標座標給function
                WinMessage(p);
            }
        }
        private void WinMessage(piece p)
        {
            if (p != null)
            {   

                if(isAI)
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
                else
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
                if (isAI)
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
                else
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
            if (isAI)
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
            else
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
            }
        }
        private void Form1_MouseMove(object sender, MouseEventArgs e)
        {
            //label1.Text = e.X.ToString() + " " + e.Y.ToString();//顯示滑鼠游標 測試用
            if (isAI)
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
            else
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
            isAI = true;
        }

        
            private void 雙人對戰ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            reset();
            isAI = false;
        }
    }
}

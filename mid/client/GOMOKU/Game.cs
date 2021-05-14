using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Drawing;

namespace GOMOKU
{
    class Game //五子棋的規則
    {
        protected Board board;

        protected Ptype nexttype;

        protected Ptype winner;
        public Ptype Winner { get { return winner; } }//唯讀 傳出winner
        public Game()
        {
             board = new Board();

             nexttype = Ptype.BLACK;//判斷棋子顏色 

             winner = Ptype.NONE;//預設勝利者是無
        }
        public bool CanPlace(int x,int y)//判斷可不可以放旗子
        {
            return board.CanPlace(x, y, true);
        }
        /*public Point Wherenode(int x,int y)
        {
            return board.wherenode(x,y);
        }*/
        public virtual piece  placepiece(int x,int y)
        {
            piece p = board.bePlace(x, y, nexttype, true);//獲取棋子下哪
            if (p != null)
            {
                whowin();//檢查是否有獲勝

                //更改下一顆棋子的顏色
                if (nexttype == Ptype.BLACK)
                    nexttype = Ptype.WHITE;
                else if (nexttype == Ptype.WHITE)
                    nexttype = Ptype.BLACK;

                return p;//回傳棋子資訊
            }
            return null;
        }
        public Point Pictureboxpoint()
        {
            return board.pictureboxpoint();
        }
        public piece re(int i,int j)
        {
            piece p = board.Re(i,j);
            return p;
        }
        public void rearray()//清空PArray;
        {
            board.Rearray();
            winner = Ptype.NONE;
            nexttype = Ptype.BLACK;
        }
        protected void whowin()
        {
            /*
            //拿到最後落子的X,Y座標
            int centerX = board.Lastnode.X;
            int centerY = board.Lastnode.Y;
            //檢查八個方向(3*3-1)，不含中心點
            int fourtime = 0;
            int Xdirection = -1, Ydirection = -1;
            for (Xdirection = -1; Xdirection <= 1; Xdirection++)
            {
                for (Ydirection = -1; Ydirection <= 1; Ydirection++)
                {
                    if (Xdirection == 0 && Ydirection == 0)//扣除中間的情況
                        break;

                    int number = 1;//紀錄看到幾顆相同棋子
                    int five = 1;
                    int Xdir = Xdirection, Ydir = Ydirection;
                    while (true)
                    {
                        int seeX = centerX + number * Xdir;
                        int seeY = centerY + number * Ydir;

                        //檢查顏色是否相同 ，且數字不超過矩陣大小
                        if (seeX < 0 || seeX >= Board.count ||
                        seeY < 0 || seeY >= Board.count ||
                        board.gettype(seeX, seeY) != nexttype)
                        {
                            if (Xdir == -1 && Ydir == -1) { Xdir = 1; Ydir = 1; fourtime++;number = 1; continue; }
                            else if (Xdir == -1 && Ydir == 0) { Xdir = 1; Ydir = 0; fourtime++; number = 1; continue; }
                            else if (Xdir== -1 && Ydir == 1) { Xdir = 1; Ydir = -1; fourtime++; number = 1; continue; }
                            else if (Xdir == 0 && Ydir == -1) { Xdir = 0; Ydir = 1; fourtime++; number = 1; continue; }
                            break;
                        }
                        five++;
                        number++;
                    }
                    if (five == 5)//檢查是否看到五顆棋子
                    {
                        winner = nexttype;
                    }
                }
                if (Xdirection == 0 && Ydirection == 0)//扣除中間的情況
                    break;
            }
            */
            int LeftTop = 1, Horizon = 1, LeftDown = 1, Vertical = 1, tempx, tempy,x=board.Lastnode.X,y=board.Lastnode.Y;
            tempx = x - 1;
            tempy = y - 1;
            while (tempx >= 0 && tempy >= 0 && board.gettype(tempx, tempy) == nexttype)
            {
                LeftTop++;
                tempx--;
                tempy--;
            }
            //設定左上的左上端點
            tempx = x + 1;
            tempy = y + 1;
            while (tempx < 15 && tempy < 15 && board.gettype(tempx, tempy) == nexttype)
            {
                LeftTop++;
                tempx++;
                tempy++;
            }
            //設定左上的右下端點
            tempx = x - 1;
            tempy = y;
            while (tempx >= 0 && board.gettype(tempx, tempy) == nexttype)
            {
                Horizon++;
                tempx--;
            }
            //設定水平的左端點
            tempx = x + 1;
            tempy = y;
            while (tempx < 15 && board.gettype(tempx, tempy) == nexttype)
            {
                Horizon++;
                tempx++;
            }
            //設定水平的右端點
            tempx = x;
            tempy = y - 1;
            while (tempy >= 0 && board.gettype(tempx, tempy) == nexttype)
            {
                Vertical++;
                tempy--;
            }
            //設定垂直的上端點
            tempx = x;
            tempy = y + 1;
            while (tempy < 15 && board.gettype(tempx, tempy) == nexttype)
            {
                Vertical++;
                tempy++;
            }
            //設定垂直的下端點
            tempx = x - 1;
            tempy = y + 1;
            while (tempx >= 0 && tempy < 15 && board.gettype(tempx, tempy) == nexttype)
            {
                LeftDown++;
                tempx--;
                tempy++;
            }
            //設定左下的左下端點
            tempx = x + 1;
            tempy = y - 1;
            while (tempx < 15 && tempy >= 0 && board.gettype(tempx, tempy) == nexttype)
            {
                LeftDown++;
                tempx++;
                tempy--;
            }
            if (LeftDown >= 5 || LeftTop >= 5 || Horizon >= 5 || Vertical >= 5)
            {
                winner = nexttype;
            }
        }
    }
}

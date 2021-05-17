using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

namespace GOMOKU
{
    class AI:Game
    {
        public AI() : base() { 
            nomatch = new Point(-100, -100);
        }
        Point NowPlace, OriginalPlace,nomatch;
        private static readonly int BeFive = 10000000;
        private static readonly int ActiveFour = 50000;
        private static readonly int DeadFour = 3000;
        private static readonly int ActiveThree = 3000;
        private static readonly int SleepThree = 200;
        private static readonly int ActiveTwo = 200;
        private static readonly int SleepTwo = 10;
        private static readonly int Other = 1;
        private static readonly int count=17;
        int[,] score = new int[count,count];
        public override piece placepiece(int x, int y,bool isOriginal)
        {
            piece p = board.bePlace(x, y, Ptype.BLACK,true);//獲取棋子下哪
            NowPlace=board.Findnode(x,y);
            OriginalPlace.X = x;
            OriginalPlace.Y = y;
            if (p != null)
            {
                whowin();//檢查是否有人獲勝
                nexttype = Ptype.WHITE;
                return p;//回傳棋子資訊
            }
            return null;
        }
        private bool isValid(int x,int y)
        {
            return x < count && x >= 0 && y < count && y >= 0;
        }
        /*private int getpiecenumber(int x,int y,int number)//mode=1 左上到右下 mode=2 水平 mode=3 左下到右上
        {
            if (board.gettype(x, y) !=nowtype)
            {
                return number;
            }
            if (mode == 1)
            {
                return getpiecenumber(x - 1, y - 1, number + 1)+getpiecenumber(x+1,y+1,number+1);
            }
        }
        private piece Defendmethod()
        {
            int LeftTop = -1, Horizon = -1, LeftDown = -1, Vertical = -1,x,y;
            Point[] LeftTopPoint= new Point[2],HorizonPoint=new Point[2],LeftDownPoint=new Point[2],VerticalPoint=new Point[2];
            x = NowPlace.X;
            y = NowPlace.Y;
            while (x >= 0 && y >= 0 && board.gettype(x, y)==nowtype)
            {
                LeftTop++;
                x--;
                y--;
            }
            if (isValid(x,y))
            {
                if (board.gettype(x, y) == Ptype.WHITE)
                {
                    LeftTop--;
                }
                LeftTopPoint[0].X = x;
                LeftTopPoint[0].Y = y;
            }
            else
            {
                LeftTopPoint[0] = nomatch;
            }
            //設定左上的左上端點
            x = NowPlace.X;
            y = NowPlace.Y;
            while (x < 15 && y < 15 && board.gettype(x, y) == nowtype)
            {
                LeftTop++;
                x++;
                y++;
            }
            if (isValid(x, y))
            {
                if (board.gettype(x, y) == Ptype.WHITE)
                {
                    LeftTop--;
                }
                LeftTopPoint[1].X = x;
                LeftTopPoint[1].Y = y;
            }
            else
            {
                LeftTopPoint[1] = nomatch;
            }
            //設定左上的右下端點
            x = NowPlace.X;
            y = NowPlace.Y;
            while (x >= 0 && board.gettype(x, y) == nowtype)
            {
                Horizon++;
                x--;
            }
            if (isValid(x, y))
            {
                if (board.gettype(x, y) == Ptype.WHITE)
                {
                    Horizon--;
                }
                HorizonPoint[0].X = x;
                HorizonPoint[0].Y = y;
            }
            else
            {
                HorizonPoint[0] = nomatch;
            }
            //設定水平的左端點
            x = NowPlace.X;
            y = NowPlace.Y;
            while (x < 15 && board.gettype(x, y) == nowtype)
            {
                Horizon++;
                x++;
            }
            if (isValid(x, y))
            {
                if (board.gettype(x, y) == Ptype.WHITE)
                {
                    Horizon--;
                }
                HorizonPoint[1].X = x;
                HorizonPoint[1].Y = y;
            }
            else
            {
                HorizonPoint[1] = nomatch;
            }
            //設定水平的右端點
            x = NowPlace.X;
            y = NowPlace.Y;
            while (y >= 0 && board.gettype(x, y) == nowtype)
            {
                Vertical++;
                y--;
            }
            if (isValid(x, y))
            {
                if (board.gettype(x, y) == Ptype.WHITE)
                {
                    Vertical--;
                }
                VerticalPoint[0].X = x;
                VerticalPoint[0].Y = y;
            }
            else
            {
                VerticalPoint[0] = nomatch;
            }
            //設定垂直的上端點
            x = NowPlace.X;
            y = NowPlace.Y;
            while (y < 15 && board.gettype(x, y) == nowtype)
            {
                Vertical++;
                y++;
            }
            if (isValid(x, y))
            {
                if (board.gettype(x, y) == Ptype.WHITE)
                {
                    Vertical--;
                }
                VerticalPoint[1].X = x;
                VerticalPoint[1].Y = y;
            }
            else
            {
                VerticalPoint[1] = nomatch;
            }
            //設定垂直的下端點
            x = NowPlace.X;
            y = NowPlace.Y;
            while (x >= 0 && y < 15 && board.gettype(x, y) == nowtype)
            {
                LeftDown++;
                x--;
                y++;
            }
            if (isValid(x, y))
            {
                if (board.gettype(x, y) == Ptype.WHITE)
                {
                    LeftDown--;
                }
                LeftDownPoint[0].X = x;
                LeftDownPoint[0].Y = y;
            }
            else
            {
                LeftDownPoint[0] = nomatch;
            }
            //設定左下的右上端點
            x = NowPlace.X;
            y = NowPlace.Y;
            while (x < 15 && y >= 0 && board.gettype(x, y) == nowtype)
            {
                LeftDown++;
                x++;
                y--;
            }
            if (isValid(x, y))
            {
                if (board.gettype(x, y) == Ptype.WHITE)
                {
                    LeftDown--;
                }
                LeftDownPoint[1].X = x;
                LeftDownPoint[1].Y = y;
            }
            else
            {
                LeftDownPoint[1] = nomatch;
            }
            //設定左下的左下端點
               if (LeftTop == 4 || LeftDown == 4 || Horizon == 4||Vertical==4) //死四
            {
                if (LeftTop == 4)
                {
                    if (board.CanPlace(LeftTopPoint[0].X, LeftTopPoint[0].Y,false))
                        return board.bePlace(LeftTopPoint[0].X, LeftTopPoint[0].Y,Ptype.WHITE, false);
                    else if (board.CanPlace(LeftTopPoint[1].X, LeftTopPoint[1].Y, false))
                        return board.bePlace(LeftTopPoint[1].X, LeftTopPoint[1].Y, Ptype.WHITE, false);
                }
                if (Horizon == 4)
                {
                    if (board.CanPlace(HorizonPoint[0].X, HorizonPoint[0].Y, false))
                        return board.bePlace(HorizonPoint[0].X, HorizonPoint[0].Y, Ptype.WHITE, false);
                    else if (board.CanPlace(HorizonPoint[1].X, HorizonPoint[1].Y, false))
                        return board.bePlace(HorizonPoint[1].X, HorizonPoint[1].Y, Ptype.WHITE, false);
                }
                if(LeftDown==4)
                {
                    if (board.CanPlace(LeftDownPoint[0].X, LeftDownPoint[0].Y, false))
                        return board.bePlace(LeftDownPoint[0].X, LeftDownPoint[0].Y, Ptype.WHITE, false);
                    else if (board.CanPlace(LeftDownPoint[1].X, LeftDownPoint[1].Y, false))
                        return board.bePlace(LeftDownPoint[1].X, LeftDownPoint[1].Y, Ptype.WHITE, false);
                }
                if (Vertical == 4)
                {
                    if (board.CanPlace(VerticalPoint[0].X, VerticalPoint[0].Y, false))
                        return board.bePlace(VerticalPoint[0].X, VerticalPoint[0].Y, Ptype.WHITE, false);
                    else if (board.CanPlace(VerticalPoint[1].X, VerticalPoint[1].Y, false))
                        return board.bePlace(VerticalPoint[1].X, VerticalPoint[1].Y, Ptype.WHITE, false);
                }
            }
            if (LeftTop == 3 || LeftDown == 3 || Horizon == 3||Vertical==3)
            {
                if (LeftTop == 3)
                {
                    if (board.CanPlace(LeftTopPoint[0].X, LeftTopPoint[0].Y, false))
                        return board.bePlace(LeftTopPoint[0].X, LeftTopPoint[0].Y, Ptype.WHITE, false);
                    else if (board.CanPlace(LeftTopPoint[1].X, LeftTopPoint[1].Y, false))
                        return board.bePlace(LeftTopPoint[1].X, LeftTopPoint[1].Y, Ptype.WHITE, false);
                }
                if (Horizon == 3)
                {
                    if (board.CanPlace(HorizonPoint[0].X, HorizonPoint[0].Y, false))
                        return board.bePlace(HorizonPoint[0].X, HorizonPoint[0].Y, Ptype.WHITE, false);
                    else if (board.CanPlace(HorizonPoint[1].X, HorizonPoint[1].Y, false))
                        return board.bePlace(HorizonPoint[1].X, HorizonPoint[1].Y, Ptype.WHITE, false);
                }
                if(LeftDown==3)
                {
                    if (board.CanPlace(LeftDownPoint[0].X, LeftDownPoint[0].Y, false))
                        return board.bePlace(LeftDownPoint[0].X, LeftDownPoint[0].Y, Ptype.WHITE, false);
                    else if (board.CanPlace(LeftDownPoint[1].X, LeftDownPoint[1].Y, false))
                        return board.bePlace(LeftDownPoint[1].X, LeftDownPoint[1].Y, Ptype.WHITE, false);
                }
                if (Vertical == 3)
                {
                    if (board.CanPlace(VerticalPoint[0].X, VerticalPoint[0].Y, false))
                        return board.bePlace(VerticalPoint[0].X, VerticalPoint[0].Y, Ptype.WHITE, false);
                    else if (board.CanPlace(VerticalPoint[1].X, VerticalPoint[1].Y, false))
                        return board.bePlace(VerticalPoint[1].X, VerticalPoint[1].Y, Ptype.WHITE, false);
                }
            }
            Queue<Point> qp=new Queue<Point>();//如果都沒有就從中心開始搜索，在離玩家落子最近的地方下
            bool[,] map = new bool[15, 15];
            qp.Enqueue(NowPlace);
            while (qp.Count != 0)
            {
                Point temp = qp.Peek();
                if (board.CanPlace(temp.X, temp.Y, false))
                    return board.bePlace(temp.X, temp.Y,Ptype.WHITE, false);
                map[temp.X,temp.Y] = true;
                for(int i = -1; i <= 1; i++)
                {
                    for(int j = -1; j <= 1; j++)
                    {
                        if (temp.X+i>=0&&temp.X+i<15&&temp.Y+j>=0&&temp.Y+j<15&&map[temp.X + i, temp.Y + j] == false) {
                            Point k = new Point(temp.X + i, temp.Y + j);
                            qp.Enqueue(k);
                        }
                    }
                }
                qp.Dequeue();
            }
            return null;
        }
*/
        /*
        private piece Attackmethod()
        {

        }
        */
        private int toval(int x,int val,bool isBlack)
        {
            if (x >= 5)
            {
                if (isBlack)
                    return BeFive;
                else return BeFive * 2;
            }
            else if (x == 4 && val == 0)
            {
                if (isBlack)
                    return ActiveFour;
                else
                {
                    double temp = ActiveFour * 1.8;
                    return (int)temp;
                }
            }
            else if (x == 4 && val == 1)
            {
                if (isBlack)
                    return DeadFour;
                else
                {
                    double temp = DeadFour * 1.2;
                    return (int)temp;
                }
            }
            else if (x == 3 && val == 0)
            {
                if (isBlack)
                    return ActiveThree;
                else
                {
                    double temp = ActiveThree *1.2;
                    return (int)temp;
                }
            }
            else if (x == 3 && val == 1)
            {
                if (isBlack)
                    return SleepThree;
                else
                {
                    double temp = SleepThree * 1.2;
                    return (int)temp;
                }
            }
            else if (x == 2 && val == 0)
            {
                if (isBlack)
                    return ActiveTwo;
                else
                {
                    double temp = ActiveTwo * 1.2;
                    return (int)temp;
                }
            }
            else if (x == 2 && val == 1)
            {
                if (isBlack)
                    return SleepTwo;
                else
                {
                    double temp = SleepTwo * 1.2;
                    return (int)temp;
                }
            }
            else return Other;
            
        }
        private int compute(int x,int y)
        {
            int LeftTop = 1, Horizon = 1, LeftDown = 1, Vertical = 1,tempx,tempy;
            int sum = 0;
            int LeftTopNode = 0, HorizonNode = 0, LeftDownNode = 0, VerticalNode = 0;
            tempx = x-1;
            tempy = y-1;
            while (tempx >= 0 && tempy >= 0 && board.gettype(tempx, tempy) == Ptype.BLACK)
            {
                LeftTop++;
                tempx--;
                tempy--;
            }
            if (isValid(tempx, tempy))
            {
                if (board.gettype(tempx, tempy) == Ptype.WHITE|| board.gettype(tempx, tempy) == Ptype.BORDER)
                    LeftTopNode++;
            }
            //設定左上的左上端點
            tempx = x+1;
            tempy = y+1;
            while (tempx < 15 && tempy < count && board.gettype(tempx, tempy) == Ptype.BLACK)
            {
                LeftTop++;
                tempx++;
                tempy++;
            }
            if (isValid(tempx, tempy))
            {
                if (board.gettype(tempx, tempy) == Ptype.WHITE || board.gettype(tempx, tempy) == Ptype.BORDER)
                    LeftTopNode++;
            }
            //設定左上的右下端點
            tempx = x-1;
            tempy = y;
            while (tempx >= 0 && board.gettype(tempx, tempy) == Ptype.BLACK)
            {
                Horizon++;
                tempx--;
            }
            if (isValid(tempx, tempy))
            {
                if (board.gettype(tempx, tempy) == Ptype.WHITE || board.gettype(tempx, tempy) == Ptype.BORDER)
                    HorizonNode++;
            }
            //設定水平的左端點
            tempx = x+1;
            tempy = y;
            while (tempx < count && board.gettype(tempx, tempy) == Ptype.BLACK)
            {
                Horizon++;
                tempx++;
            }
            if (isValid(tempx, tempy))
            {
                if (board.gettype(tempx, tempy) == Ptype.WHITE || board.gettype(tempx, tempy) == Ptype.BORDER)
                    HorizonNode++;
            }
            //設定水平的右端點
            tempx = x;
            tempy = y-1;
            while (tempy >= 0 && board.gettype(tempx, tempy) == Ptype.BLACK)
            {
                Vertical++;
                tempy--;
            }
            if (isValid(tempx, tempy))
            {
                if (board.gettype(tempx, tempy) == Ptype.WHITE || board.gettype(tempx, tempy) == Ptype.BORDER)
                    VerticalNode++;
            }
            //設定垂直的上端點
            tempx = x;
            tempy = y+1;
            while (tempy < count && board.gettype(tempx, tempy) == Ptype.BLACK)
            {
                Vertical++;
                tempy++;
            }
            if (isValid(tempx, tempy))
            {
                if (board.gettype(tempx, tempy) == Ptype.WHITE || board.gettype(tempx, tempy) == Ptype.BORDER)
                    VerticalNode++;
            }
            //設定垂直的下端點
            tempx = x-1;
            tempy = y+1;
            while (tempx >= 0 && tempy < count && board.gettype(tempx, tempy) == Ptype.BLACK)
            {
                LeftDown++;
                tempx--;
                tempy++;
            }
            if (isValid(tempx, tempy))
            {
                if (board.gettype(tempx, tempy) == Ptype.WHITE || board.gettype(tempx, tempy) == Ptype.BORDER)
                    LeftDownNode++;
            }
            //設定左下的左下端點
            tempx = x+1;
            tempy = y-1;
            while (tempx < count && tempy >= 0 && board.gettype(tempx, tempy) == Ptype.BLACK)
            {
                LeftDown++;
                tempx++;
                tempy--;
            }
            if (isValid(tempx, tempy))
            {
                if (board.gettype(tempx, tempy) == Ptype.WHITE || board.gettype(tempx, tempy) == Ptype.BORDER)
                    LeftDownNode++;
            }
            sum += toval(LeftDown,LeftDownNode,true) + toval(LeftTop,LeftTopNode,true) + toval(Horizon,HorizonNode,true) + toval(Vertical,VerticalNode,true);
            if (x == 1 && y == 7)
                ;
            LeftTop = 1; Horizon = 1; LeftDown = 1; Vertical = 1;
            LeftTopNode = 0; HorizonNode = 0; LeftDownNode = 0; VerticalNode = 0;
            tempx = x - 1;
            tempy = x - 1;
            while (tempx >= 0 && tempy >= 0 && board.gettype(tempx, tempy) == Ptype.WHITE)
            {
                LeftTop++;
                tempx--;
                tempy--;
            }
            if (isValid(tempx, tempy))
            {
                if (board.gettype(tempx, tempy) == Ptype.BLACK || board.gettype(tempx, tempy) == Ptype.BORDER)
                    LeftTopNode++;
            }
            //設定左上的左上端點
            tempx = x + 1;
            tempy = y + 1;
            while (tempx < count && tempy < count && board.gettype(tempx, tempy) == Ptype.WHITE)
            {
                LeftTop++;
                tempx++;
                tempy++;
            }
            if (isValid(tempx, tempy))
            {
                if (board.gettype(tempx, tempy) == Ptype.BLACK || board.gettype(tempx, tempy) == Ptype.BORDER)
                    LeftTopNode++;
            }
            //設定左上的右下端點
            tempx = x - 1;
            tempy = y;
            while (tempx >= 0 && board.gettype(tempx, tempy) == Ptype.WHITE)
            {
                Horizon++;
                tempx--;
            }
            if (isValid(tempx, tempy))
            {
                if (board.gettype(tempx, tempy) == Ptype.BLACK || board.gettype(tempx, tempy) == Ptype.BORDER)
                    HorizonNode++;
            }
            //設定水平的左端點
            tempx = x + 1;
            tempy = y;
            while (tempx < count && board.gettype(tempx, tempy) == Ptype.WHITE)
            {
                Horizon++;
                tempx++;
            }
            if (isValid(tempx, tempy))
            {
                if (board.gettype(tempx, tempy) == Ptype.BLACK || board.gettype(tempx, tempy) == Ptype.BORDER)
                    HorizonNode++;
            }
            //設定水平的右端點
            tempx = x;
            tempy = y - 1;
            while (tempy >= 0 && board.gettype(tempx, tempy) == Ptype.WHITE)
            {
                Vertical++;
                tempy--;
            }
            if (isValid(tempx, tempy))
            {
                if (board.gettype(tempx, tempy) == Ptype.BLACK || board.gettype(tempx, tempy) == Ptype.BORDER)
                    VerticalNode++;
            }
            //設定垂直的上端點
            tempx = x;
            tempy = y + 1;
            while (tempy < count && board.gettype(tempx, tempy) == Ptype.WHITE)
            {
                Vertical++;
                tempy++;
            }
            if (isValid(tempx, tempy))
            {
                if (board.gettype(tempx, tempy) == Ptype.BLACK || board.gettype(tempx, tempy) == Ptype.BORDER)
                    VerticalNode++;
            }
            //設定垂直的下端點
            tempx = x - 1;
            tempy = y + 1;
            while (tempx >= 0 && tempy < count && board.gettype(tempx, tempy) == Ptype.WHITE)
            {
                LeftDown++;
                tempx--;
                tempy++;
            }
            if (isValid(tempx, tempy))
            {
                if (board.gettype(tempx, tempy) == Ptype.BLACK || board.gettype(tempx, tempy) == Ptype.BORDER)
                    LeftDownNode++;
            }
            //設定左下的左下端點
            tempx = x + 1;
            tempy = y - 1;
            while (tempx < count && tempy >= 0 && board.gettype(tempx, tempy) == Ptype.WHITE)
            {
                LeftDown++;
                tempx++;
                tempy--;
            }
            if (isValid(tempx, tempy))
            {
                if (board.gettype(tempx, tempy) == Ptype.BLACK || board.gettype(tempx, tempy) == Ptype.BORDER)
                    LeftDownNode++;
            }
            sum += toval(LeftDown, LeftDownNode,false) + toval(LeftTop, LeftTopNode,false) + toval(Horizon, HorizonNode,false) + toval(Vertical, VerticalNode,false);
            if (x == 1 && y == 7)
                ;
            return sum;
        }
        private piece NewMethod()
        {
            int max = 0, maxi = -100, maxj = -100;
            for (int i = 1; i < count-1; i++)
            {
                for (int j = 1; j < count-1; j++)
                {
                    if (board.gettype(i, j) == Ptype.NONE)
                    {
                                               
                        score[i, j] = compute(i, j);
                        if (score[i, j] > max)
                        {
                            max = score[i, j];
                            maxi = i;
                            maxj = j;
                           
                        }
                    }
                }
            }
            int m=max;
            return board.bePlace(maxi, maxj, Ptype.WHITE, false);
        }
        public piece AINextPiece()
        {
            piece p = NewMethod();

            if( p != null)
            {
                whowin();
                nexttype = Ptype.BLACK;
                return p;
            }
            return null;
        }
    }
}

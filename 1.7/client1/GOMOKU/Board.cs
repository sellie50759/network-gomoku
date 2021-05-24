using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

namespace GOMOKU
{
    class Board
    {

        private static readonly int outsideleft = 58;//左邊的外框寬
        private static readonly int outsidetop = 65;//上方的外框長
        private static readonly int Radius = 10;//滑鼠游標要在棋點的判斷範圍才能下
        private static readonly int Distance = 44;//點與點的間隔
        private static readonly Point nomatch = new Point(-100, -100);//若點並不在正確的棋盤位置就給nomatch
        public static readonly int count = 17;//棋盤 大小 count*count

        private piece[,] PArray ;//設定棋盤

        private Point lastPiecetype; //落子的那個棋子點座標
        public Point Lastnode { get { return lastPiecetype; } }//唯讀 傳出 lastPiecetype
        public static Point Returnnomatch()
        {
            return nomatch;
        }
        public Board()
        {
            PArray = new piece[count, count];
            for (int i = 0; i < count; i++)
            {
                Point correct = correctlocation(new Point(i, 0));
                PArray[i, 0] = new border(correct.X, correct.Y);
                correct = correctlocation(new Point(0, i));
                PArray[0, i] = new border(correct.X, correct.Y);
                correct = correctlocation(new Point(16, i));
                PArray[16, i] = new border(correct.X, correct.Y);
                correct = correctlocation(new Point(i, 16));
                PArray[i, 16] = new border(correct.X, correct.Y);
            }
            lastPiecetype = nomatch;
        }
        public Ptype gettype(int nodedx, int nodedy)//告訴我那個位置是哪種棋子
        {
            if (PArray[nodedx, nodedy] == null)
                return Ptype.NONE;
            return PArray[nodedx, nodedy].Getpiecetype();
        }
        public bool CanPlace(int x, int y,bool isOriginal)//判斷可不可以放旗子
        {
            Point id = new Point();
            if (isOriginal)
            {
                id = Findnode(x, y);//找最近的棋點(第幾個交叉點)
            }
            else
            {
                id.X = x;
                id.Y = y;
            }
            if (id == nomatch)//如果沒有回傳false
                return false;
            if (PArray[id.X, id.Y] != null)//如果有，看是不是已擺放棋子
                return false;
            //if (id.X == 0 || id.Y == 0)
                //return false;
            return true;//滑鼠點擊的位置是在正確可以下棋的位置
        }
        public piece bePlace(int x, int y, Ptype type,bool isOriginal)//判斷可不可以放旗子多加上黑棋還白棋
        {
            Point id=new Point();
            if (isOriginal)
            {
                id = Findnode(x, y);//找最近的棋點(第幾個交叉點)
            }
            else
            {
                id.X = x;
                id.Y = y;
            }
            if (id == nomatch)  //如果沒有 回傳false
                return null;    //class 用null回傳
            if (PArray[id.X, id.Y] != null)//如果有，看是不是已擺放棋子
                return null;    //class 用null回傳

            Point correct = correctlocation(id);//傳矩陣座標給function

            //看type是黑還白產生棋子
            if (type == Ptype.BLACK)
                PArray[id.X, id.Y] = new Black(correct.X, correct.Y);
            else if (type == Ptype.WHITE)
                PArray[id.X, id.Y] = new White(correct.X, correct.Y);

            lastPiecetype = id;//記錄下最後棋子的矩陣位置
            return PArray[id.X, id.Y];//回傳棋子
        }
        public Point pictureboxpoint()
        {
            Point location = new Point();
            location.X = (lastPiecetype.X-1) * Distance + outsideleft-18;
            location.Y = (lastPiecetype.Y-1) * (Distance - 5) + outsidetop-18;
            return location;
        }
        private Point correctlocation(Point id)//將矩陣座標轉換成現實座標，是棋子的圓心座標
        {
            Point location = new Point();
            location.X = (id.X-1) * Distance + outsideleft;
            location.Y = (id.Y-1) * (Distance - 5) + outsidetop;
            return location;
        }
        public Point Findnode(int x, int y)//找到離鼠標最近的棋點，二維判斷
        {
            bool xory = true;//判斷X軸還Y軸

            int nodedx = Findnode(x - outsideleft, xory);//傳出 鼠標X軸減去左邊邊框
            if (nodedx == -1 || nodedx <=0 ||  nodedx >= count-1)//(矩陣範圍 1~15)
                return nomatch;

            xory = false;
            int nodedy = Findnode(y - outsidetop, xory);//傳出 鼠標Y軸減去上邊邊框
            if (nodedy == -1 || nodedy <=0  || nodedy >= count-1)//(矩陣範圍 1~15)
                return nomatch;

            return new Point(nodedx, nodedy);
        }
        /*public Point wherenode(int x,int y)
        {
            return new Point(x - outsideleft, y - outsidetop);
        }*/
        private int Findnode(int pos, bool xory)//找到離鼠標最近的棋點，一維判斷
        {   
            if(pos<=-10)
              return -1;
            int d;
            //判斷X軸還Y軸，給的棋點之間距離不一樣
            if (xory) { d = Distance; }
            else { d = Distance - 5; }

            int Quotient = pos / d;     //商
            int remainder = pos % d;    //餘數

            //判斷該落在哪一個位置
            if (remainder <= Radius)
                return Quotient + 1;
            else if (remainder >= d - Radius)
                return Quotient + 2;
            else
                return -1;
        }
        public piece Re(int i,int j)
        {
            return PArray[i, j];
        }
        public void Rearray()//清空PArray;
        {
            PArray= new piece[count, count];
        }
    }
}

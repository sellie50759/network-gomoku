using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GOMOKU
{
    class Black : piece //黑棋子
    {
        public Black(int x, int y):base(x,y)//給圖
        {
            this.Image = Properties.Resources.black;
        }
        public override Ptype Getpiecetype()//回傳黑棋子
        {
            return Ptype.BLACK;
        }
    }
}

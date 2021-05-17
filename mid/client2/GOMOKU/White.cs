using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GOMOKU
{
    class White:piece //白棋子
    {
        public White(int x, int y) : base(x, y)//給圖
        {
            this.Image = Properties.Resources.white;
        }
        public override Ptype Getpiecetype()//回傳白棋子
        {
            return Ptype.WHITE;
        }
    }
}

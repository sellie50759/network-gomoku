using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GOMOKU
{
    class border : piece //白棋子
    {
        public border(int x, int y) : base(x, y)
        {
            //haha is me
        }
        public override Ptype Getpiecetype()//回傳白棋子
        {
            return Ptype.BORDER;
        }
    }
}

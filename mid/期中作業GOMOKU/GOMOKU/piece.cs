using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Drawing;

namespace GOMOKU
{
    abstract class piece:PictureBox //設定棋子的大小
    {
        int width = 36; //棋子圖片的長寬
        public piece(int x,int y)
        {
            this.BackColor = Color.Transparent;
            this.Location = new Point(x- width / 2, y- width / 2);
            this.Size=new Size(width, width);
            this.SizeMode = PictureBoxSizeMode.StretchImage;
        }
        //交給black white的程式override內容
        public abstract Ptype Getpiecetype();


             
    }
}

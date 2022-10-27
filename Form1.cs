using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CG_4
{
    public partial class Form1 : Form
    {
        PictureBox field = new PictureBox();
        public Form1()
        {
            this.StartPosition = FormStartPosition.CenterScreen;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.FormBorderStyle = FormBorderStyle.FixedDialog;
            this.Text = "Paint";
            this.Size = new Size(600, 400);
            this.AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.Show();

            field.Size = new Size(600, 400);
            field.Image = new Bitmap(field.Width, field.Height);
            field.Location = new Point(0, 0);
            field.Visible = true;
            this.Controls.Add(field);
            field.MouseClick += FieldClick;
        }

        private void FieldClick(object sender, MouseEventArgs args)
        {

            (sender as PictureBox).Image = Draw((sender as PictureBox).Image.Size);
        }

        private Bitmap Draw(Size size)
        {
            int x = size.Width / 2;
            int y = size.Height / 2;
            Bitmap bm = new Bitmap(size.Width, size.Height);
            for (int i = 0; i < size.Width; i++)
            {
                for (int j = 0; j < size.Height; j++)
                {
                    bm.SetPixel(i, j, Color.White);
                }
            }
            bm.SetPixel(100, 100, Color.Black);
            int max = 100;
            for (int i = 0; i < max; i += 3)
            {
                if (i == 0) bm.SetPixel(x, y, Color.Black);
                else if (i > 0)
                {
                    bm.SetPixel(x + i, y, Color.Black);
                    bm.SetPixel(x - i, y, Color.Black);
                    bm.SetPixel(x, y + i, Color.Black);
                    bm.SetPixel(x, y - i, Color.Black);
                    for (int k = 0; k < i / 3; k++)
                    {
                        bm.SetPixel(x + i, y + k + 1, Color.Black);
                        bm.SetPixel(x + i, y - k - 1, Color.Black);
                        bm.SetPixel(x - i, y + k + 1, Color.Black);
                        bm.SetPixel(x - i, y - k - 1, Color.Black);
                        bm.SetPixel(x + k + 1, y + i, Color.Black);
                        bm.SetPixel(x - k - 1, y + i, Color.Black);
                        bm.SetPixel(x + k + 1, y - i, Color.Black);
                        bm.SetPixel(x - k - 1, y - i, Color.Black);
                    }

                    for (int k = 0; k < i * 2 / 3; k++)
                    {
                        bm.SetPixel(x + i - k, y - i / 3 - k, Color.Black);
                        bm.SetPixel(x - i + k, y - i / 3 - k, Color.Black);
                        bm.SetPixel(x - i + k, y + i / 3 + k, Color.Black);
                        bm.SetPixel(x + i - k, y + i / 3 + k, Color.Black);
                    }
                }
            }
            return bm;
        }
    }
}

﻿using System.Drawing;
using System.Windows.Forms;

namespace CG_4
{
    public partial class Form1 : Form
    {
        PictureBox initialPicture = new PictureBox();
        PictureBox grayPicture = new PictureBox();
        PictureBox thresPicture = new PictureBox();
        PictureBox diagPicture = new PictureBox();
        public Form1()
        {
            this.StartPosition = FormStartPosition.CenterScreen;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.FormBorderStyle = FormBorderStyle.FixedDialog;
            this.Size = new Size(Properties.Resources.picture_hand_compressed.Width*3+20, Properties.Resources.picture_hand_compressed.Height+ 10 + 256);
            this.AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.Show();

            initialPicture.Size = Properties.Resources.picture_hand_compressed.Size;
            initialPicture.Image = Properties.Resources.picture_hand_compressed;
            initialPicture.Location = new Point(0, 0);
            initialPicture.Visible = true;
            this.Controls.Add(initialPicture);
            initialPicture.MouseClick += initialPictureClick;

            grayPicture.Size = initialPicture.Size;
            grayPicture.Location = new Point(initialPicture.Width + 10, 0);
            grayPicture.Visible = true;
            this.Controls.Add(grayPicture);
            grayPicture.MouseClick += imageThresholding;

            thresPicture.Size = initialPicture.Size;
            thresPicture.Location = new Point((initialPicture.Width + 10)*2, 0);
            thresPicture.Visible = true;
            this.Controls.Add(thresPicture);

            diagPicture.Size = new Size(256, 256);
            diagPicture.Image = new Bitmap(256, 256);
            diagPicture.Location = new Point(0, (initialPicture.Height + 10));
            diagPicture.Visible = true;
            this.Controls.Add(diagPicture);
        }

        private void initialPictureClick(object sender, MouseEventArgs args)
        {
            Bitmap bm = (Bitmap)((sender as PictureBox).Image);
            Bitmap nbm = new Bitmap(bm.Width, bm.Height);
            for(int i = 0; i < bm.Width; i++)
            {
                for(int j = 0; j < bm.Height; j++)
                {
                    Color c = bm.GetPixel(i, j);
                    byte br = (byte)(c.R * 0.3 + c.G * 0.59 + c.B * 0.11);
                    nbm.SetPixel(i, j, Color.FromArgb(255, br, br, br));
                }
            }
            grayPicture.Image = nbm;
        }

        private void DrawDiag(byte[] diag)
        {
            Bitmap bm = new Bitmap(diag.Length, diag.Length);
            for(int i = 0; i < bm.Width; i++)
            {
                for(int j = 0; j < bm.Height; j++)
                {
                    bm.SetPixel(i, j, Color.FromArgb(255,255,255,255));
                }
            }
            for(int i = 0; i < diag.Length; i++)
            {
                bm.SetPixel(i, 255-diag[i], Color.FromArgb(255, 0, 0, 0));
            }
            diagPicture.Image = bm;
        }
        private void imageThresholding(object sender, MouseEventArgs args)
        {
             var diag = GiagramCreation(3);
            DrawDiag(diag);
            if ( (sender as PictureBox).Image != null){
                Bitmap bm = (Bitmap)((sender as PictureBox).Image);
                Bitmap nbm = new Bitmap(bm.Width, bm.Height);
                for(int i = 0; i < bm.Width; i++)
                {
                    for(int j = 0; j < bm.Height; j++)
                    {
                        Color c = bm.GetPixel(i, j);
                        byte br = (byte)((c.R + c.G + c.B)/3);
                        if (br > diag[br]) nbm.SetPixel(i, j, Color.FromArgb(255, 255, 255, 255));
                        else nbm.SetPixel(i, j, Color.FromArgb(255, 0, 0, 0));
                    }
                }
                thresPicture.Image = nbm;
            }
        }

        private byte[] GiagramCreation(int n)
        {
            byte[] diag = new byte[256];
            int dx = 256 / n;
            //int dy = n;
            int cx = 0;
            for(int i = 0; i < n; i++)
            {
                for(int j = cx; j < (i+1)*dx; j++)
                {
                    diag[j] = (byte)(j*n);
                }
                cx += dx;
            }
            return diag;
        }
        

        /*private void FieldClick(object sender, MouseEventArgs args)
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
        */
    }
}

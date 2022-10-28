﻿using System;
using System.Drawing;
using System.IO;
using System.Windows.Forms;

namespace CG_4
{
    public partial class Form1 : Form
    {
        private int nCount = 2;

        private Button Clear;
        private Button ToGrey;
        private Button ToThresh;
        private Button ToMask;

        private Button pCount;
        private Button mCount;

        private Bitmap initialPicture;

        private PictureBox InitialPicture = new PictureBox();
        private PictureBox GreyPicture = new PictureBox();
        private PictureBox ThresPicture = new PictureBox();
        private PictureBox MaskPicture = new PictureBox();
        public Form1()
        {
            Image im = Properties.Resources.no_signal;
            this.Text = "CG_4 V - 8";
            this.StartPosition = FormStartPosition.CenterScreen;
            this.FormBorderStyle = FormBorderStyle.FixedDialog;
            this.Size = new Size(im.Width, im.Height + 50);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.Show();

            InitialPicture.Size = im.Size;
            InitialPicture.Image = im;
            InitialPicture.Location = new Point(0, 0);
            this.Controls.Add(InitialPicture);
            InitialPicture.MouseClick += gettingInitialPicture;
        }

        private void gettingInitialPicture(object sender, EventArgs args)
        {
            using (OpenFileDialog openFileDialog = new OpenFileDialog())
            {
                openFileDialog.InitialDirectory = "c:\\Users\\User\\Downloads\\";
                openFileDialog.Filter = "txt files (*.txt)|*.txt|All files (*.*)|*.*";
                openFileDialog.FilterIndex = 2;
                openFileDialog.RestoreDirectory = true;

                if (openFileDialog.ShowDialog() == DialogResult.OK)
                {
                    var filePath = openFileDialog.FileName;
                    var im = (Bitmap)(Image.FromFile(filePath));
                    if (im != null)
                    {
                        initialPicture = im;
                        this.Controls.Clear();
                        InitializeComponents();
                    }
                }
            }
        }
        private void InitializeComponents()
        {
            nCount = 2;
            double maxWidth = 400.0;
            if (initialPicture.Width > (int)maxWidth) initialPicture = new Bitmap(initialPicture, new Size((int)maxWidth, (int)(maxWidth / initialPicture.Width * initialPicture.Height)));
            this.Size = new Size(initialPicture.Width * 4, initialPicture.Height + 100+50+25);
            this.Location = new Point((1920 - this.Width) / 2, (1080 - this.Height) / 2);

            InitialPicture.Size = initialPicture.Size;
            InitialPicture.Image = initialPicture;
            this.Controls.Add(InitialPicture);
            InitialPicture.MouseClick -= gettingInitialPicture;
            //InitialPicture.MouseClick += GreyPictureCreation;

            GreyPicture.Size = initialPicture.Size;
            GreyPicture.Location = new Point(initialPicture.Width, 0);
            GreyPicture.Image = null;
            this.Controls.Add(GreyPicture);
            //GreyPicture.MouseClick += ThresholdingPictureCreation;

            ThresPicture.Size = initialPicture.Size;
            ThresPicture.Location = new Point(initialPicture.Width * 2, 0);
            ThresPicture.Image = null;
            this.Controls.Add(ThresPicture);
            //ThresPicture.MouseClick += MaskFiltrationCreation;

            MaskPicture.Size = initialPicture.Size;
            MaskPicture.Location = new Point(initialPicture.Width * 3, 0);
            MaskPicture.Image = null;
            this.Controls.Add(MaskPicture);

            Clear = new Button();
            Clear.Text = "Исходная картинка";
            Clear.Size = new Size(200, 50);
            Clear.Location = new Point(InitialPicture.Width / 2 - Clear.Width / 2, initialPicture.Height + 25 );
            this.Controls.Add(Clear);
            Clear.Click += gettingInitialPicture;

            ToGrey = new Button();
            ToGrey.Text = "Картинка в оттенках серого";
            ToGrey.Size = new Size(200, 50);
            ToGrey.Location = new Point(InitialPicture.Width + InitialPicture.Width / 2 - ToGrey.Width / 2, initialPicture.Height + 25);
            this.Controls.Add(ToGrey);
            ToGrey.Click += GreyPictureCreation;

            ToThresh = new Button();
            ToThresh.Text = "Картинка после Пороговой Обработки ("+ nCount.ToString() + ")";
            ToThresh.Size = new Size(200, 50);
            ToThresh.Location = new Point(InitialPicture.Width*2 + InitialPicture.Width / 2 - ToThresh.Width / 2, initialPicture.Height + 25);
            this.Controls.Add(ToThresh);
            ToThresh.Click += ThresholdingPictureCreation;

            pCount = new Button();
            pCount.Text = "+";
            pCount.Size = new Size(50, 25);
            pCount.Location = new Point(InitialPicture.Width * 2 + InitialPicture.Width / 2 + pCount.Width, initialPicture.Height + 25 + ToThresh.Height + 15);
            this.Controls.Add(pCount);
            pCount.Click += nPlus;

            mCount = new Button();
            mCount.Text = "-";
            mCount.Size = new Size(50, 25);
            mCount.Location = new Point(InitialPicture.Width * 2 + InitialPicture.Width / 2 - 2*mCount.Width, initialPicture.Height + 25 + ToThresh.Height + 15);
            this.Controls.Add(mCount);
            mCount.Click += nMinus;

            ToMask = new Button();

            ToMask.Size = new Size(200, 50);
            ToMask.Text = "Картинка после Масочной Фильтрации";
            ToMask.Location = new Point(InitialPicture.Width*3 + InitialPicture.Width / 2 - ToMask.Width / 2, initialPicture.Height + 25);
            this.Controls.Add(ToMask);
            ToMask.Click += MaskFiltrationCreation;
        }

        private void nPlus(object sender, EventArgs args)
        {
            if (nCount < 20) nCount++;
            ToThresh.Text = "Картинка после Пороговой Обработки (" + nCount.ToString() + ")";
        }

        private void nMinus(object sender, EventArgs args)
        {
            if (nCount > 1) nCount--;
            ToThresh.Text = "Картинка после Пороговой Обработки (" + nCount.ToString() + ")";
        }

        private void MaskFiltrationCreation(object sender, EventArgs args)
        {
            Bitmap bm = (Bitmap)ThresPicture.Image;
            if (bm != null)
            {
                Bitmap nbm = new Bitmap(bm.Width, bm.Height);
                int a = 0;
                int b = 1;
                int[,] m = new int[,]
                {
              { 0, -1, 0 },
              { -1, 4, -1 },
              { 0, -1, 0 }
                };
                int n = m.GetLength(0);
                int t = (n - 1) / 2;
                for (int x = t; x < bm.Width - t; x++)
                {
                    for (int y = t; y < bm.Height - t; y++)
                    {

                        byte col = (byte)a;
                        for (int i = -t; i <= t; i++)
                        {
                            for (int j = -t; j <= t; j++)
                            {
                                col += (byte)(b * m[i + t, j + t] * bm.GetPixel(x + i, y + j).R);
                            }
                        }
                        nbm.SetPixel(x, y, Color.FromArgb(255, col, col, col));
                    }
                }
                MaskPicture.Image = nbm;
            }
        }

        private void GreyPictureCreation(object sender, EventArgs args)
        {
            Bitmap bm = (Bitmap)InitialPicture.Image;
            if (bm != null)
            {
                Bitmap nbm = new Bitmap(bm.Width, bm.Height);
                for (int i = 0; i < bm.Width; i++)
                {
                    for (int j = 0; j < bm.Height; j++)
                    {
                        Color c = bm.GetPixel(i, j);
                        byte br = (byte)(c.R * 0.3 + c.G * 0.59 + c.B * 0.11);
                        nbm.SetPixel(i, j, Color.FromArgb(255, br, br, br));
                    }
                }
                GreyPicture.Image = nbm;
            }
        }

        public byte[] DiagramCreation(int n, byte max, byte min)
        {
            byte[] diag = new byte[(max - min + 1)];
            double dx = (max - min) / n;
            double dy = 255 / dx;
            int cx = 0;
            for (int i = 0; i < n; i++)
            {
                for (int j = cx; j < ((i + 1) * dx); j++)
                {
                    diag[j] = (byte)((j - cx) * dy);
                }
                cx = (int)(cx + dx);
            }
            int k = diag.Length - 1;
            while(diag[k] == 0)
            {
                diag[k] = 255;
                k--;
            }
            return diag;
        }
        private void ThresholdingPictureCreation(object sender, EventArgs args)
        {
            Bitmap bm = (Bitmap)GreyPicture.Image;
            if (bm != null)
            {
                byte min = bm.GetPixel(0, 0).R;
                byte max = min;
                for (int x = 0; x < bm.Width; x++)
                {
                    for (int y = 0; y < bm.Height; y++)
                    {
                        byte c = bm.GetPixel(x, y).R;
                        if (max < c) max = c;
                        else if (min > c) min = c;
                    }
                }
                byte[] diag = DiagramCreation(nCount, max, min);

                Bitmap nbm = new Bitmap(bm.Width, bm.Height);
                for (int i = 0; i < bm.Width; i++)
                {
                    for (int j = 0; j < bm.Height; j++)
                    {
                        Color c = bm.GetPixel(i, j);
                        byte br = (byte)(c.R);
                        nbm.SetPixel(i, j, Color.FromArgb(255, diag[br-min], diag[br - min], diag[br - min]));
                    }
                }
                ThresPicture.Image = nbm;
            }
        }

        /*private void DrawDiag(byte[] diag)
        {
            Bitmap bm = new Bitmap(diag.Length, diag.Length);
            for (int i = 0; i < bm.Width; i++)
            {
                for (int j = 0; j < bm.Height; j++)
                {
                    bm.SetPixel(i, j, Color.FromArgb(255, 255, 255, 255));
                }
            }
            for (int i = 0; i < diag.Length; i++)
            {
                bm.SetPixel(i, 255 - diag[i], Color.FromArgb(255, 0, 0, 0));
            }
            //diagPicture.Image = bm;
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
        */
    }
}

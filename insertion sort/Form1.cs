using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace insertion_sort
{
    public partial class Form1 : Form
    {
        private List<int> numbers = new List<int>();
        private int delay = 1100;
        float progress = 0;

        public Form1()
        {
            InitializeComponent();
            this.BackColor = Color.Black;
        }

        private void Draw(int pos1 = -1, int pos2 = -1, int from = -1, int to = -1, float progress=0)
        {

            Bitmap bmp = new Bitmap(panel1.Width, panel1.Height);
            Graphics g = Graphics.FromImage(bmp);

            g.Clear(Color.Black);

            if (numbers.Count == 0)
            {
                panel1.BackgroundImage = bmp;
                return;
            }

            int boxW = (panel1.Width - 20) / numbers.Count;
            if (boxW > 50)
            {
                boxW = 50;
            }

            int gap = 5;
            int startX = 10;
            int startY = 100;

            for (int i = 0; i < numbers.Count; i++)
            {
                int x = startX + i * (boxW + gap);
                int y = startY;
                if (from != -1 && to != -1 && progress > 0)
                {
                    int targetXFrom = startX + to * (boxW + gap);
                    int targetXTo = startX + from * (boxW + gap);

                    int bumpHeight = 30;
                    int liftValue;

                    if (progress < 0.5)
                    {
                        liftValue = (int)(bumpHeight * (progress * 2));
                    }
                    else
                    {
                        liftValue = (int)(bumpHeight * ((1 - progress) * 2));
                    }

                    int lift = liftValue;

                    if (i == from)
                    {
                        int diff = targetXFrom - x;
                        int move = (int)(diff * progress);
                        x = x + move;
                        y = startY - lift;
                    }

                    else if (i == to)
                    {
                        int diff = targetXTo - x;
                        int move = (int)(diff * progress);
                        x = x + move;
                        y = startY - lift;
                    }
                }

                Color boxColor = Color.LightBlue;

                if (i == pos1)
                {
                    boxColor = Color.Yellow;
                }

                if (i == pos2)
                {
                    boxColor = Color.Red;
                }

                if (i < pos1 && pos1 != -1)
                {
                    boxColor = Color.LightGreen;
                }

                SolidBrush brush = new SolidBrush(boxColor);
                g.FillRectangle(brush, x, y, boxW, 50);
                g.DrawRectangle(Pens.Black, x, y, boxW, 50);

                string text = numbers[i].ToString();
                Font font = new Font("Arial", 12);
                SizeF size = g.MeasureString(text, font);

                g.DrawString(text, font, Brushes.Black,
                    x + (boxW - size.Width) / 2,
                    y + 15
                );


            }

            panel1.BackgroundImage = bmp;
           // panel1.Refresh();
        }

        private async Task Swap(int from, int to)
        {
            int steps = 20;
            int frameDelay = delay / steps;

            for (int step = 0; step <= steps; step++)
            {
                float progress = (float)step / (float)steps;
                Draw(-1, -1, from, to, progress);
                await Task.Delay(frameDelay);
            }

            int temp = numbers[from];
            numbers[from] = numbers[to];
            numbers[to] = temp;
        }

        private async Task Sort()
        {
            for (int i = 1; i < numbers.Count; i++)
            {
                int key = numbers[i];
                int j = i - 1;

                Draw(i, i);
                await Task.Delay(delay);

                while (j >= 0 && numbers[j] > key)
                {
                    await Swap(j + 1, j);

                    Draw(i, j);
                    await Task.Delay(delay / 2);

                    j--;
                }

                Draw(i, -1);
                await Task.Delay(delay);
            }

            Draw(numbers.Count, -1);
        }

        private void button1_Click_1(object sender, EventArgs e)
        {

            numbers.Clear();
            string[] parts = textBox1.Text.Split(',');


            for (int i = 0; i < parts.Length; i++)
            {
                numbers.Add(int.Parse(parts[i]));
            }


            if (numbers.Count == 0)
            {
                MessageBox.Show("Enter some numbers");
                return;
            }

            Draw();


        }

        private async void button2_Click_1(object sender, EventArgs e)
        {
            if (numbers.Count == 0)
            {
                MessageBox.Show("please load numbers ");

            }

            await Sort();
        }

        private void button3_Click_1(object sender, EventArgs e)
        {
            numbers.Clear();
            panel1.BackgroundImage = null;
        }
    }
}
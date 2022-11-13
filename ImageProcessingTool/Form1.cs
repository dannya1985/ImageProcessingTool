using Emgu.CV;
using Emgu.CV.Structure;
using System;
using System.Drawing;
using System.Windows.Forms;
using System.Windows.Media;

namespace ImageProcessingTool
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();

            textBox1.Text = @"C:\Users\dodgy\source\repos\Utils\ImageProcessingTool\ImageProcessingTool\source_images\Plex-almost_fullscreen-png.png";
            textBox2.Text = @"C:\Users\dodgy\source\repos\Utils\ImageProcessingTool\ImageProcessingTool\reference_images\Plex - Watchlist Icon.png";
            pictureBox1.SizeMode = PictureBoxSizeMode.StretchImage;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Image<Bgr, byte> source = new Image<Bgr, byte>(textBox1.Text); // Image B
            Image<Bgr, byte> template = new Image<Bgr, byte>(textBox2.Text); // Image A
            Image<Bgr, byte> imageToShow = source.Copy();

            using (Image<Gray, float> result = source.MatchTemplate(template, Emgu.CV.CvEnum.TemplateMatchingType.CcoeffNormed))
            {
                double[] minValues, maxValues;
                Point[] minLocations, maxLocations;
                result.MinMax(out minValues, out maxValues, out minLocations, out maxLocations);

                // You can try different values of the threshold. I guess somewhere between 0.75 and 0.95 would be good.
                if (maxValues[0] > 0.9)
                {
                    // This is a match. Do something with it, for example draw a rectangle around it.
                    Rectangle match = new Rectangle(maxLocations[0], template.Size);
                    imageToShow.Draw(match, new Bgr(System.Drawing.Color.Red), 3);

                    resultLabel.Text = "Match found: match value - " + maxValues[0].ToString() + "\r\n" + "Location Info: \r\n" + "X: " + maxLocations[0].X + "\r\n" + "Y: " + maxLocations[0].Y;
                }
            }
            imageToShow.Save("Result.jpg");
            pictureBox1.Image = new Bitmap("Result.jpg");
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void textBox2_TextChanged(object sender, EventArgs e)
        {

        }

        private void textBox1_MouseClick(object sender, MouseEventArgs e)
        {
            OpenFileDialog dialog = new OpenFileDialog();
            var res = dialog.ShowDialog();
            if(res == DialogResult.OK)
            {
                textBox1.Text = dialog.FileName;
            }
        }

        private void textBox2_MouseClick(object sender, MouseEventArgs e)
        {
            OpenFileDialog dialog = new OpenFileDialog();
            var res = dialog.ShowDialog();
            if (res == DialogResult.OK)
            {
                textBox2.Text = dialog.FileName;
            }
        }
    }
}

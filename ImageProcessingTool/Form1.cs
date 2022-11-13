using Emgu.CV;
using Emgu.CV.Structure;
using ScreenShotUtil;
using System;
using System.Drawing;
using System.IO;
using System.Windows.Forms;
using System.Windows.Media.Imaging;

namespace ImageProcessingTool
{
    public partial class Form1 : Form
    {
        private Bitmap bmp;

        public Form1()
        {
            InitializeComponent();

            textBox1.Text = @"C:\Users\dodgy\source\repos\Utils\ImageProcessingTool\ImageProcessingTool\source_images\Plex-almost_fullscreen-png.png";
            textBox2.Text = @"C:\Users\dodgy\source\repos\Utils\ImageProcessingTool\ImageProcessingTool\reference_images\Plex - Watchlist Icon.png";

            //squishify the image if it's huge and wont fit on the form
            pictureBox1.SizeMode = PictureBoxSizeMode.StretchImage;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            MatchTemplateToSourceImage();
        }

        private void MatchTemplateToSourceImage()
        {

            Image<Bgr, byte> source;
            if (bmp != null)
            {
                //convert the bitmap to the Image format needed by the library
                source = BitmapExtension.ToImage<Bgr, byte>(bmp);
                textBox1.Text = "Screenshot in memory";
            }
            else
            {
                source = new Image<Bgr, byte>(textBox1.Text); //source image
            }

            Image<Bgr, byte> template = new Image<Bgr, byte>(textBox2.Text); //template image
            Image<Bgr, byte> imageToShow = source.Copy(); //copy the source to the resultant image for markup

            using (Image<Gray, float> result = source.MatchTemplate(template, Emgu.CV.CvEnum.TemplateMatchingType.CcoeffNormed))
            {
                double[] minValues, maxValues;
                System.Drawing.Point[] minLocations, maxLocations;
                result.MinMax(out minValues, out maxValues, out minLocations, out maxLocations);

                // You can try different values of the threshold. I guess somewhere between 0.75 and 0.95 would be good.
                if (maxValues[0] > 0.9)
                {
                    // This is a match. Do something with it, for example draw a rectangle around it.
                    Rectangle match = new Rectangle(maxLocations[0], template.Size);
                    imageToShow.Draw(match, new Bgr(System.Drawing.Color.Red), 3);

                    resultLabel.Text = "Great match found: match value - " + maxValues[0].ToString() + "\r\n"
                        + "Location Info: \r\n"
                        + "X: " + maxLocations[0].X + "\r\n"
                        + "Y: " + maxLocations[0].Y;
                }
                else if (maxValues[0] > 0.75)
                {
                    // This is a match. Do something with it, for example draw a rectangle around it.
                    Rectangle match = new Rectangle(maxLocations[0], template.Size);
                    imageToShow.Draw(match, new Bgr(System.Drawing.Color.Red), 3);

                    resultLabel.Text = "Good match found: match value - " + maxValues[0].ToString() + "\r\n"
                        + "Location Info: \r\n"
                        + "X: " + maxLocations[0].X + "\r\n"
                        + "Y: " + maxLocations[0].Y;
                }
                else if (maxValues[0] > 0.5)
                {
                    // This is a match. Do something with it, for example draw a rectangle around it.
                    Rectangle match = new Rectangle(maxLocations[0], template.Size);
                    imageToShow.Draw(match, new Bgr(System.Drawing.Color.Red), 3);

                    resultLabel.Text = "Weak match found: match value - " + maxValues[0].ToString() + "\r\n"
                        + "Location Info: \r\n"
                        + "X: " + maxLocations[0].X + "\r\n"
                        + "Y: " + maxLocations[0].Y;
                }
                else
                {
                    resultLabel.Text = "No match found: match value - " + maxValues[0].ToString();
                    pictureBox1.Image = null;
                }
            }

            var filename = "Result-" + DateTime.Now.Month + DateTime.Now.Day + DateTime.Now.Year + "_" + DateTime.Now.Hour + DateTime.Now.Minute + DateTime.Now.Second + ".jpg";

            //save the result match image
            imageToShow.Save(filename);

            //update the form to show the squished version if it's larger than the form
            pictureBox1.Image = new Bitmap(filename);
            bmp = null;
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
            dialog.InitialDirectory = System.Reflection.Assembly.GetExecutingAssembly().Location;
            var res = dialog.ShowDialog();
            if(res == DialogResult.OK)
            {
                textBox1.Text = dialog.FileName;
            }
        }

        private void textBox2_MouseClick(object sender, MouseEventArgs e)
        {
            OpenFileDialog dialog = new OpenFileDialog();
            dialog.InitialDirectory = System.Reflection.Assembly.GetExecutingAssembly().Location;
            var res = dialog.ShowDialog();
            if (res == DialogResult.OK)
            {
                textBox2.Text = dialog.FileName;
            }
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            //handle keypresses
            this.KeyUp += new System.Windows.Forms.KeyEventHandler(KeyEvent);
            this.KeyPreview = true;
        }

        private void KeyEvent(object sender, KeyEventArgs e) //Keyup Event 
        {
            if (e.KeyCode == Keys.F9)
            {
                //take ascreenshot and process it for matches on the selected template
                bmp = (Bitmap)ScreenCapture.CaptureDesktop();
                MatchTemplateToSourceImage();
            }
            if (e.KeyCode == Keys.F6)
            {
                //do nothing for now
            }
        }
    }
}

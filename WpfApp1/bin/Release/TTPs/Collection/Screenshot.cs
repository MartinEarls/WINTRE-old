using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Windows.Forms;
namespace TechniqueDebugging
{
    class Program
    {
        public static void CaptureRegion()
        {

            Rectangle bounds = Screen.PrimaryScreen.Bounds;
			
            using (Bitmap bitmap = new Bitmap(bounds.Width, bounds.Height))
            {
                using (Graphics g = Graphics.FromImage(bitmap))
                {
                    g.CopyFromScreen(0, 0, 0, 0, bounds.Size);
                }
				
                bitmap.Save(".\\Outputs\\Collection\\test.png", ImageFormat.Png);
            }
        }

        public static void Main(string[] args)
        {
            CaptureRegion();
        }
    }
}

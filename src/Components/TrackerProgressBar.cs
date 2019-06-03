using System.Drawing;
using System.Windows.Forms;

namespace PoeRankingTracker.Components
{
    public partial class TrackerProgressBar : ProgressBar
    {
        private Brush brush = new SolidBrush(Color.Black);

        public TrackerProgressBar()
        {
            this.SetStyle(ControlStyles.UserPaint, true);
            this.SetAutoSizeMode(AutoSizeMode.GrowAndShrink);
        }

        public void SetColor(Color color)
        {
            brush = new SolidBrush(color);
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            Rectangle rec = e.ClipRectangle;
            var width = (int)(rec.Width * ((double)Value / Maximum)) - 4;
            var height = rec.Height - 4;
            e.Graphics.FillRectangle(brush, 2, 2, width, height);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                components?.Dispose();
                brush?.Dispose();
            }

            base.Dispose(disposing);
        }
    }
}

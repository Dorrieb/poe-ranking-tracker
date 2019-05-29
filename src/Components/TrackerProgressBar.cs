using System.Drawing;
using System.Windows.Forms;

namespace PoeRankingTracker.Components
{
    public partial class TrackerProgressBar : ProgressBar
    {
        private Color color = Color.Black;
        private Brush brush;

        public TrackerProgressBar()
        {
            this.SetStyle(ControlStyles.UserPaint, true);
            this.SetAutoSizeMode(AutoSizeMode.GrowAndShrink);
            brush = new SolidBrush(color);
        }

        public void SetColor(Color color)
        {
            this.color = color;
            brush = new SolidBrush(color);
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            Rectangle rec = e.ClipRectangle;

            rec.Width = (int)(rec.Width * ((double)Value / Maximum)) - 4;
            if (ProgressBarRenderer.IsSupported)
                ProgressBarRenderer.DrawHorizontalBar(e.Graphics, e.ClipRectangle);
            rec.Height -= 4;
            e.Graphics.FillRectangle(brush, 2, 2, rec.Width, rec.Height);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                components?.Dispose();
                brush.Dispose();
            }

            base.Dispose(disposing);
        }
    }
}

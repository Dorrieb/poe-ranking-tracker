using System;
using System.Diagnostics.Contracts;
using System.Drawing;
using System.Globalization;
using System.Runtime.InteropServices;
using System.Threading;

namespace PoeRankingTracker.Services
{
    public interface IFormService
    {
        void SetCulture(string language);
        Icon ResizeIcon(Icon icon, Size iconSize);
        bool DestroyIcon(Icon icon);
    }

    public class FormService : IFormService
    {
        public void SetCulture(string language)
        {
            var culture = CultureInfo.GetCultureInfo(language);
            Thread.CurrentThread.CurrentUICulture = culture;
            CultureInfo.CurrentCulture = culture;
        }

        public Icon ResizeIcon(Icon icon, Size iconSize)
        {
            Contract.Requires(icon != null);

            using (Bitmap bitmap = new Bitmap(iconSize.Width, iconSize.Height))
            {
                using (Graphics g = Graphics.FromImage(bitmap))
                {
                    g.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.HighQualityBicubic;
                    g.DrawImage(icon.ToBitmap(), new Rectangle(Point.Empty, iconSize));
                }

                return Icon.FromHandle(bitmap.GetHicon());
            }
        }

        public bool DestroyIcon(Icon icon)
        {
            Contract.Requires(icon != null);

            return SafeNativeMethods.DestroyIcon(icon.Handle);
        }
    }

    internal static class SafeNativeMethods
    {
        [System.Runtime.InteropServices.DllImport("user32.dll", CharSet = CharSet.Auto)]
        internal static extern bool DestroyIcon(IntPtr handle);
    }
}

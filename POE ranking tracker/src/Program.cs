using System;
using System.Windows.Forms;

namespace PoeRankingTracker
{
    static class Program
    {
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            using (var context = new RankingTrackerContext())
            {
                Application.Run(context);
            }
        }
    }
}

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
            Application.Run(new RankingTrackerContext());
        }
    }
}

using Castle.Windsor;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using PoeRankingTracker.Installers;
using PoeRankingTracker.Services;
using POEToolsTestsBase;
using System.Drawing;
using System.Globalization;
using System.Threading;
using System.Windows.Forms;

namespace PoeRankingTrackerTests.Services
{
    [TestClass]
    public class FormsServiceTest : BaseUnitTest
    {
        private IFormService formsService;

        [TestInitialize]
        public void TestSetup()
        {
            using (IWindsorContainer container = new WindsorContainer())
            {
                container.Install(new ServicesInstaller());
                formsService = container.Resolve<IFormService>();
            }
        }

        [TestMethod]
        public void SetCulture()
        {
            var fr = "fr";
            var en = "en";
            formsService.SetCulture(fr);
            var cultureFr = CultureInfo.CurrentCulture;
            Assert.AreEqual(fr, cultureFr.TwoLetterISOLanguageName);
            Assert.AreEqual(fr, Thread.CurrentThread.CurrentUICulture.TwoLetterISOLanguageName);
            formsService.SetCulture(en);
            var cultureEn = CultureInfo.CurrentCulture;
            Assert.AreEqual(en, cultureEn.TwoLetterISOLanguageName);
            Assert.AreEqual(en, Thread.CurrentThread.CurrentUICulture.TwoLetterISOLanguageName);
        }

        [TestMethod]
        public void ResizeIconSmall()
        {
            var icon = formsService.ResizeIcon(SystemIcons.Exclamation, SystemInformation.SmallIconSize);
            Assert.AreEqual(16, icon.Size.Width);
            Assert.AreEqual(16, icon.Size.Height);
        }

        [TestMethod]
        public void DestroyIcon()
        {
            var icon = formsService.ResizeIcon(SystemIcons.Exclamation, SystemInformation.SmallIconSize);
            var result = formsService.DestroyIcon(icon);
            Assert.IsTrue(result);
        }
    }
}

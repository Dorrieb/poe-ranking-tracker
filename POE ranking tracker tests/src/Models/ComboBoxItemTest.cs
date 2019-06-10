using Microsoft.VisualStudio.TestTools.UnitTesting;
using PoeRankingTracker.Models;

namespace PoeRankingTrackerTests.Models
{
    [TestClass]
    public class ComboBoxItemTest
    {
        private ComboBoxItem item;
        private readonly string text = "test";
        private readonly object value = 12;

        [TestInitialize]
        public void TestSetup()
        {
            item = new ComboBoxItem
            {
                Text = text,
                Value = value,
            };
        }

        [TestMethod]
        public void Get()
        {
            Assert.AreEqual(text, item.Text);
            Assert.AreEqual(value, item.Value);
        }

        [TestMethod]
        public void Set()
        {
            string text2 = $"{text}2";
            object value2 = "test2";
            item.Text = text2;
            item.Value = value2;
            Assert.AreEqual(text2, item.Text);
            Assert.AreEqual(value2, item.Value);
        }

        [TestMethod]
        public void StringTransform()
        {
            Assert.AreEqual(text, item.ToString());
        }
    }
}
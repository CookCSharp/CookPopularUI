using CookPopularUI.WPF.Tools;

namespace CookPopularUI.WPF.UnitTest
{
    public class MessageDialogHelperTest
    {
        [SetUp]
        public void Setup()
        {
        }

        [Test]
        public void Test1()
        {
            var d = MessageDialogHelper.ShowFileError(@"C:\Users\46535\Desktop\12.json");
        }
    }
}
namespace Tests
{
    using NUnit.Framework;

    using steamLauncher.Logic.Helpers;

    [TestFixture]
    public class EncryptionTests
    {
        [Test]
        public void TestCorrectEncryptionCycle()
        {
            var originalText = "readable string";
            var password = "12345678";

            var encrypted = EncryptionHelper.Encrypt(password, originalText);
            Assert.AreEqual("ZOMscBDLBYJ79h4SkJfBKA==", encrypted);

            var decrypted = EncryptionHelper.Decrypt(password, encrypted);
            Assert.AreEqual(originalText, decrypted);
        }
    }
}
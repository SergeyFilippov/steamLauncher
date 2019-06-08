using NUnit.Framework;

namespace Tests
{
    public class DummyProcessLauncherTests
    {
        [SetUp]
        public void Setup()
        {
        }

        [Test]
        public void CheckWorkingScenario()
        {
            var program = steamLauncher.DummyProcessLauncher.Program.Main(new[] { "notepad.exe", "c:\\temp.txt" });
            Assert.AreEqual(program, (int)steamLauncher.DummyProcessLauncher.Program.ExitCodes.Ok);
        }

        [Test]
        public void CheckNullScenario()
        {
            var program = steamLauncher.DummyProcessLauncher.Program.Main(null);
            Assert.AreEqual(program, (int)steamLauncher.DummyProcessLauncher.Program.ExitCodes.ArgsNull);
        }

        [Test]
        public void CheckArgNumberScenario()
        {
            var program = steamLauncher.DummyProcessLauncher.Program.Main(new[] { string.Empty });
            Assert.AreEqual(program, (int)steamLauncher.DummyProcessLauncher.Program.ExitCodes.ArgsNumber);

            program = steamLauncher.DummyProcessLauncher.Program.Main(new[] { "test.exe", "arg1", "arg2" });
            Assert.AreEqual(program, (int)steamLauncher.DummyProcessLauncher.Program.ExitCodes.ArgsNumber);
        }

        [Test]
        public void CheckProcessNameScenario()
        {
            var program = steamLauncher.DummyProcessLauncher.Program.Main(new[] { "notepad%^&*().exe", "c:\\temp.txt" });
            Assert.AreEqual(program, (int)steamLauncher.DummyProcessLauncher.Program.ExitCodes.GenericError);
        }
    }
}
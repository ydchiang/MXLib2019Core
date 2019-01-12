using System;
using System.IO;
using System.Reflection;
using MXLib2019Core.LogHelper;
using NUnit.Framework;

namespace MXLib2019CoreTest.LogHelper
{
    [TestFixture]
    public class MXLogHelperTest
    {
        private string _logTestFile = null;
        private string _logResetTestFile = null;
        [SetUp]
        public void Init()
        {
            string codeBase = Assembly.GetExecutingAssembly().CodeBase;
            UriBuilder uri = new UriBuilder(codeBase);
            string path = Path.GetDirectoryName(Uri.UnescapeDataString(uri.Path));
            _logTestFile = path + @"/LogTest.log";
            _logResetTestFile = path + @"/ResetLogTest.log";
        }

        [Test]
        [Parallelizable(ParallelScope.None)]
        public void CreateFileLoggerTest()
        {
            MXLogHelper.Reset();
            MXLogHelper.CreateFileLogger(_logTestFile, true);
            MXLogHelper.Info("NLogUtils Unit Test.");
            Assert.IsTrue(MXLogHelper.IsReady);
            Assert.IsTrue(File.Exists(_logTestFile));
        }

        [Test]
        [Parallelizable(ParallelScope.None)]
        public void CreateFileLoggerResetTest()
        {
            MXLogHelper.Reset();
            MXLogHelper.CreateFileLogger(_logResetTestFile);
            MXLogHelper.Info("NLogUtils Reset Unit Test.");
            Assert.IsTrue(MXLogHelper.IsReady);
            Assert.IsTrue(File.Exists(_logResetTestFile));
        }

        [Test]
        [Parallelizable(ParallelScope.None)]
        public void CreateConsoleLoggerTest()
        {
            MXLogHelper.Reset();
            MXLogHelper.CreateConsoleLogger();
            Assert.IsTrue(MXLogHelper.IsReady);
            Assert.DoesNotThrow(() => MXLogHelper.Info("NLogUtils unit test info."));
            Assert.DoesNotThrow(() => MXLogHelper.Info("NLogUtils unit test info.", new Exception("Dummy exception")));
            Assert.DoesNotThrow(() => MXLogHelper.Warn("NLogUtils unit test warn."));
            Assert.DoesNotThrow(() => MXLogHelper.Warn("NLogUtils unit test warn.", new Exception("Dummy exception")));
            Assert.DoesNotThrow(() => MXLogHelper.Error("NLogUtils unit test error."));
            Assert.DoesNotThrow(() => MXLogHelper.Error("NLogUtils unit test error.", new Exception("Dummy exception")));
        }

        [Test]
        [Parallelizable(ParallelScope.None)]
        public void CreateDefaultLoggerTest()
        {
            MXLogHelper.Reset();
            MXLogHelper.CreateDefaultLogger();
            Assert.IsTrue(MXLogHelper.IsReady);
        }

        [TearDown]
        public void TearDown()
        {
            File.Delete(_logTestFile);
            File.Delete(_logResetTestFile);
        }
    }
}

using System.IO;
using NUnit.Framework;
using VacationTests.Infrastructure;

namespace VacationTests.Infrastructure
{
    public interface ILog
    {
        void Info(string message);
    }
    
    public class MyLog : ILog
    {
        public void Info(string message)
        {
            TestContext.Out.WriteLine($"{TestContext.CurrentContext.WorkerId} {message}");
            // File.WriteAllText(TestContext.CurrentContext.Test.Name, message);
        }
    }
}


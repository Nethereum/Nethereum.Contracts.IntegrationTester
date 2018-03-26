using System.Diagnostics;

namespace Nethereum.Contracts.IntegrationTester
{
    public class DebugWriter : ISimpleWriter
    {
        public void WriteLine(string message)
        {
            Debug.WriteLine(message);
        }
    }
}
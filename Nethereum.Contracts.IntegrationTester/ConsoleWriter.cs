using System;

namespace Nethereum.Contracts.IntegrationTester
{
    public class ConsoleWriter:ISimpleWriter
    {
        public void WriteLine(string message)
        {
            Console.WriteLine(message);
        }
    }
}
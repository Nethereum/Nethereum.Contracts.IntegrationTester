using Xunit.Abstractions;

namespace Nethereum.Contracts.IntegrationTester
{
    public class XunitOutputWriter : ISimpleWriter
    {
        private readonly ITestOutputHelper output;

        public XunitOutputWriter(ITestOutputHelper output)
        {
            this.output = output;
        }

        public void WriteLine(string message)
        {
            output.WriteLine(message);
        }

    }
}
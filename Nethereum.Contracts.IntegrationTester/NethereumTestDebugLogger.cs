using Nethereum.Contracts.CQS;
using StatePrinting;

namespace Nethereum.Contracts.IntegrationTester
{
    public class NethereumTestDebugLogger: INethereumTestLogger
    {
        private readonly ISimpleWriter _writer;

        public NethereumTestDebugLogger(ISimpleWriter writer)
        {
            _writer = writer;
            Stateprinter = new Stateprinter();
        }

        protected Stateprinter Stateprinter { get; }

        public void LogGivenContractDeployment(ContractDeploymentMessage contractDeploymentMessage)
        {
            _writer.WriteLine("Given I Deploy Contract:");
            LogContractDeployment(contractDeploymentMessage);
        }

        public void LogContractDeployment(ContractDeploymentMessage contractDeploymentMessage)
        {
            _writer.WriteLine(Stateprinter.PrintObject(contractDeploymentMessage));
        }

        public void LogWhenQueryFunctionThen(FunctionMessage queryFunction, object outputDTO)
        {
           LogWhenQueryFunction(queryFunction);
           LogExpectedQueryResult(outputDTO);
        }

        public void LogExpectedQueryResult(object outputDTO)
        {
            _writer.WriteLine("Then the Expected  query result is:");
            _writer.WriteLine(Stateprinter.PrintObject(outputDTO));
        }

        public void LogWhenQueryFunction(FunctionMessage queryFunction)
        {
            _writer.WriteLine("When Querying Function:");
            _writer.WriteLine(Stateprinter.PrintObject(queryFunction));
        }

        public void LogGivenSendTransaction(FunctionMessage transactionMessage)
        {
            _writer.WriteLine("Given I Send Tranasction");
            _writer.WriteLine(Stateprinter.PrintObject(transactionMessage));
        }

        public void LogExpectedEvent(object expectedEvent)
        {
            _writer.WriteLine("Then the expected event is:");
            _writer.WriteLine(Stateprinter.PrintObject(expectedEvent));
        }
    }
}
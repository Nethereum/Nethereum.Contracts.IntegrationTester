using Nethereum.Contracts.ContractHandlers;
using Nethereum.Contracts.CQS;
using StatePrinting;

namespace Nethereum.Contracts.IntegrationTester
{
    public class QueryResult<TFunctionOuputDTO>
        where TFunctionOuputDTO : new()
    {
        public ContractHandler ContractHandler { get; }
        public TFunctionOuputDTO Result { get; }
        public INethereumTestLogger TestLogger { get; }
        public Stateprinter Stateprinter { get; }

        public QueryResult(ContractHandler contractHandler, TFunctionOuputDTO result, INethereumTestLogger testLogger, Stateprinter stateprinter)
        {
            ContractHandler = contractHandler;
            Result = result;
            TestLogger = testLogger;
            Stateprinter = stateprinter;
        }

        public QueryResult<TFunctionOuputDTO> ThenExpectResult(TFunctionOuputDTO expectedResult) 
        {
            TestLogger.LogExpectedQueryResult(expectedResult);
            Stateprinter.Assert.AreEqual(
                Stateprinter.PrintObject(expectedResult),
                Stateprinter.PrintObject(Result));
            return this;
        }
    }
}
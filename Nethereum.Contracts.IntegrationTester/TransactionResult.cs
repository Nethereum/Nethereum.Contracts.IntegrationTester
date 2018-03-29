using System.Linq;
using Nethereum.Contracts.CQS;
using Nethereum.RPC.Eth.DTOs;
using StatePrinting;
using Xunit;

namespace Nethereum.Contracts.IntegrationTester
{
    public class TransactionResult
    {
        public ContractHandler ContractHandler { get; }
        public TransactionReceipt TransactionReceipt { get; }
        public INethereumTestLogger TestLogger { get; }
        public Stateprinter Stateprinter { get; }

        public TransactionResult(ContractHandler contractHandler, TransactionReceipt transactionReceipt, INethereumTestLogger testLogger, Stateprinter stateprinter)
        {
            ContractHandler = contractHandler;
            TransactionReceipt = transactionReceipt;
            TestLogger = testLogger;
            Stateprinter = stateprinter;
        }

        public TransactionResult ThenExpectAnEvent<TEventDTO>(TEventDTO expectedEvent) where TEventDTO : new()
        {
            TestLogger.LogExpectedEvent(expectedEvent);

            var eventItem = ContractHandler.GetEvent<TEventDTO>();
            var eventFirst = eventItem.DecodeAllEventsForEvent<TEventDTO>(TransactionReceipt.Logs).FirstOrDefault();
            Assert.NotNull(eventFirst);

            Stateprinter.Assert.AreEqual(
                Stateprinter.PrintObject(expectedEvent),
                Stateprinter.PrintObject(eventFirst.Event));

            return this;
        }
    }
}
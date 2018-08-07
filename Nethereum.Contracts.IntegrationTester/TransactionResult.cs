using System.Linq;
using Nethereum.ABI.FunctionEncoding.Attributes;
using Nethereum.Contracts.ContractHandlers;
using Nethereum.Contracts.CQS;
using Nethereum.Contracts.Extensions;
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

        public TransactionResult ThenExpectAnEvent<TEventDTO>(TEventDTO expectedEvent) where TEventDTO : IEventDTO, new()
        {
            TestLogger.LogExpectedEvent(expectedEvent);

            var events = TransactionReceipt.DecodeAllEvents<TEventDTO>();
           
            var eventFirst = events.FirstOrDefault();
            Assert.NotNull(eventFirst);

            Stateprinter.Assert.AreEqual(
                Stateprinter.PrintObject(expectedEvent),
                Stateprinter.PrintObject(eventFirst.Event));

            return this;
        }
    }
}
using System.Linq;
using System.Threading.Tasks;
using Nethereum.Contracts.CQS;
using Nethereum.RPC.Eth.DTOs;
using Nethereum.Web3.Accounts;
using StatePrinting;
using Xunit;

namespace Nethereum.Contracts.IntegrationTester
{
    public class NethereumIntegrationTest
    {
        public ContractHandler ContractHandler { get; set; }
        public string Url { get;}

        public Web3.Web3 Web3 { get; }

        protected Stateprinter Stateprinter { get; }

        protected INethereumTestLogger TestLogger { get; }

        protected NethereumIntegrationTest(string url, string privateKey, INethereumTestLogger testLogger)
        {
            this.Stateprinter = new Stateprinter();
            this.Stateprinter.Configuration.Test.SetAreEqualsMethod((s, expected, message) =>
                Xunit.Assert.Equal(s, expected));
            this.Web3 = new Web3.Web3(new Account(privateKey), url);
            this.TestLogger = testLogger;
        }

        public NethereumIntegrationTest() : this("http://localhost:8545", DefaultTestAccountConstants.PrivateKey, new NethereumTestDebugLogger(new DebugWriter()))
        {
            
        }

        protected async Task WhenQueryingThen<TQueryFunction, TOuputDTO>() where TQueryFunction: ContractMessage, new()
            where TOuputDTO: new()
        {
            var message = new TQueryFunction();
            var expected = new TOuputDTO();
            await WhenQueryingThen<TQueryFunction, TOuputDTO>(message, expected);
        }


        protected async Task WhenQueryingThen<TQueryFunction, TOuputDTO>(TOuputDTO expectedOutput) where TQueryFunction : ContractMessage, new()
            where TOuputDTO : new()
        {
            var queryFunction = new TQueryFunction();
            await WhenQueryingThen<TQueryFunction, TOuputDTO>(queryFunction, expectedOutput);
        }

        protected async Task WhenQueryingThen<TQueryFunction, TOuputDTO>(TQueryFunction queryFunction, TOuputDTO expectedOutput) where TQueryFunction : ContractMessage, new()
            where TOuputDTO : new()
        {
            TestLogger.LogWhenQueryFunctionThen(queryFunction, expectedOutput);
            var result = await ContractHandler.QueryDeserializingToObjectAsync<TQueryFunction, TOuputDTO>(queryFunction);
            Stateprinter.Assert.AreEqual(
                Stateprinter.PrintObject(expectedOutput),
                Stateprinter.PrintObject(result));
        }

        protected async Task<TransactionReceipt> GivenSendTransaction<TTransactionMessage>(TTransactionMessage transactionMessage) where TTransactionMessage : ContractMessage, new()
        {
            TestLogger.LogGivenSendTransaction(transactionMessage);
            return await ContractHandler.SendRequestAndWaitForReceiptAsync<TTransactionMessage>(transactionMessage);
        }

        protected async Task<TransactionReceipt> GivenSendTransactionThenEvent<TTransactionMessage, TEventDTO>(TTransactionMessage transactionMessage, TEventDTO expectedEvent) where TTransactionMessage : ContractMessage, new()
            where TEventDTO: new()
        {
            var receipt = await GivenSendTransaction(transactionMessage);
            ThenEventFirst(expectedEvent, receipt);
            return receipt;
        }

        protected async Task<TransactionReceipt> GivenIDeployContract<TDeploymentMessage>() where TDeploymentMessage : ContractDeploymentMessage, new()
        {
            var message = new TDeploymentMessage();
            return await GivenIDeployContract(message);
        }

        protected async Task<TransactionReceipt> GivenIDeployContract<TDeploymentMessage>(TDeploymentMessage contractDeploymentMessage) where TDeploymentMessage : ContractDeploymentMessage, new()
        {
            TestLogger.LogGivenContractDeployment(contractDeploymentMessage);
            var deploymentHadler = Web3.Eth.GetContractDeploymentHandler<TDeploymentMessage>();
            var receipt = await deploymentHadler.SendRequestAndWaitForReceiptAsync(contractDeploymentMessage);
            ContractHandler = Web3.Eth.GetContractHandler(receipt.ContractAddress);
            return receipt;
        }

        protected void ThenEventFirst<TEventDTO>(TEventDTO expectedEvent, TransactionReceipt transactionReceipt) where TEventDTO: new()
        {
            TestLogger.LogExpectedEvent(expectedEvent);

            var eventItem = ContractHandler.GetEvent<TEventDTO>();
            var eventFirst = eventItem.DecodeAllEventsForEvent<TEventDTO>(transactionReceipt.Logs).FirstOrDefault();
            Assert.NotNull(eventFirst);

            Stateprinter.Assert.AreEqual(
                Stateprinter.PrintObject(expectedEvent),
                Stateprinter.PrintObject(eventFirst.Event));
        }
    }
}
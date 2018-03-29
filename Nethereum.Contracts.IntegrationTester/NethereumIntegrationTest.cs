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

        protected QueryResult<TOuputDTO> WhenQueryingThen<TQueryFunction, TOuputDTO>() where TQueryFunction: ContractMessage, new()
            where TOuputDTO: new()
        {
            var message = new TQueryFunction();
            var expected = new TOuputDTO();
            return WhenQueryingThen<TQueryFunction, TOuputDTO>(message, expected);
        }


        protected QueryResult<TOuputDTO> WhenQueryingThen<TQueryFunction, TOuputDTO>(TOuputDTO expectedOutput) where TQueryFunction : ContractMessage, new()
            where TOuputDTO : new()
        {
            var queryFunction = new TQueryFunction();
            return WhenQueryingThen<TQueryFunction, TOuputDTO>(queryFunction, expectedOutput);
        }


        protected QueryResult<TOuputDTO> WhenQuerying<TQueryFunction, TOuputDTO>() where TQueryFunction : ContractMessage, new()
            where TOuputDTO : new()
        {
            var queryFunction = new TQueryFunction();
            return WhenQuerying<TQueryFunction,TOuputDTO>(queryFunction);
        }

        protected QueryResult<TOuputDTO> WhenQuerying<TQueryFunction, TOuputDTO>(TQueryFunction queryFunction) where TQueryFunction : ContractMessage, new()
            where TOuputDTO : new()
        {
            TestLogger.LogWhenQueryFunction(queryFunction);
            var result = ContractHandler.QueryDeserializingToObjectAsync<TQueryFunction, TOuputDTO>(queryFunction).Result;
            return new QueryResult<TOuputDTO>(ContractHandler, result, TestLogger, Stateprinter);
        }

        protected QueryResult<TOuputDTO> WhenQueryingThen<TQueryFunction, TOuputDTO>(TQueryFunction queryFunction, TOuputDTO expectedOutput) where TQueryFunction : ContractMessage, new()
            where TOuputDTO : new()
        {
            TestLogger.LogWhenQueryFunctionThen(queryFunction, expectedOutput);
            var result = ContractHandler.QueryDeserializingToObjectAsync<TQueryFunction, TOuputDTO>(queryFunction).Result;
            Stateprinter.Assert.AreEqual(
                Stateprinter.PrintObject(expectedOutput),
                Stateprinter.PrintObject(result));
            return new QueryResult<TOuputDTO>(ContractHandler, result, TestLogger, Stateprinter);
        }

        protected TransactionResult GivenATransaction<TTransactionMessage>(TTransactionMessage transactionMessage) where TTransactionMessage : ContractMessage, new()
        {
            TestLogger.LogGivenSendTransaction(transactionMessage);
            var transactionReceipt = ContractHandler.SendRequestAndWaitForReceiptAsync<TTransactionMessage>(transactionMessage).Result;
            return new TransactionResult(ContractHandler, transactionReceipt, TestLogger, Stateprinter);
        }


        protected TransactionResult GivenADeployedContract<TDeploymentMessage>() where TDeploymentMessage : ContractDeploymentMessage, new()
        {
            var message = new TDeploymentMessage();
            return GivenADeployedContract(message);
        }

        protected TransactionResult GivenADeployedContract<TDeploymentMessage>(TDeploymentMessage contractDeploymentMessage) where TDeploymentMessage : ContractDeploymentMessage, new()
        {
            TestLogger.LogGivenContractDeployment(contractDeploymentMessage);
            var deploymentHadler = Web3.Eth.GetContractDeploymentHandler<TDeploymentMessage>();
            var receipt =  deploymentHadler.SendRequestAndWaitForReceiptAsync(contractDeploymentMessage).Result;
            ContractHandler = Web3.Eth.GetContractHandler(receipt.ContractAddress);
            return new TransactionResult(ContractHandler, receipt, TestLogger, Stateprinter);
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
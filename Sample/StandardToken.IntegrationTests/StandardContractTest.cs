using System.Threading.Tasks;
using Nethereum.Contracts.IntegrationTester;
using StandardToken.IntegrationTests.Scenarios;
using StandardToken.MyContractName.CQS;
using StandardToken.MyContractName.DTOs;
using Xunit;
using Xunit.Abstractions;

namespace StandardToken.IntegrationTests
{

    public class StandardContractTest: NethereumIntegrationTest
    {
        public StandardContractTest(ITestOutputHelper xunitTestOutputHelper) : base("http://localhost:8545",
            DefaultTestAccountConstants.PrivateKey, new NethereumTestDebugLogger(new XunitOutputWriter(xunitTestOutputHelper)))
        {
            
        }

        [Theory]
        [InlineData(10000)]
        [InlineData(5000)]
        [InlineData(300)]
        public async Task AfterDeployment_BalanceOwner_ShouldBeTheSameAsInitialSupply(int initialSupply)
        {
            var contractDeploymentDefault = DefaultScenario.GetDeploymentMessage();
            contractDeploymentDefault.InitialAmount = initialSupply;

            await GivenIDeployContract(contractDeploymentDefault);

            var balanceOfResult = new BalanceOfOutputDTO() { Balance = initialSupply };

            await WhenQueryingThen(DefaultScenario.GetBalanceOfOwnerMessage(), balanceOfResult);
        }

        [Theory]
        [InlineData(10000)]
        [InlineData(5000)]
        [InlineData(300)]
        public async Task Transfering_ShouldIncreaseTheBalanceOfReceiver(int valueToSend)
        {
            var contractDeploymentDefault = DefaultScenario.GetDeploymentMessage();
            
            Assert.False(valueToSend > contractDeploymentDefault.InitialAmount, "value to send is bigger than the total supply, please adjust the test data");

            await GivenIDeployContract(contractDeploymentDefault);

            var receiver = DefaultScenario.ReceiverAddress;

            var transferMessage = new TransferFunction()
            {
                Value = valueToSend,
                FromAddress = DefaultTestAccountConstants.Address,
                To = receiver,
            };

            var expectedEvent = new TransferEventDTO()
            {
                From = DefaultTestAccountConstants.Address.ToLower(), //lower case return from geth
                To = DefaultScenario.ReceiverAddress.ToLower(),
                Value = valueToSend
            };

            await GivenSendTransactionThenEvent(transferMessage, expectedEvent);

            var queryBalanceReciverMessage = new BalanceOfFunction() {Owner = DefaultScenario.ReceiverAddress};
            var expectedOutput = new BalanceOfOutputDTO() {Balance = valueToSend};

            await WhenQueryingThen(queryBalanceReciverMessage, expectedOutput);
        }
    }
}

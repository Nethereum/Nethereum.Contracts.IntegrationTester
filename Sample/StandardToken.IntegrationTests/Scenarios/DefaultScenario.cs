using Nethereum.Contracts.IntegrationTester;
using StandardToken.MyContractName.CQS;

namespace StandardToken.IntegrationTests.Scenarios
{
    public class DefaultScenario
    {
        public static string ReceiverAddress = "0x31230d2cce102216644c59daE5baed380d84830c";

        public static MyContractName.CQS.StandardTokenDeployment GetDeploymentMessage()
        {
            return new MyContractName.CQS.StandardTokenDeployment()
            {
                InitialAmount = 10000000,
                TokenName = "TST",
                TokenSymbol = "TST",
                DecimalUnits = 18,
                FromAddress = DefaultTestAccountConstants.Address
            };
        }

        public static BalanceOfFunction GetBalanceOfOwnerMessage()
        {
            return new BalanceOfFunction()
            {
                Owner = DefaultTestAccountConstants.Address
            };
        }
        
    }
}
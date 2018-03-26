using Nethereum.Contracts.CQS;

namespace Nethereum.Contracts.IntegrationTester
{
    public interface INethereumTestLogger
    {
        void LogGivenContractDeployment(ContractDeploymentMessage contractDeploymentMessage);
        void LogContractDeployment(ContractDeploymentMessage contractDeploymentMessage);
        void LogWhenQueryFunctionThen(ContractMessage contractMessage, object outputDTO);
        void LogGivenSendTransaction(ContractMessage transactionMessage);
        void LogExpectedEvent(object expectedEvent);
    }
}
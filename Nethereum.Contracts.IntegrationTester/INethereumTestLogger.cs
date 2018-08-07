using Nethereum.Contracts.CQS;
using Nethereum.Contracts;

namespace Nethereum.Contracts.IntegrationTester
{
    public interface INethereumTestLogger
    {
        void LogGivenContractDeployment(ContractDeploymentMessage contractDeploymentMessage);
        void LogContractDeployment(ContractDeploymentMessage contractDeploymentMessage);
        void LogWhenQueryFunctionThen(FunctionMessage functionMessage, object outputDTO);
        void LogGivenSendTransaction(FunctionMessage transactionMessage);
        void LogExpectedEvent(object expectedEvent);
        void LogExpectedQueryResult(object expectedResult);
        void LogWhenQueryFunction(FunctionMessage queryFunction);
    }
}
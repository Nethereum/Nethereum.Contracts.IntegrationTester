using System;
using System.Threading.Tasks;
using System.Numerics;
using Nethereum.Hex.HexTypes;
using Nethereum.ABI.FunctionEncoding.Attributes;
using Nethereum.Contracts.CQS;
using StandardToken.MyContractName.DTOs;
namespace StandardToken.MyContractName.CQS
{
    [Function("balances", "uint256")]
    public class BalancesFunction:ContractMessage
    {
        [Parameter("address", "", 1)]
        public string B {get; set;}
    }
}

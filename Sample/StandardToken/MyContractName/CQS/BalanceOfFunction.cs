using System;
using System.Threading.Tasks;
using System.Numerics;
using Nethereum.Hex.HexTypes;
using Nethereum.ABI.FunctionEncoding.Attributes;
using Nethereum.Contracts;
using Nethereum.Contracts.CQS;
using StandardToken.MyContractName.DTOs;
namespace StandardToken.MyContractName.CQS
{
    [Function("balanceOf", typeof(BalanceOfOutputDTO))]
    public class BalanceOfFunction:FunctionMessage
    {
        [Parameter("address", "_owner", 1)]
        public string Owner {get; set;}
    }
}

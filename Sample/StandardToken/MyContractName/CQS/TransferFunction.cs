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
    [Function("transfer", "bool")]
    public class TransferFunction:FunctionMessage
    {
        [Parameter("address", "_to", 1)]
        public string To {get; set;}
        [Parameter("uint256", "_value", 2)]
        public BigInteger Value {get; set;}
    }
}

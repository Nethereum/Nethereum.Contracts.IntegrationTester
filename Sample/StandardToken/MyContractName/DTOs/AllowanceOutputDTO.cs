using System;
using System.Threading.Tasks;
using System.Numerics;
using Nethereum.Hex.HexTypes;
using Nethereum.ABI.FunctionEncoding.Attributes;
namespace StandardToken.MyContractName.DTOs
{
    [FunctionOutput]
    public class AllowanceOutputDTO: IFunctionOutputDTO
    {
        [Parameter("uint256", "remaining", 1)]
        public BigInteger Remaining {get; set;}
    }
}

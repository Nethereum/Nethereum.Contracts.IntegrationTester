# Nethereum Integration Tester

The Nethereum Integration Tester provides a simple way to test your smart contracts, message input, outputs and events using a BDD style.
The aim is to provide a way to black box test complex requirements driven by data messages.

## Test sample

Having a simple default setup scenario as follows:

```csharp
public class DefaultScenario
    {
        public static string ReceiverAddress = "0x31230d2cce102216644c59daE5baed380d84830c";

        public static StanadardTokenDeployment GetDeploymentMessage()
        {
            return new StanadardTokenDeployment()
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
```

In which we setup a default deployment for a standard token smart contract, and a simple message to query the balance of the ownner.
We can create a simple XUnit Theory as follows:

```csharp
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
```

We can see here two helper methods:

* **GiveIDeployContract** : This method deploys a contract using the message provided and log the input.
* **WhenQueryingThen**: This method first makes a contract query and assert the results based on a provided expectation.

## Testing Output Example:

The xunit ouput of the test, can help us visualised the data and ensure the requirements are matched.

```
Given I Deploy Contract:
new MyContractNameDeployment()
{
    AmountToSend = new BigInteger()
    {
        _sign = 0
        _bits = null
    }
    Gas = null
    GasPrice = null
    FromAddress = "0x12890D2cce102216644c59daE5baed380d84830c"
    ByteCode = "6060604052341561000f57600080fd5b6040516107ae3803806107ae833981016040528080519190602001805182019190602001805191906020018051600160a060020a03331660009081526001602052604081208790558690559091019050600383805161007292916020019061009f565b506004805460ff191660ff8416179055600581805161009592916020019061009f565b505050505061013a565b828054600181600116156101000203166002900490600052602060002090601f016020900481019282601f106100e057805160ff191683800117855561010d565b8280016001018555821561010d579182015b8281111561010d5782518255916020019190600101906100f2565b5061011992915061011d565b5090565b61013791905b808211156101195760008155600101610123565b90565b610665806101496000396000f3006060604052600436106100ae5763ffffffff7c010000000000000000000000000000000000000000000000000000000060003504166306fdde0381146100b3578063095ea7b31461013d57806318160ddd1461017357806323b872dd1461019857806327e235e3146101c0578063313ce567146101df5780635c6581651461020857806370a082311461022d57806395d89b411461024c578063a9059cbb1461025f578063dd62ed3e14610281575b600080fd5b34156100be57600080fd5b6100c66102a6565b60405160208082528190810183818151815260200191508051906020019080838360005b838110156101025780820151838201526020016100ea565b50505050905090810190601f16801561012f5780820380516001836020036101000a031916815260200191505b509250505060405180910390f35b341561014857600080fd5b61015f600160a060020a0360043516602435610344565b604051901515815260200160405180910390f35b341561017e57600080fd5b6101866103b0565b60405190815260200160405180910390f35b34156101a357600080fd5b61015f600160a060020a03600435811690602435166044356103b6565b34156101cb57600080fd5b610186600160a060020a03600435166104bc565b34156101ea57600080fd5b6101f26104ce565b60405160ff909116815260200160405180910390f35b341561021357600080fd5b610186600160a060020a03600435811690602435166104d7565b341561023857600080fd5b610186600160a060020a03600435166104f4565b341561025757600080fd5b6100c661050f565b341561026a57600080fd5b61015f600160a060020a036004351660243561057a565b341561028c57600080fd5b610186600160a060020a036004358116906024351661060e565b60038054600181600116156101000203166002900480601f01602080910402602001604051908101604052809291908181526020018280546001816001161561010002031660029004801561033c5780601f106103115761010080835404028352916020019161033c565b820191906000526020600020905b81548152906001019060200180831161031f57829003601f168201915b505050505081565b600160a060020a03338116600081815260026020908152604080832094871680845294909152808220859055909291907f8c5be1e5ebec7d5bd14f71427d1e84f3dd0314c0f7b2291e5b200ac8c7c3b9259085905190815260200160405180910390a350600192915050565b60005481565b600160a060020a0380841660008181526002602090815260408083203390951683529381528382205492825260019052918220548390108015906103fa5750828110155b151561040557600080fd5b600160a060020a038085166000908152600160205260408082208054870190559187168152208054849003905560001981101561046a57600160a060020a03808616600090815260026020908152604080832033909416835292905220805484900390555b83600160a060020a031685600160a060020a03167fddf252ad1be2c89b69c2b068fc378daa952ba7f163c4a11628f55a4df523b3ef8560405190815260200160405180910390a3506001949350505050565b60016020526000908152604090205481565b60045460ff1681565b600260209081526000928352604080842090915290825290205481565b600160a060020a031660009081526001602052604090205490565b60058054600181600116156101000203166002900480601f01602080910402602001604051908101604052809291908181526020018280546001816001161561010002031660029004801561033c5780601f106103115761010080835404028352916020019161033c565b600160a060020a033316600090815260016020526040812054829010156105a057600080fd5b600160a060020a033381166000818152600160205260408082208054879003905592861680825290839020805486019055917fddf252ad1be2c89b69c2b068fc378daa952ba7f163c4a11628f55a4df523b3ef9085905190815260200160405180910390a350600192915050565b600160a060020a039182166000908152600260209081526040808320939094168252919091522054905600a165627a7a723058201145b253e40a502d8bd264f98d66de641dec0c9e4a25e35eaba523821e0fb6ad0029"
    InitialAmount = new BigInteger()
    {
        _sign = 10000
        _bits = null
    }
    TokenName = "TST"
    DecimalUnits = 18
    TokenSymbol = "TST"
}
When Querying Function:
new BalanceOfFunction()
{
    AmountToSend = new BigInteger()
    {
        _sign = 0
        _bits = null
    }
    Gas = null
    GasPrice = null
    FromAddress = null
    Owner = "0x12890D2cce102216644c59daE5baed380d84830c"
}
Then the Expected result is:
new BalanceOfOutputDTO()
{
    Balance = new BigInteger()
    {
        _sign = 10000
        _bits = null
    }
}

```

## Transactions and Events

Another common situation is the testing of Transactions and Events, this example demonstrates how to validate the Event output of a transaction.

```csharp
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

```




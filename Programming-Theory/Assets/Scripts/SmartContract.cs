using System;
using System.Numerics;
using UnityEngine;
using Nethereum.Web3;
using Nethereum.Contracts;
using Nethereum.Hex.HexTypes;
using Nethereum.ABI.FunctionEncoding.Attributes;
using System.Collections.Generic;

public class SmartContractCaller : MonoBehaviour
{
    // Ethereum network URL (Infura or another provider)
    private string url = "https://sepolia.infura.io/v3/1cbe7b7bfc1241ff801c647dbeb52815";

    // The smart contract address
    private string contractAddress = "0xa8123fF26EEaF45C3C684b2d3ADF4Ec1FEab21Ef";

    // The ABI for the smart contract
    private string contractABI = "[{\"inputs\":[{\"internalType\":\"string\",\"name\":\"_item\",\"type\":\"string\"}],\"name\":\"buyItem\",\"outputs\":[],\"stateMutability\":\"nonpayable\",\"type\":\"function\"},{\"inputs\":[],\"name\":\"buyTokens\",\"outputs\":[],\"stateMutability\":\"payable\",\"type\":\"function\"},{\"inputs\":[{\"internalType\":\"uint16\",\"name\":\"_i\",\"type\":\"uint16\"}],\"name\":\"sellItem\",\"outputs\":[],\"stateMutability\":\"nonpayable\",\"type\":\"function\"},{\"inputs\":[{\"internalType\":\"uint256\",\"name\":\"_tokens\",\"type\":\"uint256\"}],\"name\":\"sellTokens\",\"outputs\":[],\"stateMutability\":\"nonpayable\",\"type\":\"function\"},{\"inputs\":[{\"internalType\":\"address\",\"name\":\"receiver\",\"type\":\"address\"},{\"internalType\":\"uint16\",\"name\":\"itemIndex\",\"type\":\"uint16\"}],\"name\":\"transferItem\",\"outputs\":[],\"stateMutability\":\"nonpayable\",\"type\":\"function\"},{\"inputs\":[{\"internalType\":\"address\",\"name\":\"receiver\",\"type\":\"address\"},{\"internalType\":\"uint256\",\"name\":\"_tokens\",\"type\":\"uint256\"}],\"name\":\"transferTokens\",\"outputs\":[],\"stateMutability\":\"nonpayable\",\"type\":\"function\"},{\"stateMutability\":\"payable\",\"type\":\"receive\"},{\"inputs\":[{\"internalType\":\"uint256\",\"name\":\"amount\",\"type\":\"uint256\"}],\"name\":\"withdrawEther\",\"outputs\":[],\"stateMutability\":\"nonpayable\",\"type\":\"function\"},{\"inputs\":[{\"internalType\":\"string[]\",\"name\":\"items\",\"type\":\"string[]\"},{\"internalType\":\"uint256[]\",\"name\":\"price\",\"type\":\"uint256[]\"},{\"internalType\":\"uint16\",\"name\":\"len\",\"type\":\"uint16\"}],\"stateMutability\":\"nonpayable\",\"type\":\"constructor\"},{\"inputs\":[],\"name\":\"deployer\",\"outputs\":[{\"internalType\":\"address\",\"name\":\"\",\"type\":\"address\"}],\"stateMutability\":\"view\",\"type\":\"function\"},{\"inputs\":[{\"internalType\":\"address\",\"name\":\"person\",\"type\":\"address\"}],\"name\":\"getDetails\",\"outputs\":[{\"components\":[{\"internalType\":\"uint256\",\"name\":\"tokens\",\"type\":\"uint256\"},{\"internalType\":\"string[]\",\"name\":\"items\",\"type\":\"string[]\"}],\"internalType\":\"struct GAME.Details\",\"name\":\"\",\"type\":\"tuple\"}],\"stateMutability\":\"view\",\"type\":\"function\"}]\r\n";

    // Web3 instance
    private Web3 web3;

    private void Start()
    {
        // Initialize the Web3 instance
        web3 = new Web3(url);

        // Example calls (comment/uncomment based on use case)
        //CallBuyItem("Sword");
        //CallBuyTokens(1.0m);
        //CallSellItem(1);
        //CallSellTokens(10);
        //CallTransferItem("0xRecipientAddress", 2);
        //CallTransferTokens("0xRecipientAddress", 100);
        //CallWithdrawEther(1);
        CallGetDetails("0x6cD1a34C//a09D85Cd232Db511FAa8d098f5");
        //CallDeployer();
    }

    // -------------------------
    // Function Implementations:
    // -------------------------

    // 1. Call buyItem(string _item)
    private async void CallBuyItem(string item)
    {
        var contract = web3.Eth.GetContract(contractABI, contractAddress);
        var function = contract.GetFunction("buyItem");
        try
        {
            var receipt = await function.SendTransactionAsync("YOUR_WALLET_ADDRESS", new HexBigInteger(0), item);
            Debug.Log("buyItem transaction receipt: " + receipt);
        }
        catch (Exception e)
        {
            Debug.LogError("Error calling buyItem: " + e.Message);
        }
    }

    // 2. Call buyTokens() with Ether (payable)
    private async void CallBuyTokens(decimal etherAmount)
    {
        var contract = web3.Eth.GetContract(contractABI, contractAddress);
        var function = contract.GetFunction("buyTokens");
        var amount = Web3.Convert.ToWei(etherAmount);  // Convert Ether to Wei
        try
        {
            var receipt = await function.SendTransactionAsync("YOUR_WALLET_ADDRESS", new HexBigInteger(amount));
            Debug.Log("buyTokens transaction receipt: " + receipt);
        }
        catch (Exception e)
        {
            Debug.LogError("Error calling buyTokens: " + e.Message);
        }
    }

    // 3. Call sellItem(uint16 _i)
    private async void CallSellItem(ushort itemIndex)
    {
        var contract = web3.Eth.GetContract(contractABI, contractAddress);
        var function = contract.GetFunction("sellItem");
        try
        {
            var receipt = await function.SendTransactionAsync("YOUR_WALLET_ADDRESS", new HexBigInteger(0), itemIndex);
            Debug.Log("sellItem transaction receipt: " + receipt);
        }
        catch (Exception e)
        {
            Debug.LogError("Error calling sellItem: " + e.Message);
        }
    }

    // 4. Call sellTokens(uint256 _tokens)
    private async void CallSellTokens(BigInteger tokens)
    {
        var contract = web3.Eth.GetContract(contractABI, contractAddress);
        var function = contract.GetFunction("sellTokens");
        try
        {
            var receipt = await function.SendTransactionAsync("YOUR_WALLET_ADDRESS", new HexBigInteger(0), tokens);
            Debug.Log("sellTokens transaction receipt: " + receipt);
        }
        catch (Exception e)
        {
            Debug.LogError("Error calling sellTokens: " + e.Message);
        }
    }

    // 5. Call transferItem(address receiver, uint16 itemIndex)
    private async void CallTransferItem(string receiver, ushort itemIndex)
    {
        var contract = web3.Eth.GetContract(contractABI, contractAddress);
        var function = contract.GetFunction("transferItem");
        try
        {
            var receipt = await function.SendTransactionAsync("YOUR_WALLET_ADDRESS", new HexBigInteger(0), receiver, itemIndex);
            Debug.Log("transferItem transaction receipt: " + receipt);
        }
        catch (Exception e)
        {
            Debug.LogError("Error calling transferItem: " + e.Message);
        }
    }

    // 6. Call transferTokens(address receiver, uint256 _tokens)
    private async void CallTransferTokens(string receiver, BigInteger tokens)
    {
        var contract = web3.Eth.GetContract(contractABI, contractAddress);
        var function = contract.GetFunction("transferTokens");
        try
        {
            var receipt = await function.SendTransactionAsync("YOUR_WALLET_ADDRESS", new HexBigInteger(0), receiver, tokens);
            Debug.Log("transferTokens transaction receipt: " + receipt);
        }
        catch (Exception e)
        {
            Debug.LogError("Error calling transferTokens: " + e.Message);
        }
    }

    // 7. Call withdrawEther(uint256 amount)
    private async void CallWithdrawEther(BigInteger amount)
    {
        var contract = web3.Eth.GetContract(contractABI, contractAddress);
        var function = contract.GetFunction("withdrawEther");
        try
        {
            var receipt = await function.SendTransactionAsync("YOUR_WALLET_ADDRESS", new HexBigInteger(0), amount);
            Debug.Log("withdrawEther transaction receipt: " + receipt);
        }
        catch (Exception e)
        {
            Debug.LogError("Error calling withdrawEther: " + e.Message);
        }
    }

    // 8. Call getDetails(address person) (view function)
    private async void CallGetDetails(string personAddress)
    {
        var contract = web3.Eth.GetContract(contractABI, contractAddress);
        var function = contract.GetFunction("getDetails");
        try
        {
            // Call the function and deserialize the result to the 'Details' class
            var details = await function.CallDeserializingToObjectAsync<Details>(personAddress);

            // Access the fields
            Debug.Log("Tokens: " + details.Tokens);

            if (details.Items != null && details.Items.Count > 0)
            {
                Debug.Log("Items: " + string.Join(", ", details.Items));
            }
            else
            {
                Debug.Log("Items: No items owned");
            }
        }
        catch (Exception e)
        {
            Debug.LogError("Error calling getDetails: " + e.Message);
        }
    }






    // 9. Call deployer() (view function)
    private async void CallDeployer()
    {
        var contract = web3.Eth.GetContract(contractABI, contractAddress);
        var function = contract.GetFunction("deployer");
        try
        {
            string deployerAddress = await function.CallAsync<string>();
            Debug.Log("Deployer Address: " + deployerAddress);
        }
        catch (Exception e)
        {
            Debug.LogError("Error calling deployer: " + e.Message);
        }
    }

    // Struct to match the return type of getDetails

    [FunctionOutput]
    public class Details
    {
    public BigInteger Tokens { get; set; }
    public List<string> Items { get; set; }
    }



}

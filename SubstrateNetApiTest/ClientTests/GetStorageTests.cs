using System;
using System.Threading.Tasks;
using NLog;
using NLog.Config;
using NLog.Targets;
using NUnit.Framework;
using SubstrateNetApi;
using SubstrateNetApi.Exceptions;
using SubstrateNetApi.Model.Types.Base;
using SubstrateNetApi.Model.Types.Struct;

namespace SubstrateNetApiTests.ClientTests
{
    public class GetStorageTests
    {
        private const string WebSocketUrl = "wss://rpc.polkadot.io";

        private SubstrateClient _substrateClient;

        [OneTimeSetUp]
        public void Setup()
        {
            var config = new LoggingConfiguration();

            // Targets where to log to: File and Console
            var logconsole = new ConsoleTarget("logconsole");

            // Rules for mapping loggers to targets            
            config.AddRule(LogLevel.Debug, LogLevel.Fatal, logconsole);

            // Apply config           
            LogManager.Configuration = config;

            _substrateClient = new SubstrateClient(new Uri(WebSocketUrl));
        }

        [OneTimeTearDown]
        public void TearDown()
        {
            _substrateClient.Dispose();
        }

        [Test]
        public async Task BasicStorageTestNoParameterAsync1()
        {
            await _substrateClient.ConnectAsync();

            var reqResult = await _substrateClient.GetStorageAsync("System", "Number");
            Assert.AreEqual("BlockNumber", reqResult.GetType().Name);
            Assert.IsTrue(reqResult is BlockNumber);

            await _substrateClient.CloseAsync();
        }

        [Test]
        public async Task BasicStorageTestWithParameterAsync1()
        {
            await _substrateClient.ConnectAsync();
            
            var reqResult = await _substrateClient.GetStorageAsync("System", "Account", new string[] { Utils.Bytes2HexString(Utils.GetPublicKeyFrom("13RDY9nrJpyTDBSUdBw12dGwhk19sGwsrVZ2bxkzYHBSagP2")) });
            Assert.AreEqual("AccountInfo", reqResult.GetType().Name);
            Assert.IsTrue(reqResult is AccountInfo);

            await _substrateClient.CloseAsync();
        }

        [Test]
        public async Task InvalidStorageNameTestAsync()
        {
            await _substrateClient.ConnectAsync();

            Assert.ThrowsAsync<MissingModuleOrItemException>(async () =>
                await _substrateClient.GetStorageAsync("Invalid", "Name"));

            await _substrateClient.CloseAsync();
        }

        [Test]
        public void InvalidConnectionStateTest()
        {
            Assert.ThrowsAsync<ClientNotConnectedException>(async () =>
                await _substrateClient.GetStorageAsync("Invalid", "Name"));
        }

        [Test]
        public async Task MissingTypeConverterTestAsync()
        {
            await _substrateClient.ConnectAsync();

            Assert.ThrowsAsync<MissingConverterException>(async () =>
                await _substrateClient.GetStorageAsync("Timestamp", "Now"));

            await _substrateClient.CloseAsync();
        }
    }
}
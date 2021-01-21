using System;
using PWARemake.Lib.Protocol;
using Xunit;

namespace PWARemake.Tests
{
    public class ApiTest
    {
        [Fact]
        public void TestSerializeClientRpcOk()
        {
            var api = new PWARemake.Lib.Protocol.Api();
            Assert.Equal(
                "{\"GameCmd\":{\"cmd\":{\"PlayerMove\":{\"current_pos\":null,\"next_pos\":null,\"use_time\":0,\"speed\":0,\"move_mode\":0,\"stamp\":0}}}}",
                api.Serialize(new CmdPlayerMove())
            );
        }
    }
}

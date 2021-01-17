using System;
using Xunit;
using PWARemake.Protocol;

namespace PWARemake.Tests
{
    public class ApiTest
    {
        [Fact]
        public void TestSerializeClientRpcOk()
        {
            var api = new PWARemake.Protocol.Api();
            Assert.Equal(
                "{\"GameCmd\":{\"cmd\":{\"CmdPlayerMove\":{\"current_pos\":null,\"next_pos\":null,\"use_time\":0,\"speed\":0,\"move_mode\":0,\"stamp\":0}}}}",
                api.Serialize(new CmdPlayerMove().IntoRpc2())
            );
        }
    }
}

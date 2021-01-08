using System;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json.Serialization;

class RootClassContractResolver : DefaultContractResolver
{
    protected override JsonConverter ResolveContractConverter(Type objectType)
    {
        if (typeof(ApiResponse.Root).IsAssignableFrom(objectType) && !objectType.IsAbstract)
            return null; // pretend TableSortRuleConvert is not specified (thus avoiding a stack overflow)
        return base.ResolveContractConverter(objectType);
    }
}

class RootConverter : JsonConverter
{
    static JsonSerializerSettings SpecifiedSubclassConversion = new JsonSerializerSettings() { ContractResolver = new RootClassContractResolver() };

    public override bool CanConvert(Type objectType)
    {
        return (objectType == typeof(ApiResponse.Root));
    }

    public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
    {
        JObject jo = JObject.Load(reader);
        switch (jo.First.Path)
        {
            case "Single":
                return JsonConvert.DeserializeObject<ApiResponse.Single>(jo.First.First.ToString(), SpecifiedSubclassConversion);
            case "Container":
                return JsonConvert.DeserializeObject<ApiResponse.Container>(jo.First.First.ToString());
            default:
                throw new Exception();
        }
        throw new NotImplementedException();
    }

    public override bool CanWrite
    {
        get { return false; }
    }

    public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
    {
        throw new NotImplementedException(); // won't be called because CanWrite returns false
    }
}

class SingleClassContractResolver : DefaultContractResolver
{
    protected override JsonConverter ResolveContractConverter(Type objectType)
    {
        if (typeof(ApiResponse.Single).IsAssignableFrom(objectType) && !objectType.IsAbstract)
            return null; // pretend TableSortRuleConvert is not specified (thus avoiding a stack overflow)
        return base.ResolveContractConverter(objectType);
    }
}

class SingleConverter : JsonConverter
{
    static JsonSerializerSettings SpecifiedSubclassConversion = new JsonSerializerSettings() { ContractResolver = new SingleClassContractResolver() };

    public override bool CanConvert(Type objectType)
    {
        return (objectType == typeof(ApiResponse.Single));
    }

    public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
    {
        JObject jo = JObject.Load(reader);
        switch (jo.First.Path)
        {
            case "Unknown":
                return JsonConvert.DeserializeObject<ApiResponse.UnknownSingle>(jo.First.First.ToString());
            case "PrivateChat":
                return JsonConvert.DeserializeObject<ApiResponse.PrivateChatSingle>(jo.First.First.ToString(), SpecifiedSubclassConversion);
            case "ChatMessage":
                return JsonConvert.DeserializeObject<ApiResponse.ChatMessageSingle>(jo.First.First.ToString(), SpecifiedSubclassConversion);
            case "GameData":
                return JsonConvert.DeserializeObject<ApiResponse.GameDataSingle>(jo.ToString());
            default:
                throw new Exception($"unknown Single {jo.First.Path}");
        }
        throw new NotImplementedException();
    }

    public override bool CanWrite
    {
        get { return false; }
    }

    public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
    {
        throw new NotImplementedException(); // won't be called because CanWrite returns false
    }
}

class UnknownSingleConverter : JsonConverter
{
    public override bool CanConvert(Type objectType)
    {
        return (objectType == typeof(ApiResponse.UnknownSingle));
    }

    public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
    {
        var jo = JArray.Load(reader);
        var id = jo.First.Value<int>();
        var octetStream = jo.Last.Value<string>();
        return new ApiResponse.UnknownSingle(id, octetStream);
    }

    public override bool CanWrite
    {
        get { return false; }
    }

    public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
    {
        throw new NotImplementedException(); // won't be called because CanWrite returns false
    }
}


class ContainerConverter : JsonConverter
{
    public override bool CanConvert(Type objectType)
    {
        return (objectType == typeof(ApiResponse.Container));
    }

    public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
    {
        var jo = JArray.Load(reader);
        var id = jo.First.Value<short>();
        var packets = JsonConvert.DeserializeObject<List<ApiResponse.Single>>(jo.Last.ToString());
        return new ApiResponse.Container(id, packets);
    }

    public override bool CanWrite
    {
        get { return false; }
    }

    public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
    {
        throw new NotImplementedException(); // won't be called because CanWrite returns false
    }
}


class GameDataClassContractResolver : DefaultContractResolver
{
    protected override JsonConverter ResolveContractConverter(Type objectType)
    {
        if (typeof(ApiResponse.GameCmd).IsAssignableFrom(objectType) && !objectType.IsAbstract)
            return null; // pretend TableSortRuleConvert is not specified (thus avoiding a stack overflow)
        return base.ResolveContractConverter(objectType);
    }
}

class GameDataConverter : JsonConverter
{
    static JsonSerializerSettings SpecifiedSubclassConversion = new JsonSerializerSettings() { ContractResolver = new GameDataClassContractResolver() };
    public override bool CanConvert(Type objectType)
    {
        return (objectType == typeof(ApiResponse.GameDataSingle));
    }

    public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
    {
        var jo = JObject.Load(reader).First.First.First.First.First;
        var cmd = jo.Path.Replace("GameData.cmd.", "");
        ApiResponse.GameCmd gameCmd;
        switch (cmd)
        {
            case "ObjectMove":
                gameCmd = JsonConvert.DeserializeObject<ApiResponse.ObjectMove>(jo.First.ToString(), SpecifiedSubclassConversion);
                break;
            case "ObjectStopMove":
                gameCmd = JsonConvert.DeserializeObject<ApiResponse.ObjectStopMove>(jo.First.ToString(), SpecifiedSubclassConversion);
                break;
            case "OwnExtProp":
                gameCmd = JsonConvert.DeserializeObject<ApiResponse.OwnExtProp>(jo.First.ToString(), SpecifiedSubclassConversion);
                break;
            case "SelfInfo00":
                gameCmd = JsonConvert.DeserializeObject<ApiResponse.SelfInfo00Single>(jo.First.ToString(), SpecifiedSubclassConversion);
                break;
            default:
                Console.WriteLine($"unknown GameCmd {cmd}");
                throw new Exception($"unknown GameCmd {cmd}");
        }
        return new ApiResponse.GameDataSingle { Cmd = gameCmd };
    }

    public override bool CanWrite
    {
        get { return false; }
    }

    public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
    {
        throw new NotImplementedException(); // won't be called because CanWrite returns false
    }
}


public class ApiResponse
{
    [JsonConverter(typeof(SingleConverter))]
    public abstract class Single : Root
    {
        public abstract object IntoRpc();
    }


    [JsonConverter(typeof(ContainerConverter))]
    public class Container : Root
    {
        public short Id { get; private set; }
        public List<Single> Packets { get; private set; }

        public Container(short id, List<Single> packets)
        {
            Id = id;
            Packets = packets;
        }
    }


    [JsonConverter(typeof(UnknownSingleConverter))]
    public sealed class UnknownSingle : Single
    {
        public int Id { get; internal set; }
        public string OctetStream { get; internal set; }

        public UnknownSingle(int id, string octetStream)
        {
            Id = id;
            OctetStream = octetStream;
        }

        public override object IntoRpc()
        {
            return new UnknownRpc(Id, OctetStream);
        }
    }


    public abstract class GameCmd { }

    [JsonConverter(typeof(GameDataConverter))]
    public sealed class GameDataSingle : Single {
        public GameCmd Cmd { get; set; }

        public override object IntoRpc()
        {
            if (Cmd is SelfInfo00Single)
            {
                return new SelfInfo00();
            }
            throw new Exception($"unknown GameData {Cmd.ToString()}");
        }
    }
    public sealed class ObjectMove : GameCmd
    {
        public int id { get; set; }
        public Point3D dest { get; set; }
        public int use_time { get; set; }
        public short speed { get; set; }
        public short move_mode { get; set; }
    }
    public sealed class ObjectStopMove : GameCmd
    {
        public long id { get; set; }
        public Point3D dest { get; set; }
        public int speed { get; set; }
        public int dir { get; set; }
        public int move_mode { get; set; }
    }
    public sealed class OwnExtProp : GameCmd {
        public int status_point { get; set; }
    }
    public sealed class SelfInfo00Single : GameCmd
    {
        public int level { get; set; }
        public int state { get; set; }
        public int level2 { get; set; }
        public int hp { get; set; }
        public int max_hp { get; set; }
        public int mp { get; set; }
        public int max_mp { get; set; }
        public int exp { get; set; }
        public int sp { get; set; }
        public int ap { get; set; }
        public int max_ap { get; set; }
    }


    public sealed class ChatMessageSingle : Single
    {
        public int channel { get; set; }
        public int emotion { get; set; }
        public long sender { get; set; }
        public string msg { get; set; }

        public override object IntoRpc()
        {
            return new ChatMessage() 
            {
                channel = channel,
                emotion = emotion,
                sender = sender,
                msg = msg
            };
        }
    }


    public sealed class PrivateChatSingle : Single
    {
        public int channel { get; set; }
        public int emotion { get; set; }
        public string src_name { get; set; }
        public long src_id { get; set; }
        public string dst_name { get; set; }
        public int dst_id { get; set; }
        public string msg { get; set; }
        public List<object> data { get; set; }
        public int src_level { get; set; }

        public override object IntoRpc()
        {
            return new PrivateChat() {
                channel = channel,
                emotion = emotion,
                src_name = src_name,
                src_id = src_id,
                dst_name = dst_name,
                dst_id = dst_id,
                msg = msg,
                data = data,
                src_level = src_level
            };
        }
    }


    public sealed class GameDataCmdResponse 
    {
        public SelfInfo00Single SelfInfo00 { get; internal set; }
        public ObjectMove ObjectMove { get; internal set; }
        public ObjectStopMove ObjectStopMove { get; internal set; }
        public OwnExtProp OwnExtProp { get; internal set; }
    }


    public sealed class GameDataResponse 
    {
        public GameDataCmdResponse Cmd { get; internal set; }
    }


    public class SingleResponse
    {
        public UnknownSingle Unknown { get; set; }
        public PrivateChatSingle PrivateChat { get; set; }
        public ChatMessageSingle ChatMessage { get; set; }
        public GameDataResponse GameData { get; set; }
    }

    public sealed class RootSimple {
        public SingleResponse Single { get; set; }
        public dynamic Container { get; set; }
    }

    [JsonConverter(typeof(RootConverter))]
    public abstract class Root { }
}

public abstract class PwRpc { }
public abstract class RpcSingle : PwRpc { }
public sealed class RpcContainer : PwRpc
{
    public short Id { get; private set; }
    public List<RpcSingle> Packets { get; private set; }

    public RpcContainer(short id, List<RpcSingle> packets)
    {
        Id = id;
        Packets = packets;
    }
}

public sealed class UnknownRpc : RpcSingle
{
    public int Id { get; internal set; }
    public string OctetStream { get; internal set; }

    public UnknownRpc(int id, string octetStream)
    {
        Id = id;
        OctetStream = octetStream;
    }
}


public sealed class ChatMessage : RpcSingle
{
    public int channel { get; internal set; }
    public int emotion { get; internal set; }
    public long sender { get; internal set; }
    public string msg { get; internal set; }
}


public sealed class PrivateChat : RpcSingle
{
    public int channel { get; internal set; }
    public int emotion { get; internal set; }
    public string src_name { get; internal set; }
    public long src_id { get; internal set; }
    public string dst_name { get; internal set; }
    public int dst_id { get; internal set; }
    public string msg { get; internal set; }
    public List<object> data { get; internal set; }
    public int src_level { get; internal set; }
}

public abstract class GameCmd { }
public sealed class SelfInfo00: GameCmd
{

}

public sealed class GameData: RpcSingle
{

}

class Api
{

    public string EnterWorld(string serverAddr, Account account)
    {
        return $"{{\"addr\": \"{serverAddr}\", \"select_role_by_index\": {account.DefaultRoleIndex}, \"account\": {{\"login\": \"{account.Login}\", \"passwd\": \"{account.Password}\"}}}}";
    }

    public PwRpc ParseEvent(string msg)
    {
        // Console.WriteLine(msg);
        var resp = JsonConvert.DeserializeObject<ApiResponse.Root>(msg);
        if (resp.Single != null) {
            Console.WriteLine("Single parsed");
        }
        if (resp.Container != null) {
            Console.WriteLine("Container parsed");
            var id = (short)resp.Container.First;
            var packets = ((JArray)resp.Container.Last).Select(x => (RpcSingle)JsonConvert.DeserializeObject<ApiResponse.Single>(x.ToString()).IntoRpc()).ToList();
            return new RpcContainer(id, packets);
        }
        return new UnknownRpc(143, "");
    }
}
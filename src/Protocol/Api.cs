using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json.Serialization;

class RootClassContractResolver : DefaultContractResolver
{
    protected override JsonConverter ResolveContractConverter(Type objectType)
    {
        if (typeof(RootConverter).IsAssignableFrom(objectType) && !objectType.IsAbstract)
            return null; // pretend TableSortRuleConvert is not specified (thus avoiding a stack overflow)
        return base.ResolveContractConverter(objectType);
    }
}

class RootConverter : JsonConverter
{
    static JsonSerializerSettings SpecifiedSubclassConversion = new JsonSerializerSettings() { ContractResolver = new RootClassContractResolver() };

    public override bool CanConvert(Type objectType)
    {
        return (objectType == typeof(ApiResponse.Single));
    }

    public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
    {
        JObject jo = JObject.Load(reader);
        switch (jo.First.Path)
        {
            case "Single":
                return JsonConvert.DeserializeObject<ApiResponse.Single>(jo.First.First.ToString(), SpecifiedSubclassConversion);
            case "Container":
                return JsonConvert.DeserializeObject<ApiResponse.Container>(jo.First.First.ToString(), SpecifiedSubclassConversion);
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
        if (typeof(Single).IsAssignableFrom(objectType) && !objectType.IsAbstract)
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
                return JsonConvert.DeserializeObject<ApiResponse.UnknownSingle>(jo.ToString(), SpecifiedSubclassConversion);
            case "PrivateChat":
                return JsonConvert.DeserializeObject<ApiResponse.PrivateChatSingle>(jo.ToString(), SpecifiedSubclassConversion);
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

class UnknownSingleConverter: JsonConverter
{
    public override bool CanConvert(Type objectType)
    {
        return (objectType == typeof(ApiResponse.UnknownSingle));
    }

    public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
    {
        JObject jo = JObject.Load(reader);
        var id = jo.First.First.First.Value<int>();
        var octetStream = jo.First.First.Last.Value<string>();
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

#region ContainerJsonConverter
class ContainerConverter : JsonConverter
{
    public override bool CanConvert(Type objectType)
    {
        return (objectType == typeof(ApiResponse.Container));
    }

    public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
    {
        JObject jo = JObject.Load(reader);
        var id = jo.First.First.First.Value<short>();
        var packets = JsonConvert.DeserializeObject<List<ApiResponse.Single>>(jo.First.First.Last.ToString());
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
#endregion

public class ApiResponse {
    [JsonConverter(typeof(SingleConverter))]
    public abstract class Single: Root {}

    [JsonConverter(typeof(ContainerConverter))]
    public class Container: Root {
        public short Id { get; private set; }
        public List<Single> Packets { get; private set; }

        public Container(short id, List<Single> packets) {
            this.Id = id;
            this.Packets = packets;
        }
    }


    [JsonConverter(typeof(UnknownSingleConverter))]
    public sealed class UnknownSingle: Single { 
        public int Id { get; internal set; }
        public string OctetStream { get; internal set; }

        public UnknownSingle(int id, string octetStream) {
            this.Id = id;
            this.OctetStream = octetStream;
        }
    }


    #region GameData

    public abstract class GameData {}
    sealed class ObjectMove: GameData {
        public int id { get; private set; }
        public Point3D dest { get; private set; }
        public int use_time { get; private set; }
        public short speed { get; private set; }
        public short move_mode { get; private set; }
    }

    #endregion


    public sealed class GameDataSingle: Single { 
        public GameData data;
        public GameDataSingle(GameData data) {
            this.data = data;
        }
    }


    public sealed class PrivateChatSingle: Single {
        public int channel { get; set; } 
        public int emotion { get; set; } 
        public string src_name { get; set; } 
        public long src_id { get; set; } 
        public string dst_name { get; set; } 
        public int dst_id { get; set; } 
        public string msg { get; set; } 
        public List<object> data { get; set; } 
        public int src_level { get; set; } 
    }


    public class SingleResponse {
        public List<UnknownSingle> Unknown { get; internal set; }
        public PrivateChatSingle PrivateChat { get; internal set; }
    }

    [JsonConverter(typeof(RootConverter))]
    public abstract class Root { }
}

public sealed class PwEvent {
    public ApiResponse.Single Single { get; internal set; }
    public ApiResponse.Container Container { get; internal set; }
}

class Api
{

    public string EnterWorld(string serverAddr, Account account) {
        return $"{{\"addr\": \"{serverAddr}\", \"select_role_by_index\": {account.DefaultRoleIndex}, \"account\": {{\"login\": \"{account.Login}\", \"passwd\": \"{account.Password}\"}}}}";
    }

    public PwEvent ParseEvent(string msg) {
        Console.WriteLine(msg);
        var resp = JsonConvert.DeserializeObject<ApiResponse.Root>(msg);
        if (resp is ApiResponse.Single)
        {
            return new PwEvent() { Single = (ApiResponse.Single)resp };
        }
        if (resp is ApiResponse.Container)
        {
            return new PwEvent() { Container = (ApiResponse.Container)resp };
        }
        throw new Exception("invalid api resp");
    }
}
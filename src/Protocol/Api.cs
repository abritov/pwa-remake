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
                gameCmd = JsonConvert.DeserializeObject<ApiResponse.ObjectMoveSingle>(jo.First.ToString(), SpecifiedSubclassConversion);
                break;
            case "ObjectStopMove":
                gameCmd = JsonConvert.DeserializeObject<ApiResponse.ObjectStopMoveSingle>(jo.First.ToString(), SpecifiedSubclassConversion);
                break;
            case "OwnExtProp":
                gameCmd = JsonConvert.DeserializeObject<ApiResponse.OwnExtPropSingle>(jo.First.ToString(), SpecifiedSubclassConversion);
                break;
            case "SelfInfo00":
                gameCmd = JsonConvert.DeserializeObject<ApiResponse.SelfInfo00Single>(jo.First.ToString(), SpecifiedSubclassConversion);
                break;
            case "ObjectLeaveSlice":
                gameCmd = JsonConvert.DeserializeObject<ApiResponse.ObjectLeaveSliceSingle>(jo.First.ToString(), SpecifiedSubclassConversion);
                break;
            case "PlayerLeaveWorld":
                gameCmd = JsonConvert.DeserializeObject<ApiResponse.PlayerLeaveWorldSingle>(jo.First.ToString(), SpecifiedSubclassConversion);
                break;
            case "OwnInventoryData":
                gameCmd = JsonConvert.DeserializeObject<ApiResponse.OwnInventoryInfoSingle>(jo.First.ToString(), SpecifiedSubclassConversion);
                break;
            case "OwnInventoryDetailData":
                gameCmd = JsonConvert.DeserializeObject<ApiResponse.OwnInventoryDetailDataSingle>(jo.First.ToString(), SpecifiedSubclassConversion);
                break;
            case "SkillData":
                gameCmd = JsonConvert.DeserializeObject<ApiResponse.SkillDataSingle>(jo.First.ToString(), SpecifiedSubclassConversion);
                break;
            case "TrashboxPwdState":
                gameCmd = JsonConvert.DeserializeObject<ApiResponse.TrashboxPwdStateSingle>(jo.First.ToString(), SpecifiedSubclassConversion);
                break;
            case "HostReputation":
                gameCmd = JsonConvert.DeserializeObject<ApiResponse.HostReputationSingle>(jo.First.ToString(), SpecifiedSubclassConversion);
                break;
            case "Unknown":
                var id = jo.First.First.Value<int>();
                var octetStream = jo.First.Last.Value<string>();
                gameCmd = new ApiResponse.UnknownGameCmdSingle(id, octetStream);
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


class ApiResponse
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
        public long Id { get; internal set; }
        public string OctetStream { get; internal set; }

        public UnknownSingle(long id, string octetStream)
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
            if (Cmd is ObjectMoveSingle) 
            {
                return new ObjectMove() 
                {
                    id = ((ObjectMoveSingle)Cmd).id,
                    dest = ((ObjectMoveSingle)Cmd).dest,
                    use_time = ((ObjectMoveSingle)Cmd).use_time,
                    speed = ((ObjectMoveSingle)Cmd).speed,
                    move_mode = ((ObjectMoveSingle)Cmd).move_mode,
                };
            }
            if (Cmd is ObjectStopMoveSingle) 
            {
                return new ObjectStopMove() 
                {
                    id = ((ObjectStopMoveSingle)Cmd).id,
                    dest = ((ObjectStopMoveSingle)Cmd).dest,
                    speed = ((ObjectStopMoveSingle)Cmd).speed,
                    dir = ((ObjectStopMoveSingle)Cmd).dir,
                    move_mode = ((ObjectStopMoveSingle)Cmd).move_mode,
                };
            }
            if (Cmd is SelfInfo00Single)
            {
                return new SelfInfo00() 
                {
                    level = ((SelfInfo00Single)Cmd).level,
                    state = ((SelfInfo00Single)Cmd).state,
                    level2 = ((SelfInfo00Single)Cmd).level2,
                    hp = ((SelfInfo00Single)Cmd).hp,
                    max_hp = ((SelfInfo00Single)Cmd).max_hp,
                    mp = ((SelfInfo00Single)Cmd).mp,
                    max_mp = ((SelfInfo00Single)Cmd).max_mp,
                    exp = ((SelfInfo00Single)Cmd).exp,
                    sp = ((SelfInfo00Single)Cmd).sp,
                    ap = ((SelfInfo00Single)Cmd).ap,
                    max_ap = ((SelfInfo00Single)Cmd).max_ap,
                };
            }
            if (Cmd is OwnExtPropSingle)
            {
                return ((OwnExtPropSingle)Cmd).IntoRpc();
            }
            if (Cmd is OwnInventoryInfoSingle)
            {
                return ((OwnInventoryInfoSingle)Cmd).IntoRpc();
            }
            if (Cmd is OwnInventoryDetailDataSingle)
            {
                return ((OwnInventoryDetailDataSingle)Cmd).IntoRpc();
            }
            if (Cmd is SkillDataSingle)
            {
                return ((SkillDataSingle)Cmd).IntoRpc();
            }
            if (Cmd is TrashboxPwdStateSingle)
            {
                return ((TrashboxPwdStateSingle)Cmd).IntoRpc();
            }
            if (Cmd is HostReputationSingle)
            {
                return ((HostReputationSingle)Cmd).IntoRpc();
            }
            if (Cmd is UnknownGameCmdSingle)
            {
                return ((UnknownGameCmdSingle)Cmd).IntoRpc();
            }
            if (Cmd is ObjectLeaveSliceSingle)
            {
                return ((ObjectLeaveSliceSingle)Cmd).IntoRpc();
            }
            if (Cmd is PlayerLeaveWorldSingle)
            {
                return ((PlayerLeaveWorldSingle)Cmd).IntoRpc();
            }
            throw new Exception($"unknown GameData {Cmd.ToString()}");
        }
    }
    public sealed class UnknownGameCmdSingle : GameCmd
    {
        public int Id { get; set; }
        public string OctetStream { get; set; }
        public UnknownGameCmdSingle(int id, string octetStream)
        {
            Id = id;
            OctetStream = octetStream;
        }

        public UnknownGameCmd IntoRpc() {
            return new UnknownGameCmd(Id, OctetStream);
        }
    }
    public sealed class ObjectMoveSingle : GameCmd
    {
        public long id { get; set; }
        public Point3D dest { get; set; }
        public int use_time { get; set; }
        public short speed { get; set; }
        public short move_mode { get; set; }
    }
    public sealed class ObjectStopMoveSingle : GameCmd
    {
        public long id { get; set; }
        public Point3D dest { get; set; }
        public int speed { get; set; }
        public int dir { get; set; }
        public int move_mode { get; set; }
    }
    public class BasePropSingle
    {
        public int vitality { get; set; } 
        public int energy { get; set; } 
        public int strength { get; set; } 
        public int agility { get; set; } 
        public int max_hp { get; set; } 
        public int max_mp { get; set; } 
        public int hp_gen { get; set; } 
        public int mp_gen { get; set; } 

        public BaseProp IntoRpc() {
            return new BaseProp()
            {
                vitality = vitality,
                energy = energy,
                strength = strength,
                agility = agility,
                max_hp = max_hp,
                max_mp = max_mp,
                hp_gen = hp_gen,
                mp_gen = mp_gen,
            };
        }
    }

    public class MovePropSingle
    {
        public double walk_speed { get; set; } 
        public double run_speed { get; set; } 
        public double swim_speed { get; set; } 
        public double flight_speed { get; set; } 

        public MoveProp IntoRpc() 
        {
            return new MoveProp()
            {
                walk_speed = walk_speed,
                run_speed = run_speed,
                swim_speed = swim_speed,
                flight_speed = flight_speed
            };
        }
    }

    public class AddonDamageSingle
    {
        public int damage_low { get; set; } 
        public int damage_high { get; set; } 

        public AddonDamage IntoRpc() 
        {
            return new AddonDamage() 
            {
                damage_low = damage_low,
                damage_high = damage_high,
            };
        }
    }

    public class AtkPropSingle
    {
        public int attack { get; set; } 
        public int damage_low { get; set; } 
        public int damage_high { get; set; } 
        public int attack_speed { get; set; } 
        public double attack_range { get; set; } 
        public List<AddonDamageSingle> addon_damage { get; set; } 
        public int damage_magic_low { get; set; } 
        public int damage_magic_high { get; set; } 

        public AtkProp IntoRpc() 
        {
            return new AtkProp() 
            {
                attack = attack,
                damage_low = damage_low,
                damage_high = damage_high,
                attack_speed = attack_speed,
                attack_range = attack_range,
                addon_damage = addon_damage.Select(x => x.IntoRpc()).ToList(),
                damage_magic_low = damage_magic_low,
                damage_magic_high = damage_magic_high
            };
        }
    }

    public class DefPropSingle
    {
        public List<int> resistance { get; set; } 
        public int defense { get; set; } 
        public int armor { get; set; } 

        public DefProp IntoRpc()
        {
            return new DefProp()
            {
                resistance = resistance,
                defense = defense,
                armor = armor
            };
        }
    }

    public class RoleExtPropSingle
    {
        public BasePropSingle base_prop { get; set; } 
        public MovePropSingle move_prop { get; set; } 
        public AtkPropSingle atk_prop { get; set; } 
        public DefPropSingle def_prop { get; set; } 
        public int max_ap { get; set; } 

        public RoleExtProp IntoRpc() {
            return new RoleExtProp()
            {
                base_prop = base_prop.IntoRpc(),
                move_prop = move_prop.IntoRpc(),
                atk_prop = atk_prop.IntoRpc(),
                def_prop = def_prop.IntoRpc(),
                max_ap = max_ap,
            };
        }
    }
    public sealed class OwnExtPropSingle : GameCmd {
        public int status_point { get; set; } 
        public int? attack_degree { get; set; } 
        public int? defend_degree { get; set; } 
        public int? crit_rate { get; set; } 
        public int? crit_damage_bonus { get; set; } 
        public int? invisible_degree { get; set; } 
        public int? anti_invisible_degree { get; set; } 
        public int? penetration { get; set; } 
        public int? resilience { get; set; } 
        public int? vigour { get; set; } 
        public RoleExtPropSingle prop { get; set; } 

        public OwnExtProp IntoRpc()
        {
            return new OwnExtProp()
            {
                status_point = status_point,
                attack_degree = attack_degree,
                defend_degree = defend_degree,
                crit_rate = crit_rate,
                crit_damage_bonus = crit_damage_bonus,
                invisible_degree = invisible_degree,
                anti_invisible_degree = anti_invisible_degree,
                penetration = penetration,
                resilience = resilience,
                vigour = vigour,
                prop = prop.IntoRpc()
            };
        }
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
    public sealed class ObjectLeaveSliceSingle : GameCmd
    {
        public long object_id { get; set; }

        public ObjectLeaveSlice IntoRpc()
        {
            return new ObjectLeaveSlice()
            {
                object_id = object_id
            };
        }
    }
    public sealed class PlayerLeaveWorldSingle : GameCmd
    {
        public long player_id { get; set; }

        public PlayerLeaveWorld IntoRpc()
        {
            return new PlayerLeaveWorld()
            {
                player_id = player_id
            };
        }
    }
    public sealed class OwnInventoryInfoSingle : GameCmd
    {
        public int by_package { get; set; } 
        public int inventory_size { get; set; } 
        public List<byte> content { get; set; } 

        public OwnInventoryInfo IntoRpc()
        {
            return new OwnInventoryInfo()
            {
                by_package = by_package,
                inventory_size = inventory_size,
                content = content
            };
        }
    }
    public sealed class OwnInventoryDetailDataSingle : GameCmd
    {
        public int by_package { get; set; } 
        public int inventory_size { get; set; } 
        public List<byte> content { get; set; } 

        public OwnInventoryDetailData IntoRpc()
        {
            return new OwnInventoryDetailData()
            {
                by_package = by_package,
                inventory_size = inventory_size,
                content = content
            };
        }
    }
    public class SkillSingle
    {
        public int id { get; set; } 
        public int level { get; set; } 
        public int ability { get; set; } 

        public Skill IntoRpc()
        {
            return new Skill()
            {
                id = id,
                level = level,
                ability = ability
            };
        }
    }

    public class SkillDataSingle : GameCmd
    {
        public List<SkillSingle> skills { get; set; } 

        public SkillData IntoRpc()
        {
            return new SkillData()
            {
                skills = skills.Select(x => x.IntoRpc()).ToList()
            };
        }
    }

    public class TrashboxPwdStateSingle : GameCmd
    {
        public byte has_passwd { get; set; }

        public TrashboxPwdState IntoRpc()
        {
            return new TrashboxPwdState()
            {
                has_passwd = has_passwd == 1
            };
        }
    }

    public class HostReputationSingle : GameCmd
    {
        public int reputation { get; set; } 

        public HostReputation IntoRpc() 
        {
            return new HostReputation()
            {
                reputation = reputation
            };
        }
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
        public long dst_id { get; set; }
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
        public ObjectMoveSingle ObjectMove { get; internal set; }
        public ObjectStopMoveSingle ObjectStopMove { get; internal set; }
        public OwnExtPropSingle OwnExtProp { get; internal set; }
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

class Api
{

    public string EnterWorld(string serverAddr, Account account)
    {
        return $"{{\"addr\": \"{serverAddr}\", \"select_role_by_index\": {account.DefaultRoleIndex}, \"account\": {{\"login\": \"{account.Login}\", \"passwd\": \"{account.Password}\"}}}}";
    }

    public PwRpc ParseEvent(string msg)
    {
        // Console.WriteLine(msg);
        try
        {
            var resp = JsonConvert.DeserializeObject<ApiResponse.Root>(msg);
            if (resp is ApiResponse.Single)
            {
                return (PwRpc)((ApiResponse.Single)resp).IntoRpc();
            }
            if (resp is ApiResponse.Container)
            {
                return new RpcContainer(
                    ((ApiResponse.Container)resp).Id,
                    ((ApiResponse.Container)resp).Packets.Select(x => (RpcSingle)x.IntoRpc()).ToList()
                );
            }
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
        }
        throw new Exception("ParseEvent invalid data");
    }
}
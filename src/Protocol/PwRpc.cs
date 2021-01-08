using System.Collections.Generic;

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

public abstract class GameData : RpcSingle { }
public sealed class ObjectMove : GameData 
{
    public long id { get; set; }
    public Point3D dest { get; set; }
    public int use_time { get; set; }
    public short speed { get; set; }
    public short move_mode { get; set; }
}
public sealed class ObjectStopMove : GameData 
{
    public long id { get; set; }
    public Point3D dest { get; set; }
    public int speed { get; set; }
    public int dir { get; set; }
    public int move_mode { get; set; }
}
public sealed class SelfInfo00: GameData
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


public class BaseProp
{
    public int vitality { get; set; } 
    public int energy { get; set; } 
    public int strength { get; set; } 
    public int agility { get; set; } 
    public int max_hp { get; set; } 
    public int max_mp { get; set; } 
    public int hp_gen { get; set; } 
    public int mp_gen { get; set; } 
}

public class MoveProp
{
    public double walk_speed { get; set; } 
    public double run_speed { get; set; } 
    public double swim_speed { get; set; } 
    public double flight_speed { get; set; } 
}

public class AddonDamage
{
    public int damage_low { get; set; } 
    public int damage_high { get; set; } 
}

public class AtkProp
{
    public int attack { get; set; } 
    public int damage_low { get; set; } 
    public int damage_high { get; set; } 
    public int attack_speed { get; set; } 
    public double attack_range { get; set; } 
    public List<AddonDamage> addon_damage { get; set; } 
    public int damage_magic_low { get; set; } 
    public int damage_magic_high { get; set; } 
}

public class DefProp
{
    public List<int> resistance { get; set; } 
    public int defense { get; set; } 
    public int armor { get; set; } 
}

public class RoleExtProp
{
    public BaseProp base_prop { get; set; } 
    public MoveProp move_prop { get; set; } 
    public AtkProp atk_prop { get; set; } 
    public DefProp def_prop { get; set; } 
    public int max_ap { get; set; } 
}
public sealed class OwnExtProp : GameData
{
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
    public RoleExtProp prop { get; set; } 
}
public sealed class OwnInventoryInfo : GameData
{
    public int by_package { get; set; } 
    public int inventory_size { get; set; } 
    public List<byte> content { get; set; } 
}
public sealed class OwnInventoryDetailData : GameData
{
    public int by_package { get; set; } 
    public int inventory_size { get; set; } 
    public List<byte> content { get; set; } 
}
public sealed class UnknownGameCmd : GameData
{
    public int Id { get; set; }
    public string OctetStream { get; set; }
    public UnknownGameCmd(int id, string octetStream)
    {
        Id = id;
        OctetStream = octetStream;
    }
}
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
    public int id { get; set; }
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
public sealed class OwnExtProp: GameData
{

}
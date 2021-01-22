using PWARemake.Lib.Game;

namespace PWARemake.Lib.Protocol
{
    public abstract class RpcCommand { }
    public abstract class GameCmd : RpcCommand { }
    public sealed class CmdPlayerMove : GameCmd
    {
        public Point3D current_pos { get; internal set; }
        public Point3D next_pos { get; internal set; }
        public short use_time { get; internal set; }
        public short speed { get; internal set; }
        public MoveMode move_mode { get; internal set; }
        public short stamp { get; internal set; }
    }
    public sealed class CmdPlayerStopMove : GameCmd
    {
        public Point3D current_pos { get; internal set; }
        public short speed { get; internal set; }
        public byte dir { get; internal set; }
        public MoveMode move_mode { get; internal set; }
        public short stamp { get; internal set; }
        public short use_time { get; internal set; }
    }
    public sealed class CmdReviveVillage : GameCmd { }
    public sealed class CmdGetAllData : GameCmd
    {
        public bool include_pack_info { get; internal set; }
        public bool include_equip_info { get; internal set; }
        public bool include_task_info { get; internal set; }
    }

}
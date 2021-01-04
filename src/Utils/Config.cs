class Offsets {
    public int BaseAddress { get; private set; }
    public int Hp { get; private set; }
    public int Mp { get; private set; }
    public int HostCharId { get; private set; }
    public int TargetId { get; private set; }
    public int Player { get; private set; }
    public int Name { get; private set; }
    public int Coordinates { get; private set; }
}

class Server {
    public string Host { get; private set; }
    public int Port { get; private set; }
    public bool AutoReconnect { get; private set; }
    public bool AutoHeal { get; private set; }
    public bool AutoRessurect { get; private set; }
    public Offsets Offsets { get; private set; }
    public int Version { get; private set; }
}

class Macros {
    public string Name { get; private set; }
    public string Hotkey { get; private set; }
    public Skill[] Skills { get; private set; }
}

public class Account {
    public string Login { get; set; }
    public string Password { get; set; }
    public int DefaultRoleIndex { get; set; }
}

class Config {
    public Server[] Servers { get; private set; }
    public Account[] Accounts { get; private set; }
}

interface IConfigManager {
    Config Load(string path);
    void Save(string path, Config config);
}

class ConfigManager: IConfigManager {
    public Config Load(string path) {
        return new Config();
    }

    public void Save(string path, Config config) {

    }
}
namespace Terminal_XP.Classes
{
    public static class ConfigManager
    {
        private const string Path = "files/Config.json";
        
        public static Config Config { get; private set; }

        private static Manager<Config> _manager;

        static ConfigManager() => _manager = new Manager<Config>(Path);

        public static void Load() => Config = _manager.Load();

        public static void Save() => _manager.Save(Config);
    }
}
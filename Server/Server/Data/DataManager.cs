using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Server.Data
{
    public interface ILoader<Key, Value>
    {
        Dictionary<Key, Value> MakeDict();
    }

    public class DataManager
    {
        public static Dictionary<int, Data.LevelStat> LevelStatDict { get; private set; } = new Dictionary<int, Data.LevelStat>();
        public static Dictionary<int, Data.PowerUpSkill> PowerUpDict { get; private set; } = new Dictionary<int, Data.PowerUpSkill>();
        public static Dictionary<int, Data.RangeUpSkill> RangeUpDict { get; private set; } = new Dictionary<int, Data.RangeUpSkill>();
        public static Dictionary<int, Data.WaterCountUpSkill> WaterCountUpDict { get; private set; } = new Dictionary<int, Data.WaterCountUpSkill>();
        public static Dictionary<int, Data.SpeedUpSkill> SpeedUpDict { get; private set; } = new Dictionary<int, Data.SpeedUpSkill>();

        public void Init()
        {
            LevelStatDict = LoadJson<LevelStatData, int, Data.LevelStat>("LevelStat.json").MakeDict();
        }

        static Loader LoadJson<Loader, Key, Value>(string path) where Loader : ILoader<Key, Value>
        {
            string text = File.ReadAllText(path);
            return Newtonsoft.Json.JsonConvert.DeserializeObject<Loader>(text);
        }
}

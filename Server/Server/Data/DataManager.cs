using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Server.Data
{
    public class DataManager
    {
        public static Dictionary<int, Data.LevelStat> LevelStatDict { get; private set; } = new Dictionary<int, Data.LevelStat>();
        public static Dictionary<int, Data.PowerUpSkill> PowerUpDict { get; private set; } = new Dictionary<int, Data.PowerUpSkill>();
        public static Dictionary<int, Data.RangeUpSkill> RangeUpDict { get; private set; } = new Dictionary<int, Data.RangeUpSkill>();
        public static Dictionary<int, Data.WaterCountUpSkill> WaterCountUpDict { get; private set; } = new Dictionary<int, Data.WaterCountUpSkill>();
        public static Dictionary<int, Data.SpeedUpSkill> SpeedUpDict { get; private set; } = new Dictionary<int, Data.SpeedUpSkill>();

        public static void LoadData()
        {
            LevelStatDict = LoadJson<LevelStatData, int, Data.LevelStat>("LevelStat.json").MakeDict();
            PowerUpDict = LoadJson<PowerUpSkillData, int, Data.PowerUpSkill>("PowerUpSkill.json").MakeDict();
            RangeUpDict = LoadJson<RangeUpSkillData, int, Data.RangeUpSkill>("RangeUpSkill.json").MakeDict();
            WaterCountUpDict = LoadJson<WaterCountUpSkillData, int, Data.WaterCountUpSkill>("WaterCountUpSkill.json").MakeDict();
            SpeedUpDict = LoadJson<SpeedUpSkillData, int, Data.SpeedUpSkill>("SpeedUpSkill.json").MakeDict();
        }

        static Loader LoadJson<Loader, Key, Value>(string path)
        {
            string text = File.ReadAllText($"../../../../../2D_BattleGround/Assets/Resources/Data/{path}");
            return Newtonsoft.Json.JsonConvert.DeserializeObject<Loader>(text);
        }
    }
}

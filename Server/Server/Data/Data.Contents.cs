using System;
using System.Collections.Generic;
using System.Text;

namespace Server.Data
{
    #region Speed Up
    [Serializable]
    public class LevelStat
    {
        public int level;
        public int totalEXP;
    }
    [Serializable]
    public class LevelStatData
    {
        public List<LevelStat> levelStats = new List<LevelStat>();

        public Dictionary<int, LevelStat> MakeDict()
        {
            Dictionary<int, LevelStat> dic = new Dictionary<int, LevelStat>();
            foreach(LevelStat LevelStat in levelStats)
            {
                dic.Add(LevelStat.level, LevelStat);
            }

            return dic;
        }
    }
    #endregion

    #region Speed Up
    [Serializable]
    public class SpeedUpSkill
    {
        public int point;
        public int speed;
    }
    [Serializable]
    public class SpeedUpSkillData
    {
        public List<SpeedUpSkill> speedUpSkills = new List<SpeedUpSkill>();

        public Dictionary<int, SpeedUpSkill> MakeDict()
        {
            Dictionary<int, SpeedUpSkill> dic = new Dictionary<int, SpeedUpSkill>();
            foreach (SpeedUpSkill speedUpSkills in speedUpSkills)
            {
                dic.Add(speedUpSkills.point, speedUpSkills);
            }
            return dic;
        }
    }
    #endregion

    #region RangeUp
    [Serializable]
    public class RangeUpSkill
    {
        public int point;
        public int range;
    }
    [Serializable]
    public class RangeUpSkillData
    {
        public List<RangeUpSkill> rangeUpSkills = new List<RangeUpSkill>();
        public Dictionary<int, RangeUpSkill> MakeDict()
        {
            Dictionary<int, RangeUpSkill> dic = new Dictionary<int, RangeUpSkill>();
            foreach (RangeUpSkill rangeUpSkill in rangeUpSkills)
            {
                dic.Add(rangeUpSkill.point, rangeUpSkill);
            }
            return dic;
        }
    }
    #endregion

    #region PowerUp
    [Serializable]
    public class PowerUpSkill
    {
        public int point;
        public float power;
    }

    [Serializable]
    public class PowerUpSkillData
    {
        public List<PowerUpSkill> powerUpSkills = new List<PowerUpSkill>();
        public Dictionary<int, PowerUpSkill> MakeDict()
        {
            Dictionary<int, PowerUpSkill> dic = new Dictionary<int, PowerUpSkill>();
            foreach (PowerUpSkill powerUpSkill in powerUpSkills)
            {
                dic.Add(powerUpSkill.point, powerUpSkill);
            }
            return dic;
        }
    }
    #endregion

    #region WaterCountUp
    [Serializable]
    public class WaterCountUpSkill
    {
        public int point;
        public int waterMaxCount;
    }
    [Serializable]
    public class WaterCountUpSkillData
    {
        public List<WaterCountUpSkill> waterCountUpSkills = new List<WaterCountUpSkill>();
        public Dictionary<int, WaterCountUpSkill> MakeDict()
        {
            Dictionary<int, WaterCountUpSkill> dic = new Dictionary<int, WaterCountUpSkill>();
            foreach (WaterCountUpSkill waterCountUpSkill in waterCountUpSkills)
            {
                dic.Add(waterCountUpSkill.point, waterCountUpSkill);
            }
            return dic;
        }
    }
    #endregion
}

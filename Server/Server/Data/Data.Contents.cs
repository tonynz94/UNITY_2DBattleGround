using System;
using System.Collections.Generic;
using System.Text;

namespace Server.Data
{
    #region Speed Up
    public class LevelStat
    {
        public int level;
        public int totalEXP;
    }

    public class LevelStatData : ILoader<int , LevelStat>
    {
        List<LevelStat> LevelStats = new List<LevelStat>();

        public Dictionary<int, LevelStat> MakeDict()
        {
            Dictionary<int, LevelStat> dic = new Dictionary<int, LevelStat>();
            foreach(LevelStat LevelStat in LevelStats)
            {
                dic.Add(LevelStat.level, LevelStat);
            }

            return dic;
        }
    }
    #endregion

    #region Speed Up
    public class SpeedUpSkill
    {
        public int id;
        public int name;
        public int speed;
    }

    public class SpeedUpSkillData
    {

    }
    #endregion

    #region RangeUp
    public class RangeUpSkill
    {
        public int id;
        public int name;
        public int range;
    }

    public class RangeUpSkillData
    {

    }
    #endregion

    #region PowerUp
    public class PowerUpSkill
    {
        public int id;
        public int name;
        public float power;
    }


    public class PowerUpSkillData
    {

    }
    #endregion

    #region WaterCountUp
    public class WaterCountUpSkill
    {
        public int id;
        public int name;
        public int waterMaxCount;
    }

    public class WaterCountUpSkillData
    {

    }
    #endregion
}

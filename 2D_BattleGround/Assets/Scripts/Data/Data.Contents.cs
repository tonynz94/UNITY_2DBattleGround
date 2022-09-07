using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Data
{
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
            foreach (LevelStat LevelStat in levelStats)
            {
                dic.Add(LevelStat.level, LevelStat);
            }

            return dic;
        }
    }



};
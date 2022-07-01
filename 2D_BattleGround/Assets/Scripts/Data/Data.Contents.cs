using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Data
{
    #region Stat
    [Serializable]
    public class Stat
    {
        public int level;
        public int maxHp;
        public int maxMp;
        public int attack;
        public int defense;
        public int critical;
        public int evasive;
        public int totalExp;
    }

    [Serializable]
    public class StatData : ILoader<int, Stat>
    {
        public List<Stat> stats = new List<Stat>();

        public Dictionary<int, Stat> MakeDict()
        {
            Dictionary<int, Stat> statData = new Dictionary<int, Stat>();
            
            foreach (Stat stat in stats)
                statData.Add(stat.level, stat);
            
            return statData;
        }
    }

    #endregion



};
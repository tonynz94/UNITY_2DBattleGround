using Server.Util;
using System;
using System.Collections.Generic;
using System.Text;
using static Server.Define;

namespace Server.Game
{
    class WaterBoomObject : GameObject
    {
        long _blowUpTick = 3000;
        public WaterBoomObject(Vector2Int pos)
        {
            _objectType = ObjectType.WaterBoom;
            _cellPos = pos;
        }
        public void Update()
        {
            Environment.TickCount64
        }


        public void WaterBoomBlowUp()
        {
            if (_blowUpTick > Environment.TickCount64)
                return;

            Console.WriteLine("POW");
        }
    }
}

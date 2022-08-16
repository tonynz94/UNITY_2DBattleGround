using Server.Util;
using System;
using System.Collections.Generic;
using System.Text;
using static Server.Define;

namespace Server.Game
{
    class WaterBoomObject : GameObject
    {
        public WaterBoomObject(Vector2Int pos)
        {
            _objectType = ObjectType.WaterBoom;
            _cellPos = pos;
        }
        public void Update()
        {
            
        }


        public void WaterBoomBlowUp()
        {

        }
    }
}

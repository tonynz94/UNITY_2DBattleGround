using Server.Util;
using System;
using System.Collections.Generic;
using System.Text;
using static Server.Define;

namespace Server.Game
{

    class GameObject
    {
        public Vector2Int _cellPos { get; protected set; }
        public int _roomID { get; protected set; }
        public ObjectType _objectType { get; protected set; } = ObjectType.None;

        public GameObject()
        {

        }

        
    }
}

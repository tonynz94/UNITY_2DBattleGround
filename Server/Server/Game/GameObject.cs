using Server.Util;
using System;
using System.Collections.Generic;
using System.Text;
using static Server.Define;

namespace Server.Game
{

    class GameObject
    {
        public int _ownerID { get; set; }
        public int _id { get; set; }
        public Vector2Int _cellPos { get; set; }
        public int _roomID { get; set; }
        public ObjectType _objectType { get; protected set; } = ObjectType.None;

        public GameObject()
        {

        }

        
    }
}

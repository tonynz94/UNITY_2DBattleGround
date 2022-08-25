using Server.Util;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using static Server.Define;

namespace Server.Game
{
    class WaterBoomObject : GameObject
    {
        long _blowUpTick = 3000;
        long _startTick = 0;
        int _blowXYRange = 1;

        Task t1;
        CancellationTokenSource tokenSource = new CancellationTokenSource();

        public WaterBoomObject(int roomID, Vector2Int pos, int blowXYRange = 1)
        {
            _roomID = roomID;
            _blowXYRange = blowXYRange;
            _objectType = ObjectType.WaterBoom;
            _cellPos = pos;

            _startTick = Environment.TickCount;

            t1 = new Task(Update);
            t1.Start();
        }
        
        public int GetWaterBlowRange()
        {
            return _blowXYRange;
        }

        public Vector2Int GetPos()
        {
            return _cellPos;
        }

        public void Update()
        {
            while (true)
            {
                if (_startTick + _blowUpTick < Environment.TickCount)
                {
                    Console.WriteLine("POW");
                    WaterBoomBlowUp();
                    Console.WriteLine("비워주기");
                    break;
                }

                if (tokenSource.IsCancellationRequested)
                {
                    Console.WriteLine("다른 물풍선에 의해 3초보다 먼저 POW");
                    break;
                }
            }
        }

        public void WaterBoomBlowUp()
        {
            //StopCorotuine
            if (!t1.IsCompleted) 
                tokenSource.Cancel();

            GameManager.Instance.GetGameField(_roomID).BlowWaterBoom(this);
        }
    }
}

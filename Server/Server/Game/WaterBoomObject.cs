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

        Task t1;
        CancellationTokenSource tokenSource = new CancellationTokenSource();

        public WaterBoomObject(Vector2Int pos)
        {
            _objectType = ObjectType.WaterBoom;
            _cellPos = pos;

            _startTick = Environment.TickCount;

            t1 = new Task(Update);
            t1.Start();
        }

        public void Update()
        {
            while (true)
            {
                if (_startTick + _blowUpTick < Environment.TickCount)
                {
                    Console.WriteLine("POW");
                    WaterBoomBlowUp();
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
            //TODO
            //삭제 해주고 충돌체크 해주면 끝 난 잔다 이제
            //Destroy(gameObject);
            //Managers.Game.BlowWaterBoom(this);

        }
    }
}

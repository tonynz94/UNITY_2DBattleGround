using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaterBoomObject : MonoBehaviour
{
    public enum Direction
    {
        UP,
        DOWN,
        LEFT,
        RIGHT,
        FourDirections
    }

    Vector2Int[] DIR = new Vector2Int[(int)Direction.FourDirections] { new Vector2Int(0, 1), new Vector2Int(0, -1), new Vector2Int(-1, 0), new Vector2Int(1, 0) };

    // Start is called before the first frame update
    protected Vector2Int _cellPos;
    protected int _blowXYRange = 1;
    Coroutine co;



    void Start()
    {
        //StartCoroutine(AfterSecWaterBlow(2.0f));
    }

    public void SetInField(Vector2Int cellPos, int blowXYRange = 1)
    {
        _cellPos = cellPos;
        _blowXYRange = blowXYRange;
    }

    public Vector2Int GetPos()
    {
        return _cellPos;
    }

    public int GetWaterBlowRange()
    {
        return _blowXYRange;
    }

    //IEnumerator AfterSecWaterBlow(float sec)
    //{
    //    yield return new WaitForSeconds(sec);
    //    Debug.Log("POW!!");
    //    WaterBoomBlowUp();
    //}

    public void WaterBoomBlowUp()
    {
        //StopAllCoroutines();
        Destroy(gameObject);
        DrawWaterBlow();
    }

    public void DrawWaterBlow()
    {
        {
            GameObject boomObject = Managers.Resource.Instantiate("Objects/WaterBlowEffectObject");
            boomObject.transform.localPosition = new Vector3(_cellPos.x + 0.5f, _cellPos.y + 0.5f, 0);
            Object.Destroy(boomObject, 0.8f);
        }


        for (Direction dir = Direction.UP; dir < Direction.FourDirections; dir++) {
            for (int i = 0; i < _blowXYRange; i++)
            {
                GameObject boomObject = Managers.Resource.Instantiate("Objects/WaterBlowEffectObject");
                boomObject.transform.localPosition = new Vector3(_cellPos.x + 0.5f + DIR[(int)dir].x, _cellPos.y + 0.5f + DIR[(int)dir].y, 0);
                Object.Destroy(boomObject, 0.8f);
            }
        }

        
    }
}

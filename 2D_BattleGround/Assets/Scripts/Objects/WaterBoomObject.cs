using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaterBoomObject : MonoBehaviour
{
    // Start is called before the first frame update
    protected Vector2Int _cellPos;
    protected int _blowXYRange = 1;
    Coroutine co;

    void Start()
    {
        StartCoroutine(AfterSecWaterBlow(2.0f));
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

    IEnumerator AfterSecWaterBlow(float sec)
    {
        yield return new WaitForSeconds(sec);
        Debug.Log("POW!!");
        WaterBoomBlowUp();
    }

    public void WaterBoomBlowUp()
    {
        StopAllCoroutines();
        Managers.Game.BlowWaterBoom(this);
        Object.Destroy(this);
    }
}

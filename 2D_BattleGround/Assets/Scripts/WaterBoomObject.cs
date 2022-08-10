using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaterBoomObject : MonoBehaviour
{
    // Start is called before the first frame update
    public Vector2Int _cellPos;

    void Start()
    {
        StartCoroutine(AfterSecWaterBlow(2.0f));
    }

    public void InitPos(Vector2Int cellPos)
    {
        _cellPos = cellPos;
    }

    IEnumerator AfterSecWaterBlow(float sec)
    {
        yield return new WaitForSeconds(sec);
        Debug.Log("POW!!");
        Managers.Game.BlowBoom(_cellPos);
    }

    public void WaterBoomBlowUp()
    {
        Managers.Resource.Destroy(this.gameObject);
    }
}

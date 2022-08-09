using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaterBoomObject : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(AfterSecWaterBlow(2.0f));
    }

    IEnumerator AfterSecWaterBlow(float sec)
    {
        yield return new WaitForSeconds(sec);
        Debug.Log("POW!!");

        Managers.Resource.Destroy(this.gameObject);
    }
}

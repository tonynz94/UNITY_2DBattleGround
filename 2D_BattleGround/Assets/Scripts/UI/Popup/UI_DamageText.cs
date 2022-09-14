using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class UI_DamageText : UI_WorldSpace
{
    private int _damage;
    enum Texts
    {
        DamageText,
    }
    //Awake
    public override bool Init()
    {
        Debug.Log("Awake");
        if (base.Init() == false)
            return false;

        BindText(typeof(Texts));

        return true;
    }

    public void ShowDamage(int damage)
    { 
       _damage = damage;
        GetText((int)Texts.DamageText).text = _damage.ToString();
        StartCoroutine(coDestory());

        transform.localScale = new Vector3(0f, 0f, 0f);
        gameObject.transform.DOScale(new Vector3(0.01f, 0.01f, 0.01f), 0.5f);
    }

    IEnumerator coDestory()
    {
        yield return new WaitForSeconds(0.8f);
        Managers.Resource.Destroy(gameObject);
    }
}

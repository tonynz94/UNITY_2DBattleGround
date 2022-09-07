using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using System;
using Data;

public class UI_InGame : UI_Scene
{
    int _myRank = 0;
    int clickCount = 0;

    int _plusMoney = 0;
    int _plusDia = 0;
    int _plusExp = 0;
    enum Buttons
    {
        ToWinnerResultButton,
        ToLoserResultButton,
        ToLobbyButton,
        NextStepButton,
    }

    enum Texts
    {
        MiddleNoticeText,
        SurvivorText,
        ResultRankText,

        LevelText,
        ObtainExpText,
        CoinText,
        DiamondText,

        SpeedUpText,
        AttackRangeText,
        DamageUpText,
        WaterBallonNumText,
    }

    enum GameObjects
    {
        StartNoticeObject,
        InGameObject,
        LoserObject,
        WinnerObject,
        KillAndDeathBoardObject,

        ExpObject,
        MoneyObject,
        DiamondObject,
        SkillPointObject,
        ExpBarControllerObject,
        ResultObject,
    }

    public void OnEnable()
    {
        clickCount = 0;
        MessageSystem.RegisterMessageSystem((int)MESSAGE_EVENT_TYPE.MESS_PLAYERDIE, OnPlayerDieMessage);
        MessageSystem.RegisterMessageSystem((int)MESSAGE_EVENT_TYPE.MESS_PLAYERWINNER, OnWinnerMessage);
        MessageSystem.RegisterMessageSystem((int)MESSAGE_EVENT_TYPE.MESS_PLAYERDEATH, OnDeathMessage);
    }

    public void OnDisable()
    {
        MessageSystem.UnRegisterMessageSystem((int)MESSAGE_EVENT_TYPE.MESS_PLAYERDIE, OnPlayerDieMessage);
        MessageSystem.UnRegisterMessageSystem((int)MESSAGE_EVENT_TYPE.MESS_PLAYERWINNER, OnWinnerMessage);
        MessageSystem.UnRegisterMessageSystem((int)MESSAGE_EVENT_TYPE.MESS_PLAYERDEATH, OnDeathMessage);
    }

    public override bool Init()
    {
        if (base.Init() == false)
            return false;

        BindButton(typeof(Buttons));
        BindText(typeof(Texts));
        BindObject(typeof(GameObjects));

        BindEvent(GetButton((int)Buttons.ToWinnerResultButton).gameObject, OnToResultButton);
        BindEvent(GetButton((int)Buttons.ToLoserResultButton).gameObject, OnToResultButton);
        BindEvent(GetButton((int)Buttons.ToLobbyButton).gameObject, OnToLobbyButton);
        BindEvent(GetButton((int)Buttons.NextStepButton).gameObject, OnNextStepButton);

        return true;
    }

    private void Start()
    {
        GetObject((int)GameObjects.WinnerObject).SetActive(false);
        GetObject((int)GameObjects.LoserObject).SetActive(false);
        GetObject((int)GameObjects.ResultObject).SetActive(false);

        GetObject((int)GameObjects.ExpObject).SetActive(false);
        GetObject((int)GameObjects.MoneyObject).SetActive(false);
        GetObject((int)GameObjects.DiamondObject).SetActive(false);
        GetObject((int)GameObjects.SkillPointObject).SetActive(false);

        GetButton((int)Buttons.ToLobbyButton).gameObject.SetActive(false);

        StartCoroutine(coStart());
        StartCoroutine(coCameraAnimation());


        GetText((int)Texts.SurvivorText).text = string.Format("Suvivor : {0}", Managers.Game._startPlayerCount.ToString());  
    }

    public void OnPlayerDieMessage(object obj)
    {
        KillDeath killDeath = obj as KillDeath;
        GameObject go = Managers.Resource.Instantiate("UI/SubItem/UI_KillAndDeathItem", GetObject((int)GameObjects.KillAndDeathBoardObject).transform);
        go.GetComponent<UI_KillAndDeathItem>().SetKillDeath(killDeath);
        RefreshSurvivor();
    }

    public void RefreshSurvivor()
    {
        GetText((int)Texts.SurvivorText).text = string.Format("Suvivor : {0}", Managers.Game.GetPlayerCount() - 1);
    }

    public void OnWinnerMessage(object obj)
    {
        _myRank = 1;
        GetObject((int)GameObjects.WinnerObject).SetActive(true);
        GetObject((int)GameObjects.LoserObject).SetActive(false);
    }

    public void OnDeathMessage(object obj)
    {
        _myRank = ((int)obj) + 1;
        GiveReward(_myRank);
        GetObject((int)GameObjects.WinnerObject).SetActive(false);
        GetObject((int)GameObjects.LoserObject).SetActive(true);  
    }

    public void OnToResultButton()
    {
        GetObject((int)GameObjects.WinnerObject).SetActive(false);
        GetObject((int)GameObjects.LoserObject).SetActive(false);
        GetObject((int)GameObjects.ResultObject).SetActive(true);

        GetText((int)Texts.ResultRankText).text = $"Rank : {_myRank}";
        GiveReward(_myRank);
    }

    public void OnToLobbyButton()
    {
        C_FieldToLobby cPkt = new C_FieldToLobby();
        cPkt.RoomID = Managers.Game.GetCurrentRoomID();
        cPkt.CGUID = Managers.Player.GetMyCGUID();

        Managers.Net.Send(cPkt.Write());

        Managers.Game.ClearRoom();

        Managers.Scene.ChangeScene(Define.Scene.LobbyScene);
        
    }

    public void OnNextStepButton()
    {
        if(clickCount == 0)
        {
            GetObject((int)GameObjects.ExpObject).SetActive(true);
            int level = Managers.Player.MyPlayer._level;
            StartCoroutine(coExpObtain());
        }
        else if(clickCount == 1)
        {
            GetObject((int)GameObjects.MoneyObject).SetActive(true);
            StartCoroutine(coCoinObtain());
        }
        else if(clickCount == 2)
        {
            GetObject((int)GameObjects.DiamondObject).SetActive(true);
            StartCoroutine(coDiaObtain());
        }
        else if(clickCount == 3)
        {
            GetObject((int)GameObjects.SkillPointObject).SetActive(true);
            GetText((int)Texts.SpeedUpText).text = $"Speed UP : {Managers.Player.MyPlayer._SpeedUpSkillCount}";
            GetText((int)Texts.AttackRangeText).text = $"Attack Range : {Managers.Player.MyPlayer._RangeUpSkillCount}";
            GetText((int)Texts.DamageUpText).text = $"Damage : {Managers.Player.MyPlayer._PowerUpSkillCount}";
            GetText((int)Texts.WaterBallonNumText).text = $"Water Ballon : {Managers.Player.MyPlayer._WaterCountUpSkillCount}";
            Managers.Player.MyPlayer._skillPoint += 2;
        }
        else if(clickCount == 4)
        {
            GetButton((int)Buttons.NextStepButton).gameObject.SetActive(false);
            GetButton((int)Buttons.ToLobbyButton).gameObject.SetActive(true);
        }
        clickCount++;
    }

    IEnumerator coExpObtain()
    {
        float currentTime = 0;
        int level = Managers.Player.MyPlayer._level;

        GetText((int)Texts.LevelText).text = "LV " + level.ToString();
        GetText((int)Texts.ObtainExpText).text = $"+{_plusExp} EXP";

        Managers.Player.MyPlayer.Exp += _plusExp;

        LevelStat levelStat;
        LevelStat beforeLevelStat;
        Managers.Data.LevelStatDict.TryGetValue(level, out levelStat);
        Managers.Data.LevelStatDict.TryGetValue(level-1, out beforeLevelStat);

        int currentLevelTotalEXP = levelStat.totalEXP - beforeLevelStat.totalEXP;

        float startPos = GetObject((int)GameObjects.ExpBarControllerObject).transform.localScale.x;
        float desPos = (Managers.Player.MyPlayer.Exp - beforeLevelStat.totalEXP) / currentLevelTotalEXP;

        float distance = desPos - startPos;

        while (currentTime < 2.0f)
        {
            currentTime += Time.deltaTime;
            
            float scaleX = GetObject((int)GameObjects.ExpBarControllerObject).transform.localScale.x + ((distance/2.0f) * Time.deltaTime);

            if (scaleX > 1.0f)
            {
                GetText((int)Texts.LevelText).text = $"LV {++level}";
                scaleX = 0f;
            }
            GetObject((int)GameObjects.ExpBarControllerObject).transform.localScale = new Vector3(scaleX, 1, 1);
            yield return null;
        }
        
    }

    IEnumerator coCoinObtain()
    {
        int start = 0;
        int des = _plusMoney;

        float currentTime = 0;

        Managers.Player.MyPlayer._gameMoney += _plusMoney;
        while (currentTime < 2.0f)
        {
            currentTime += Time.deltaTime;
            float persent = Math.Clamp(currentTime/2.0f, 0, 1f);
            int coin = Math.Clamp((int)Mathf.Lerp(start, des, persent), 0, _plusMoney);
            GetText((int)Texts.CoinText).text = $"+{coin} Coin";
            yield return null;
        }
    }

    IEnumerator coDiaObtain()
    {
        int start = 0;
        int des = _plusDia;

        float currentTime = 0;

        Managers.Player.MyPlayer._gameDiamond += _plusDia;
        while (currentTime < 2.0f)
        {
            currentTime += Time.deltaTime;
            float persent = Math.Clamp(currentTime / 2.0f, 0, 1f);
            int dia = Math.Clamp((int)Mathf.Lerp(start, des, persent), 0, _plusMoney);
            GetText((int)Texts.DiamondText).text = $"+{dia} Dia";
            yield return null;
        }
    }

    IEnumerator coCameraAnimation()
    {
        GameObject myPlayerObject = Managers.Game.GetPlayerObject(Managers.Player.GetMyCGUID());

        Vector3 desPos = new Vector3(myPlayerObject.transform.position.x, myPlayerObject.transform.position.y, -10) ;
        Vector3 startPos = Camera.main.transform.position;

        //========µµÀü=======================================
        Vector3 dir = (desPos - startPos).normalized;
        float distance = (desPos - startPos).magnitude;

        float runTime = 0;

        while (runTime < 3f)
        {
            runTime += Time.deltaTime;
            Camera.main.transform.position += dir * (distance * (Time.deltaTime / 3f));
            yield return null;

        }
        Camera.main.transform.GetComponent<CameraController>().setTargetPoint(myPlayerObject);

        //===========================================
        /*
        float runTime = 0;

        while (runTime < 3f)
        {
            runTime += Time.deltaTime;
            Camera.main.transform.position = Vector3.Lerp(startPos, desPos, runTime / 3f);
            yield return null;
            
        }
        Camera.main.transform.GetComponent<CameraController>().setTargetPoint(myPlayerObject);
        */
    }

    IEnumerator coStart()
    {
        yield return new WaitForSeconds(0.5f);
        GetText((int)Texts.MiddleNoticeText).text = 3.ToString();
        yield return new WaitForSeconds(1f);
        GetText((int)Texts.MiddleNoticeText).text = 2.ToString();
        yield return new WaitForSeconds(1f);
        GetText((int)Texts.MiddleNoticeText).text = 1.ToString();
        yield return new WaitForSeconds(1f);
        GetText((int)Texts.MiddleNoticeText).text = "Survive and Be the last persion!!";
        yield return new WaitForSeconds(1f);
        GetText((int)Texts.MiddleNoticeText).gameObject.SetActive(false);
    }

    public void GiveReward(int rank)
    {
        _plusMoney = 400 - (100 * (rank - 1));
        _plusDia =  40 - (10 * (rank - 1)); 
        _plusExp = 100 - (10 * (rank - 1));
    }
}

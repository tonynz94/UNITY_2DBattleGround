using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI_InGame : UI_Scene
{
    enum Buttons
    {
        ToResultButton,
        ToLobbyButton,
    }

    enum Texts
    {
        SurvivorText,
        ResultRankText,
    }

    enum GameObjects
    {
        InGameObject,
        LoserObject,
        WinnerObject,
        KillAndDeathBoardObject,

        ResultObject,
    }


    public void OnEnable()
    {
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

        BindEvent(GetButton((int)Buttons.ToResultButton).gameObject, OnToResultButton);
        BindEvent(GetButton((int)Buttons.ToLobbyButton).gameObject, OnToLobbyButton);

        return true;
    }

    private void Start()
    {
        GetObject((int)GameObjects.WinnerObject).SetActive(false);
        GetObject((int)GameObjects.LoserObject).SetActive(false);
        GetObject((int)GameObjects.ResultObject).SetActive(false);

        GetText((int)Texts.SurvivorText).text = string.Format("Suvivor : {0}", Managers.Game.GetPlayerCount());
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
        GetObject((int)GameObjects.WinnerObject).SetActive(true);
        GetObject((int)GameObjects.LoserObject).SetActive(false);
    }

    public void OnDeathMessage(object obj)
    {
        GetObject((int)GameObjects.WinnerObject).SetActive(false);
        GetObject((int)GameObjects.LoserObject).SetActive(true);
    }

    public void OnToResultButton()
    {
        GetObject((int)GameObjects.WinnerObject).SetActive(false);
        GetObject((int)GameObjects.LoserObject).SetActive(false);
        GetObject((int)GameObjects.ResultObject).SetActive(true);
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
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//내 플레이어대한 모든 정보
public class Player
{
    public int _CGUID;
    public string _nickName;

    public int _level;
    public int _totalExp;
    public int _currentExp;
    public int _profileImage;
    public int _maxHP;
    public int _gameMoney;
    public int _gameDiamond;

    public bool _isInGameRoom;
    public bool _isGameOwner;
    public bool _isPlayerReady;

    public Player(int CGUID, string nickName)
    {
        _CGUID = CGUID;
        _nickName = nickName;

        _level = 1;
        _totalExp = 0;
        _currentExp = 0;   
        _profileImage = 1;
        _maxHP = 300;

        _gameMoney = 10000;
        _gameDiamond = 300;

        _isInGameRoom = false;
        _isGameOwner = false;
        _isPlayerReady = false;
    }

    public string NickName
    {
        get { return _nickName; }
        set { _nickName = value; }
    }

    public int Level
    {
        get { return _level; }
        set { _level = value; }
    }

    public string GetLevel()
    {
        return $"Level : {_level}";
    }
}


public class PlayerManager
{
    //내 플레이어 정보
    public Player MyPlayer;
    //게임 안에 있는 모든 플레이어 
    Dictionary<int, Player> _players = new Dictionary<int, Player>();

    public void AddMyPlayer(int CGUID, string nickName)
    {
        MyPlayer = new Player(CGUID, nickName);
        AddPlayer(MyPlayer);
    }

    public void AddPlayer(int CGUID, string nickName, int level = 0)
    {
        Player player = new Player(CGUID, nickName);
        _players.Add(CGUID, player);
    }

    public void AddPlayer(Player player)
    {
        _players.Add(player._CGUID, player);
    }

    public int GetMyCGUID()
    {
        return MyPlayer._CGUID;
    }

    public string GetMyNick()
    {
        return MyPlayer.NickName;
    }

    public Player GetPlayer(int CGUID)
    {
        Player player;
        _players.TryGetValue(CGUID, out player);
        return player;
    }

    public string GetPlayerNick(int CGUID)
    {
        Player player;
        _players.TryGetValue(CGUID, out player);
        return player.NickName;
    }



}

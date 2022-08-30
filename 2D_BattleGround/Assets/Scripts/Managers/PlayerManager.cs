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

    //Skill
    public int _skillPoint;
    public int _SpeedUpSkillCount;
    public int _RangeUpSkillCount;
    public int _PowerUpSkillCount;
    public int _WaterCountUpSkillCount;

    public bool IsPlayerReady;

    public bool _isPlayerReady
    {
        get
        {
            return IsPlayerReady;
        }
        set
        {
            IsPlayerReady = value;
        }
    }

    public Player(int CGUID, string nickName)
    {
        _CGUID = CGUID;
        _nickName = nickName;

        _level = 1;
        _totalExp = 200;
        _currentExp = 0;
        _profileImage = 1;
        _maxHP = 4000;

        _SpeedUpSkillCount = 0;
        _RangeUpSkillCount = 0;
        _PowerUpSkillCount = 0;
        _WaterCountUpSkillCount = 0;

        _gameMoney = 10000;
        _gameDiamond = 300;
        _skillPoint = 0;

        _isPlayerReady = false;
    }

    public void LeaveFromGame()
    {
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

    public Player GetMyPlayer()
    {
        return GetPlayer(GetMyCGUID());
    }

    public Player GetPlayer(int CGUID)
    {
        Player player;
        _players.TryGetValue(CGUID, out player);
        return player;
    }

    //public Player GetMyRoomID()
    //{
    //    //GetMyPlayer().ro
    //}

    public string GetPlayerNick(int CGUID)
    {
        Player player;
        _players.TryGetValue(CGUID, out player);
        return player.NickName;
    }




}

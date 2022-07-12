using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//내 플레이어대한 모든 정보
public class Player
{
    public int _CGUID;
    public int _level;
    public int _totalExp;
    public int _currentExp;
    public string _nickName;
    public int _profileImage;
    public int _maxHP;

    public int _gameMoney;
    public int _gameDiamond;

    public Player(int CGUID, string nickName)
    {
        _nickName = nickName;
        _CGUID = CGUID;

        _level = 1;
        _totalExp = 0;
        _currentExp = 0;   
        _profileImage = 1;
        _maxHP = 300;

        _gameMoney = 10000;
        _gameDiamond = 300;
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

    //게임중인 플레이어 정보
    Dictionary<int, Player> _ingamePlayers = new Dictionary<int, Player>();

    public void AddMyPlayer(int CGUID, string nickName)
    {
        MyPlayer = new Player(CGUID, nickName);
    }

}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//내 플레이어대한 모든 정보
public class PlayerManager
{
    public int _level;
    public int _totalExp;
    public int _currentExp;
    public string _nickName;
    public int _profileImage;
    public int _maxHP;

    public int _gameMoney;
    public int _gameDiamond;

    public void NewGame(string nickName)
    {
        _level = 1;
        _totalExp = 0;
        _currentExp = 0;
        _nickName = nickName;
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

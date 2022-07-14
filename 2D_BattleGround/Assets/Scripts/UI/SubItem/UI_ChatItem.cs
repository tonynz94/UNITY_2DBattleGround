using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI_ChatItem : UI_Base
{
    enum Images
    {
        ChatTypeImage
    }

    enum Texts
    { 

        ChatTypeText,
        ChatNickText,
        ChatItemText,
    }

    public override bool Init()
    {
        if (base.Init() == false)
            return false;
    
        BindText(typeof(Texts));
        BindImage(typeof(Images));

        return true;
    }

    public void SetChatPiece(Define.ChatType chatType, string nickText, string chatText)
    {
        switch (chatType)
        {
            case Define.ChatType.System:
                GetImage((int)Images.ChatTypeImage).color = Color.red;
                GetText((int)Texts.ChatTypeText).text = "System";
                break;

            case Define.ChatType.Channel:
                GetImage((int)Images.ChatTypeImage).color = Color.yellow;
                GetText((int)Texts.ChatTypeText).text = "Channel";
                break;
        }

        GetText((int)Texts.ChatNickText).text = nickText;
        GetText((int)Texts.ChatItemText).text = chatText;
    }
}

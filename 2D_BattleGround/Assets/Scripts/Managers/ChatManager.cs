using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using static Define;

class ChatPiece
{
    public Define.ChatType _chatType;
    public string _nickName;
    public string _chatContent;

    public ChatPiece(Define.ChatType chatType, string nickName, string chatContent)
    {
        _chatType = chatType;
        _nickName = nickName;
        _chatContent = chatContent;
    }
}

public class ChatManager
{
    Queue<ChatPiece> _chatFieldList = new Queue<ChatPiece>();
    public void chatAdd(Define.ChatType chatType, string nickName, string chatContent)
    {
        MessageSystem.CallEventMessage((int)MESSAGE_EVENT_TYPE.MESS_CHATTING_ADD, new ChatPiece(chatType, nickName, chatContent));
    }

    public void AllNotice(string chatContent)
    {
        Define.ChatType chatType = ChatType.AllNotices;
        MessageSystem.CallEventMessage((int)MESSAGE_EVENT_TYPE.MESS_ALLNOTICE_ADD);
    }

    public int GetChatCount()
    {
        return _chatFieldList.Count;
    }



}

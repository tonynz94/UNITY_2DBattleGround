using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using static Define;

class ChatPiece
{
    Define.ChatType _chatType;
    string _nickName;
    string _chatContent;

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
    int _chatCount;
    public void chatAdd(Define.ChatType chatType, string nickName, string chatContent)
    {
        _chatFieldList.Enqueue(new ChatPiece(chatType, nickName, chatContent));
        MessageSystem.CallEventMessage((int)MESSAGE_EVENT_TYPE.MESS_CHATTING_ADD);
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

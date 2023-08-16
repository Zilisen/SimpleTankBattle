using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class testClient : MonoBehaviour
{
    [SerializeField] private InputField idInput;
    [SerializeField] private InputField pwInput;
    [SerializeField] private InputField textInput;

    //开始
    void Start()
    {
        NetManager.AddEventListener(NetEvent.ConnectSuccess,
OnConnectSucc);
        NetManager.AddEventListener(NetEvent.ConnectFail,
OnConnectFail);
        NetManager.AddEventListener(NetEvent.Close, OnConnectClose);

        //NetManager.AddMsgListener("MsgMove", OnMsgMove);
        NetManager.AddMsgListener("MsgRegister", OnMsgRegister);
        NetManager.AddMsgListener("MsgLogin", OnMsgLogin);
        NetManager.AddMsgListener("MsgKick", OnMsgKick);
        NetManager.AddMsgListener("MsgGetText", OnMsgGetText);
        NetManager.AddMsgListener("MsgSaveText", OnMsgSaveText);
    }

    //玩家点击连接按钮
    public void OnConnectClick()
    {
        NetManager.Connect("127.0.0.1", 8888);
        //TODO：开始转圈圈，提示“连接中”
    }

    //主动关闭
    public void OnCloseClick()
    {
        NetManager.Close();
    }

    //连接成功回调
    void OnConnectSucc(string err)
    {
        Debug.Log("OnConnectSucc");
        //TODO：进入游戏
    }

    //连接失败回调
    void OnConnectFail(string err)
    {
        Debug.Log("OnConnectFail" + err);
        //TODO：弹出提示框(连接失败，请重试）
    }

    //关闭连接
    void OnConnectClose(string err)
    {
        Debug.Log("OnConnectClose");
        //TODO：弹出提示框（网络断开）
        //TODO：弹出按钮（重新连接）
    }

    //玩家点击发送按钮
    public void OnMoveClick()
    {
        MsgMove msg = new MsgMove();
        msg.x = 120;
        msg.y = 123;
        msg.z = -6;
        NetManager.Send(msg);
    }


    //收到MsgMove协议
    public void OnMsgMove(MsgBase msgBase)
    {
        MsgMove msg = (MsgMove)msgBase;
        //消息处理
        Debug.Log("OnMsgMove msg.x = " + msg.x);
        Debug.Log("OnMsgMove msg.y = " + msg.y);
        Debug.Log("OnMsgMove msg.z = " + msg.z);
    }


    public void Update()
    {
        NetManager.Update();

    }

    //发送注册协议
    public void OnRegisterClick()
    {
        MsgRegister msg = new MsgRegister();
        msg.id = idInput.text;
        msg.pw = pwInput.text;
        NetManager.Send(msg);
    }

    //收到注册协议
    public void OnMsgRegister(MsgBase msgBase)
    {
        MsgRegister msg = (MsgRegister)msgBase;
        if (msg.result == 0)
        {
            Debug.Log("注册成功");
        }
        else
        {
            Debug.Log("注册失败");
        }
    }

    //发送登录协议
    public void OnLoginClick()
    {
        MsgLogin msg = new MsgLogin();
        msg.id = idInput.text;
        msg.pw = pwInput.text;
        NetManager.Send(msg);
    }

    //收到登录协议
    public void OnMsgLogin(MsgBase msgBase)
    {
        MsgLogin msg = (MsgLogin)msgBase;
        if (msg.result == 0)
        {
            Debug.Log("登录成功");
            //请求记事本文本
            MsgGetText msgGetText = new MsgGetText();
            NetManager.Send(msgGetText);
        }
        else
        {
            Debug.Log("登录失败");
        }
    }

    //被踢下线
    void OnMsgKick(MsgBase msgBase)
    {
        Debug.Log("被踢下线");
    }

    //收到记事本文本协议
    public void OnMsgGetText(MsgBase msgBase)
    {
        MsgGetText msg = (MsgGetText)msgBase;
        textInput.text = msg.text;
    }

    //发送保存协议
    public void OnSaveClick()
    {
        MsgSaveText msg = new MsgSaveText();
        msg.text = textInput.text;
        NetManager.Send(msg);
    }

    //收到保存协议
    public void OnMsgSaveText(MsgBase msgBase)
    {
        MsgSaveText msg = (MsgSaveText)msgBase;
        if (msg.result == 0)
        {
            Debug.Log("保存成功");
        }
        else
        {
            Debug.Log("保存失败");
        }
    }

}


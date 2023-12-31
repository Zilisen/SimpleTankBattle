using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LoginPanel : BasePanel
{
    //账号输入框
    private InputField idInput;
    //密码输入框
    private InputField pwInput;
    //登录按钮
    private Button loginBtn;
    //注册按钮
    private Button regBtn;

    //初始化
    public override void OnInit()
    {
        skinPath = "LoginPanel";
        layer = PanelManager.Layer.Panel;
    }

    //显示
    public override void OnShow(params object[] args)
    {
        //寻找组件
        idInput = skin.transform.Find("IdInput").GetComponent<InputField>();
        pwInput = skin.transform.Find("PwInput").GetComponent<InputField>();
        loginBtn = skin.transform.Find("LoginBtn").GetComponent<Button>();
        regBtn = skin.transform.Find("RegisterBtn").GetComponent<Button>();
        //监听
        loginBtn.onClick.AddListener(OnLoginClick);
        regBtn.onClick.AddListener(OnRegClick);

        //网络协议监听
        NetManager.AddMsgListener("MsgLogin", OnMsgLogin);
        //网络事件监听
        NetManager.AddEventListener(NetEvent.ConnectSuccess, OnConnectSucc);
        NetManager.AddEventListener(NetEvent.ConnectFail, OnConnectFail);
        //连接服务器
        NetManager.Connect("127.0.0.1", 8888);
    }

    //关闭
    public override void OnClose()
    {
        //网络协议监听
        NetManager.RemoveMsgListener("MsgLogin", OnMsgLogin);
        //网络事件监听
        NetManager.RemoveEventListener(NetEvent.ConnectSuccess, OnConnectSucc);
        NetManager.RemoveEventListener(NetEvent.ConnectFail, OnConnectFail);
    }

    //连接成功回调
    void OnConnectSucc(string err)
    {
        Debug.Log("OnConnectSucc");
    }

    //连接失败回调
    void OnConnectFail(string err)
    {
        Debug.Log("OnConnectFail");
        PanelManager.Open<TipPanel>(err);
    }

    //当按下登录按钮
    public void OnLoginClick()
    {
        //用户名密码为空
        if (idInput.text == "" || pwInput.text == "")
        {
            PanelManager.Open<TipPanel>("用户名和密码不能为空");
            return;
        }
        //发送
        MsgLogin msgLogin = new MsgLogin();
        msgLogin.id = idInput.text;
        msgLogin.pw = pwInput.text;
        NetManager.Send(msgLogin);
        Debug.Log("OnLoginClick");
    }
    //当按下注册按钮

    public void OnRegClick()
    {
        PanelManager.Open<RegisterPanel>();
    }

    //收到登录协议
    public void OnMsgLogin(MsgBase msgBase)
    {
        MsgLogin msg = (MsgLogin)msgBase;
        if (msg.result == 0)
        {
            Debug.Log("登录成功");
            //进入游戏
            ////添加坦克
            //GameObject tankObj = new GameObject("myTank");
            //CtrlTank ctrlTank = tankObj.AddComponent<CtrlTank>();
            //ctrlTank.Init("tankPrefab");
            ////设置相机
            //tankObj.AddComponent<CameraFollow>();

            GameMain.id = msg.id;
            //打开房间列表界面
            PanelManager.Open<RoomListPanel>();
            //关闭界面
            Close();
        }
        else
        {
            PanelManager.Open<TipPanel>("登录失败");
        }
    }
}


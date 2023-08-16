using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class testTank : MonoBehaviour
{

    // Use this for initialization
    public void Start()
    {
        ////坦克
        //GameObject tankObj = new GameObject("myTank");
        //CtrlTank ctrlTank = tankObj.AddComponent<CtrlTank>();
        //ctrlTank.Init("tankPrefab");
        ////相机
        //tankObj.AddComponent<CameraFollow>();

        //// another tank
        //GameObject tankObj2 = new GameObject("myTank");
        //BaseTank baseTank2 = tankObj2.AddComponent<BaseTank>();
        //baseTank2.Init("tankPrefab");
        //baseTank2.transform.position = new Vector3(0, 1, 30);

        // 测试UI
        PanelManager.Init();
        PanelManager.Open<LoginPanel>();
        PanelManager.Open<TipPanel>("即使引导早已破碎，也请您当上艾尔登之王！");
    }

    public void Update()
    {
        NetManager.Update();
    }
}
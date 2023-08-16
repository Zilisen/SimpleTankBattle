using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SyncTank : BaseTank
{
    //坦克模型、转向速度、移动速度
    //炮塔旋转速度、炮塔、炮管、发射点
    //炮弹Cd时间、上一次发射炮弹的时间、物理


    //预测信息，哪个时间到达哪个位置
    private Vector3 lastPos;
    private Vector3 lastRot;
    private Vector3 forecastPos; // 预测的位置
    private Vector3 forecastRot; // 预测的旋转
    private float forecastTime; // 最近一次收到的位置同步协议的时间

    new void Update()
    {
        base.Update();
        //更新位置
        ForecastUpdate();
    }

    //更新位置
    public void ForecastUpdate()
    {
        //时间
        float t = (Time.time - forecastTime) / CtrlTank.syncInterval;
        t = Mathf.Clamp(t, 0f, 1f);
        //位置
        Vector3 pos = transform.position;
        pos = Vector3.Lerp(pos, forecastPos, t);
        transform.position = pos;
        //旋转
        Quaternion quat = transform.rotation;
        Quaternion forcastQuat = Quaternion.Euler(forecastRot);
        quat = Quaternion.Lerp(quat, forcastQuat, t);
        transform.rotation = quat;
    }

    //重写Init
    public override void Init(string skinPath)
    {
        base.Init(skinPath);
        //不受物理运动影响
        tankRigidbody.constraints = RigidbodyConstraints.FreezeAll;
        tankRigidbody.useGravity = false;
        //初始化预测信息
        lastPos = transform.position;
        lastRot = transform.eulerAngles;
        forecastPos = transform.position;
        forecastRot = transform.eulerAngles;
        forecastTime = Time.time;
    }

    //移动同步
    public void SyncPos(MsgSyncTank msg)
    {
        //预测位置
        Vector3 pos = new Vector3(msg.x, msg.y, msg.z);
        Vector3 rot = new Vector3(msg.ex, msg.ey, msg.ez);
        forecastPos = pos + 2*(pos - lastPos);
        forecastRot = rot + 2*(rot - lastRot);
        //更新
        lastPos = pos;
        lastRot = rot;
        forecastTime = Time.time;
        //炮塔
        Vector3 le = turret.localEulerAngles;
        le.y = msg.turretY;
        turret.localEulerAngles = le;
    }

    //开火
    public void SyncFire(MsgFire msg)
    {
        Bullet bullet = Fire();
        //更新坐标
        Vector3 pos = new Vector3(msg.x, msg.y, msg.z);
        Vector3 rot = new Vector3(msg.ex, msg.ey, msg.ez);
        bullet.transform.position = pos;
        bullet.transform.eulerAngles = rot;
    }
}
    
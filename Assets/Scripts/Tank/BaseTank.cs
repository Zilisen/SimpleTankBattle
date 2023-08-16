using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseTank : MonoBehaviour
{
    //坦克模型
    private GameObject skin;

    // 移动速度
    protected float speed = 20;
    // 转向速度
    protected float steer = 3f;
    // 炮塔旋转速度
    protected float turretSpeed = 30f;

    // 炮弹cd时间
    protected float fireCD = 0.5f;
    protected float lastFireTime = 0;

    //生命值
    public float hp = 100;
    //属于哪一名玩家
    public string id = "";
    //阵营
    public int camp = 0;

    // 炮塔
    protected Transform turret;
    // 炮管
    protected Transform gun;
    // 发射点
    protected Transform firePoint;

    protected Rigidbody tankRigidbody;

    // Use this for initialization
    void Start()
    {
    }

    //初始化
    public virtual void Init(string skinPath)
    {
        GameObject skinRes = ResourceManager.LoadPrefab(skinPath);
        skin = (GameObject)Instantiate(skinRes);
        skin.transform.parent = this.transform;
        skin.transform.localPosition = Vector3.zero;
        skin.transform.localEulerAngles = Vector3.zero;

        // 物理
        tankRigidbody = gameObject.AddComponent<Rigidbody>();
        BoxCollider boxCollider = gameObject.AddComponent<BoxCollider>();
        boxCollider.center = new Vector3(0, 2.5f, 1.47f);
        boxCollider.size = new Vector3(7, 5, 12);

        // 炮塔炮管
        turret = skin.transform.Find("Turret");
        gun = turret.transform.Find("Gun");
        firePoint = gun.transform.Find("FirePoint");
    }

    // Update is called once per frame
    protected void Update()
    {
    }

    //发射炮弹
    public Bullet Fire()
    {
        //已经死亡
        if (IsDie())
        {
            return null;
        }
        //产生炮弹
        GameObject bulletObj = new GameObject("bullet");
        Bullet bullet = bulletObj.AddComponent<Bullet>();
        bullet.Init();
        bullet.tank = this;
        //位置
        bullet.transform.position = firePoint.position;
        bullet.transform.rotation = firePoint.rotation;
        //更新时间
        lastFireTime = Time.time;
        return bullet;
    }

    public bool IsDie()
    {
        return hp <= 0;
    }

    //被攻击
    public void Attacked(float att)
    {
        //已经死亡
        if (IsDie())
        {
            return;
        }
        //扣血
        hp -= att;
        Debug.Log("Attacked, hp remain: " + hp);
        //死亡
        if (IsDie())
        {
            //显示焚烧效果
            GameObject obj = ResourceManager.LoadPrefab("FireDeath");
            GameObject explosion = Instantiate(obj, turret.position, turret.rotation);
            explosion.transform.SetParent(transform);
        }
    }
}
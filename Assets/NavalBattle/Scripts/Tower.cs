using UnityEngine;
using System.Collections;

public class Tower : MonoBehaviour {
    // 攻击范围
    public float attackArea = 3.0f;
    // 攻击力
    public int attackPower = 2;
    // 攻击时间间隔
    public float attackInterval = 2.0f;
    // 目标敌人
    protected Enemy targetEnemy;
    // 是否已经面向敌人
    protected bool isFaceEnemy;
    // 炮口与炮弹
    [SerializeField]
    private Transform muzzle;
    [SerializeField]
    private GameObject projectile;
    protected Animator animator;

    private void Start() 
    {
        animator = this.GetComponent<Animator>();
        StartCoroutine(Attack()); // 执行攻击逻辑
    }

    private void Update() {
        FindEnemy();
        RotateTo();
        Attack();
    }

    // 查找目标敌人
    void FindEnemy()
    {
        if (targetEnemy != null)
            return;
        targetEnemy = null;
        int minlife = 0; // 最低的生命值
        foreach (Enemy enemy in NavalController.Instance.enemyList) // 遍历敌人
        {
            if (enemy.m_life == 0)
                continue;
            Vector3 pos1 = this.transform.position; pos1.y = 0;
            Vector3 pos2 = enemy.transform.position; pos2.y = 0;
            // 计算与敌人的距离
            float dist = Vector3.Distance(pos1, pos2);
            // 如果距离超过攻击范
            if (dist > attackArea)
                continue;
            // 查找生命值最低的敌人
            if (minlife == 0 || minlife > enemy.m_life)
            {
                targetEnemy = enemy;
                minlife = enemy.m_life;
            }
        }
    }

    public void RotateTo()
    {
        if (targetEnemy == null)
        {
            this.transform.rotation = Quaternion.identity;
            // this.transform.rotation = Quaternion.Euler(new Vector3(0, 90, 0));
            return;
        }

        var targetdir = targetEnemy.transform.position - transform.position;
        targetdir.y = 0; // 保证仅旋转Y轴
        // 获取旋转方向
        Vector3 rot_delta = Vector3.RotateTowards(this.transform.forward, targetdir, 20.0f * Time.deltaTime, 0.0F);
        Quaternion targetrotation = Quaternion.LookRotation(rot_delta);

        // 计算当前方向与目标之间的角度
        float angle = Vector3.Angle(targetdir, transform.forward);
        // 如果已经面向敌人
        if (angle < 1.0f)
        {
            isFaceEnemy = true;
        }
        else
            isFaceEnemy = false;

        transform.rotation = targetrotation;
    }



    // [SerializeField]
    // private float fireSpeed = 3f;
    // private float fireCounter = 0f;
    // private bool canFire = true;



    // private bool isLockedOn = true;

    // public bool LockedOn {
    //     get { return isLockedOn; }
    //     set { isLockedOn = value; }
    // }

    // private void Update() {
    //     Debug.Log(LockedOn.ToString() + "------" + canFire.ToString());
    //     if (LockedOn && canFire) {
    //         StartCoroutine(Fire());
    //     }
    // }

    // private void OnTriggerEnter(Collider other) {
    //     if (other.tag == "Enemy") {
    //         animator.SetBool("TankInRange", true);
    //     }
    // }

    // private void OnTriggerExit(Collider other) {
    //     if (other.tag == "Enemy") {
    //         animator.SetBool("TankInRange", false);
    //     }
    // }

    private void FireProjectile() {
        GameObject bullet = Instantiate(projectile, muzzle.position, muzzle.rotation) as GameObject;
        bullet.GetComponent<Rigidbody>().AddForce(muzzle.forward * 700);
    }

    // private IEnumerator Fire() {
    //     canFire = false;
    //     FireProjectile();
    //     while (fireCounter < fireSpeed) {
    //         fireCounter += Time.deltaTime;
    //         yield return null;
    //     }
    //     canFire = true;
    //     fireCounter = 0f;
    // }



    // 攻击逻辑
    protected virtual IEnumerator Attack()
    {
        while (targetEnemy == null || !isFaceEnemy) // 如果没有目标一直等待
            yield return 0;
        // animator.CrossFade("attack", 0.1f); // 播放攻击动画

        // while (!animator.GetCurrentAnimatorStateInfo(0).IsName("attack")) // 等待进入攻击动画
        //     yield return 0;
        // float ani_lenght = animator.GetCurrentAnimatorStateInfo(0).length; // 获得攻击动画时间长度
        // yield return new WaitForSeconds(ani_lenght * 0.5f); // 等待完成攻击动作
        // if (targetEnemy != null)
        //     targetEnemy.SetDamage(attackPower); // 攻击
        // yield return new WaitForSeconds(ani_lenght * 0.5f); // 等待播放剩余的攻击动画
        // animator.CrossFade("idle", 0.1f); // 播放待机动画

        FireProjectile();
        yield return new WaitForSeconds(attackInterval*0.5f); // 间隔一定时间
        
        if(targetEnemy != null)
        {
            targetEnemy.SetDamage(attackPower);
        }
        yield return new WaitForSeconds(attackInterval); // 间隔一定时间

        StartCoroutine(Attack()); // 下一轮攻击
    }
}

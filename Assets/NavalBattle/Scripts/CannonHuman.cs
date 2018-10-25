using System.Collections;
using UnityEngine;

public class CannonHuman : MonoBehaviour {
    // 攻击范围
    public float attackArea = 18.0f;
    // 攻击力
    public int attackPower = 4;
    // 攻击时间间隔
    public float attackInterval = 1.0f;
    private float attackTimer = 1.0f;
    private bool canAttack = false;
    // 目标敌人
    protected EnemyUnit targetEnemy;
    // 是否已经面向敌人
    protected bool isFaceEnemy;
    // 炮口与炮弹
    [SerializeField]
    private Transform muzzle;
    [SerializeField]
    private GameObject projectile;

    private void Update () {
        FindEnemy ();
        RotateTo ();

        // 控制敌船受伤频率
        attackTimer -= Time.deltaTime;
        if (attackTimer <= 0) {
            attackTimer = 1.0f;
            canAttack = true;
        }
    }

    // 查找目标敌人
    void FindEnemy () {
        if (targetEnemy != null)
            return;
        targetEnemy = null;
        int minlife = 0; // 最低的生命值
        foreach (EnemyUnit enemy in NavalController.Instance.enemyList) // 遍历敌人
        {
            if (enemy.m_life == 0)
                continue;
            Vector3 pos1 = this.transform.position;
            pos1.y = 0;
            Vector3 pos2 = enemy.transform.position;
            pos2.y = 0;
            // 计算与敌人的距离
            float dist = Vector3.Distance (pos1, pos2);
            // 如果距离超过攻击范
            if (dist > attackArea)
                continue;
            // 查找生命值最低的敌人
            if (minlife == 0 || minlife > enemy.m_life) {
                targetEnemy = enemy;
                minlife = enemy.m_life;
            }
        }
    }

    public void RotateTo () {
        if (targetEnemy == null) {
            this.transform.rotation = Quaternion.identity;
            return;
        }

        var targetdir = targetEnemy.transform.position - transform.position;
        targetdir.y = 0; // 保证仅旋转Y轴
        // 获取旋转方向
        Vector3 rot_delta = Vector3.RotateTowards (this.transform.forward, targetdir, 20.0f * Time.deltaTime, 0.0F);
        Quaternion targetrotation = Quaternion.LookRotation (rot_delta);

        // 计算当前方向与目标之间的角度
        float angle = Vector3.Angle (targetdir, transform.forward);
        // 如果已经面向敌人
        if (angle < 1.0f) {
            isFaceEnemy = true;
        } else
            isFaceEnemy = false;

        transform.rotation = targetrotation;
    }

    private void FireProjectile () {
        GameObject bullet = Instantiate (projectile, muzzle.position, muzzle.rotation) as GameObject;
        bullet.GetComponent<Rigidbody> ().AddForce (muzzle.forward * 900.0f);
    }

    public void Attack () {
        // if (targetEnemy == null || !isFaceEnemy || !canAttack) // 如果没有目标一直等待
        //     return;

        if (targetEnemy != null && isFaceEnemy && canAttack) {

            canAttack = false;
            FireProjectile ();
            if (targetEnemy != null) {
                targetEnemy.SetDamage (attackPower);
            }
        }

    }
}
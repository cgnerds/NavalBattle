using System.Collections;
using HWRWeaponSystem;
using UnityEngine;

public class Projectile : MonoBehaviour {

    [SerializeField]
    private GameObject explosionPrefab;
    public ObjectPool objPool;

    private EnemyUnit enemy;
    public int attackPower = 4;

    private void Awake () {
        objPool = this.GetComponent<ObjectPool> ();
    }

    void Start () { }

    private void OnTriggerEnter (Collider other) {
        Debug.Log(other.name);

        if (other.tag == "Enemy" || other.tag == "Tile") {
            GameObject obj = (GameObject) Instantiate (explosionPrefab, transform.position, transform.rotation);
            enemy = other.transform.GetComponent<EnemyUnit> ();

            if (enemy != null) {
                enemy.SetDamage (attackPower);
            }
            Destroy (obj, 3);
        }

        Destroy (this.gameObject);
    }
}
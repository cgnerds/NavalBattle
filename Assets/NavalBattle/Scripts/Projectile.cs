using System.Collections;
using HWRWeaponSystem;
using UnityEngine;

public class Projectile : MonoBehaviour {

    [SerializeField]
    private GameObject explosionPrefab;
    public ObjectPool objPool;

    private void Awake () {
        objPool = this.GetComponent<ObjectPool> ();
        Debug.Log(objPool.name);
    }

    void Start () { }

    private void OnTriggerEnter (Collider other) {

        // if (objPool && !objPool.Active && WeaponSystem.Pool != null) {
        //     Debug.Log(objPool.name);
        //     return;
        // }

        if (other.tag == "Enemy" || other.tag == "Tile") {
            if (explosionPrefab) {
                if (WeaponSystem.Pool != null) {
                    WeaponSystem.Pool.Instantiate (explosionPrefab, transform.position, transform.rotation, 3);
                } else {
                    GameObject obj = (GameObject) Instantiate (explosionPrefab, transform.position, transform.rotation);
                    Destroy (obj, 3);
                }
            }
            
            Destroy(this.gameObject);
        }
    }

}
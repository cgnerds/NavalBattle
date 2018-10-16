﻿using System.Collections;
using HWRWeaponSystem;
using UnityEngine;

public class Projectile : MonoBehaviour {

    [SerializeField]
    private GameObject explosionPrefab;
    public ObjectPool objPool;

    private void Awake () {
        objPool = this.GetComponent<ObjectPool> ();
    }

    void Start () { }

    private void OnTriggerEnter (Collider other) {

        if (objPool && !objPool.Active && WeaponSystem.Pool != null) {
            Debug.Log(objPool.Active);
            return;
        }

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

        // if (other.tag == "Enemy" || other.tag == "Tile") {
        //     if (explosionPrefab == null) {
        //         return;
        //     }
        //     GameObject explosion = Instantiate (explosionPrefab, transform.position, Quaternion.identity) as GameObject;
        //     Destroy (this.gameObject);
        // }
    }

}
using UnityEngine;
using System.Collections;

public class Projectile : MonoBehaviour {

    [SerializeField]
    private GameObject explosionPrefab;

	void Start () {	}

    private void OnTriggerEnter(Collider other) {
        Debug.Log(other.name);
        if (other.tag == "Enemy" || other.tag == "Tile") {
            if (explosionPrefab == null) {
                return;
            }
            GameObject explosion = Instantiate(explosionPrefab, transform.position, Quaternion.identity) as GameObject;
            Destroy(this.gameObject);            
        }
    }
	
}

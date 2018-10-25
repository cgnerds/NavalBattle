using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponMove : MonoBehaviour {
	public Vector3 StartPos;
	public Vector3 EndPos;
	public float moveSpeed = 5.0f;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		Vector3 pos1 = this.transform.position;
		float dist = Vector3.Distance(pos1, EndPos);
		if(dist < 1.0f)
		{
			NavalController.Instance.SetDamage (-1);
			this.transform.position = StartPos;
		}
		this.transform.Translate(new Vector3(0, 0, moveSpeed * Time.deltaTime));
	}
}

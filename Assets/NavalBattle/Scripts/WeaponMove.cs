using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponMove : MonoBehaviour {
	public Vector3 StartPos;
	public Vector3 EndPos;
	public float moveSpeed = 2.0f;
	public float minDist;

	// public GameObject cannon;
	public string cannonName;
	private CannonHuman cannonHuman;

	// Use this for initialization
	void Start () {
		cannonHuman = (CannonHuman)GameObject.Find(cannonName).GetComponent(typeof(CannonHuman));
	}
	
	// Update is called once per frame
	void Update () {
		Vector3 pos1 = this.transform.position;
		float dist = Vector3.Distance(pos1, EndPos);
		if(cannonHuman!=null && dist <= minDist)
		{
			cannonHuman.ChangeBulletCount(1);
			this.transform.position = StartPos;
		}
		this.transform.Translate(new Vector3(0, 0, moveSpeed * Time.deltaTime));
	}
}

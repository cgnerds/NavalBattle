using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour {

	public int m_life = 15;
	public int m_maxlife = 15;
	public System.Action<Enemy> OnDeath;

	// life bar
	public GameObject lifebarFab;
	private Transform lifebarObj;
	private UnityEngine.UI.Slider lifebarSlider;


	// Use this for initialization
	void Start () {
		lifebarObj = ((GameObject)Instantiate(lifebarFab, Vector3.zero, Camera.main.transform.rotation, this.transform)).transform;
		lifebarObj.localPosition = new Vector3(0, 1.25f, 0);
		lifebarObj.localScale = new Vector3(0.015f, 0.015f, 0.015f);
		lifebarSlider = lifebarObj.GetComponentInChildren<UnityEngine.UI.Slider>();
		StartCoroutine(UpdateLifebar());		
	}

	IEnumerator UpdateLifebar()
	{
		lifebarSlider.value = (float)m_life/(float)m_maxlife;
		lifebarObj.transform.eulerAngles = Camera.main.transform.eulerAngles;
		yield return 0;
		StartCoroutine(UpdateLifebar());		
	}

	public void SetDamage(int damage)
	{
		m_life -= damage;
		if(m_life <= 0)
		{
			m_life = 0;
			Destroy(this.gameObject);
		}
	}
	
	// Update is called once per frame
	void Update () {
		
	}

}

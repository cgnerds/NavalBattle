using System.Collections;
using System.Collections.Generic;
using HWRWeaponSystem;
using MapNavKit;
using UnityEngine;

public class EnemyUnit : MonoBehaviour {
	public float moveSpeed = 2.0f;

	public NavalTile tile { get; set; } // the tile that this unit is on
	public int movesLeft { get; set; } // each unit can move up to 3 tiles per turn

	private bool moving = false;
	private List<MapNavNode> path = null;
	private int nextPathIdx = 0;
	private Vector3 targetPos = Vector3.zero;
	private float journeyLength;
	private float startTime;

	// 到达目标位置回调
	public delegate void OnMoveCompleted ();
	private OnMoveCompleted onMoveCompleted = null;

	// 生命值
	public int m_life = 20;
	public int m_maxlife = 20;
	// 鱼死亡回调
	public delegate void EnemyDeathDelegate (EnemyUnit enemy);
	public EnemyDeathDelegate OnDeath;
	// 血条
	public GameObject lifebarFab;
	private Transform lifebarObj;
	private UnityEngine.UI.Slider lifebarSlider;
	// 敌船导弹系统
	public WeaponController weapon;
	private float attackInterval = 2.0f;
	private float damageTimer = 0.5f;
	private bool canDamage = false;
	// 受到攻击后材质变色并发声
	public GameObject ShipMesh;
	public AudioSource audioSource;
	public AudioClip attaked;
	// 船帆颜色
	public GameObject fanPart;
	private Color fanColor;


	// ------------------------------------------------------------------------------------------------------------
	private void Start () {
		lifebarObj = ((GameObject) Instantiate (lifebarFab, Vector3.zero, Camera.main.transform.rotation, this.transform)).transform;
		lifebarObj.localPosition = new Vector3 (-1.5f, 4.0f, -3.5f);
		lifebarObj.localScale = new Vector3 (0.015f, 0.015f, 0.015f);
		lifebarSlider = lifebarObj.GetComponentInChildren<UnityEngine.UI.Slider> ();
		StartCoroutine (UpdateLifebar ());
		// 敌船导弹系统
		weapon = this.transform.GetComponent<WeaponController> ();
		// 声音
		if (!audioSource) {
			audioSource = this.GetComponent<AudioSource> ();
			if (!audioSource) {
				this.gameObject.AddComponent<AudioSource> ();
			}
		}

		// 船帆随机颜色
		float r = Random.Range(0.0f, 1.0f);
		float g = Random.Range(0.0f, 1.0f);
		float b = Random.Range(0.0f, 1.0f);
		fanColor = new Color(r, g, b, 1.0f);
		fanPart.GetComponent<Renderer>().material.color = fanColor;
	}

	IEnumerator UpdateLifebar () {
		lifebarSlider.value = (float) m_life / (float) m_maxlife;
		lifebarObj.transform.eulerAngles = Camera.main.transform.eulerAngles;
		yield return 0;
		StartCoroutine (UpdateLifebar ());
	}

	public void SetDamage (int damage) {
		if (canDamage) {
			m_life -= damage;
			canDamage = false;
			StartCoroutine (ChangeColor ());
		}
		if (m_life <= 0) {
			m_life = 0;
			OnDeath (this);
			// 取消敌船与地图网格的关联
			LinkWithTile (null);
			NavalTile last = path[path.Count - 1] as NavalTile;
			last.target = false;

			Destroy (this.gameObject);
		}
	}

	IEnumerator ChangeColor () {
		if (audioSource) {
			audioSource.PlayOneShot (attaked);
		}

		// 敌船变红色
		foreach (MeshRenderer part in ShipMesh.GetComponentsInChildren<MeshRenderer> ()) {
			part.GetComponent<Renderer> ().material.color = Color.red;
		}
		yield return new WaitForSeconds (0.1f);
		// 敌船恢复颜色
		foreach (MeshRenderer part in ShipMesh.GetComponentsInChildren<MeshRenderer> ()) {
			part.GetComponent<Renderer> ().material.color = Color.white;
		}
		fanPart.GetComponent<Renderer>().material.color = fanColor;
	}

	/// <summary>
	/// Call this at "end of turn" to reset the unit, like how many moves it got left
	/// </summary>
	public void Resetunit () {
		movesLeft = 25;
	}

	/// <summary>
	/// I use this to link the tile and unit with each other so that the
	/// tile and unit knows which unit is on the tile. I will pass null
	/// to simply unlink the tile and unit.
	/// </summary>
	public void LinkWithTile (NavalTile t) {
		// first unlink the unit from the tile
		if (tile != null) tile.unit = null;
		tile = t;

		// if t == null then this was simply an unlink and it ends here
		if (tile == null) return;

		// else tell the tile that this unit is on it
		tile.unit = this;
	}

	/// <summary>
	/// Change the colour of the unit when it is selected
	/// </summary>
	public void UnitSelected () {
		transform.GetChild (0).GetComponent<Renderer> ().material.color = Color.green;
	}

	/// <summary>
	/// Change the colour of the unit when it is de-selected
	/// </summary>
	public void UnitDeSelected () {
		transform.GetChild (0).GetComponent<Renderer> ().material.color = new Color (0f, 0.36f, 0.51f, 1f);
	}

	IEnumerator AttackWall () {
		if (weapon) {
			weapon.LaunchWeapon ();
			yield return new WaitForSeconds (attackInterval * 0.5f); // 间隔一定时间
			NavalController.Instance.SetDamage (1);
		}
		yield return new WaitForSeconds (attackInterval * 0.5f); // 间隔一定时间

		StartCoroutine (AttackWall ()); // 下一轮攻击
	}

	/// <summary>
	/// start moving the unit form node to node
	/// </summary>
	public void Move (List<MapNavNode> path, OnMoveCompleted callback) {
		if (path.Count == 0) {
			//** callback();
			AttackWall ();
			return;
		}

		this.onMoveCompleted = callback;
		this.path = path;
		this.moving = true;

		// unlink with tile
		LinkWithTile (null);

		// start moving
		targetPos = path[0].position;
		nextPathIdx = 1;
		journeyLength = Vector3.Distance (transform.position, targetPos);
		startTime = Time.time;
		movesLeft--;
	}

	// ------------------------------------------------------------------------------------------------------------

	protected void Update () {
		if (Input.GetKeyDown (KeyCode.Space)) {
			if (weapon) {
				weapon.LaunchWeapon ();
			}
		}

		// 控制敌船受伤频率
		damageTimer -= Time.deltaTime;
		if (damageTimer <= 0) {
			damageTimer = 0.5f;
			canDamage = true;
		}

		if (moving) {
			float distCovered = (Time.time - startTime) * moveSpeed;
			float fracJourney = distCovered / journeyLength;

			transform.position = Vector3.Lerp (transform.position, targetPos, fracJourney);

			if (transform.position == targetPos) {
				if (nextPathIdx >= path.Count) {
					// reached end of path. link with new tile and tell controller i am done
					moving = false;
					LinkWithTile (path[path.Count - 1] as NavalTile);
					//** onMoveCompleted();
					StartCoroutine (AttackWall ());
					return;
				}

				// go to next node
				targetPos = path[nextPathIdx].position;
				nextPathIdx++;
				journeyLength = Vector3.Distance (transform.position, targetPos);
				startTime = Time.time;
				movesLeft--;
			}
		}
	}

	// ------------------------------------------------------------------------------------------------------------
}

using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using MapNavKit;


public class EnemyUnit : MonoBehaviour 
{
	public float moveSpeed = 2.0f;

	public NavalTile tile { get; set; }	// the tile that this unit is on
	public int movesLeft { get; set; }		// each unit can move up to 3 tiles per turn

	public delegate void OnMoveCompleted();
	private OnMoveCompleted onMoveCompleted = null; // callback to call when this unit is done moving

	private bool moving = false;
	private List<MapNavNode> path = null;
	private int nextPathIdx = 0;
	private Vector3 targetPos = Vector3.zero;
	private float journeyLength;
	private float startTime;

	// 生命值
	public int m_life = 15;
	public int m_maxlife = 15;
	// 鱼死亡回调
	public delegate void EnemyDeathDelegate(EnemyUnit enemy);
	public EnemyDeathDelegate OnDeath;
	// 血条
	public GameObject lifebarFab;
	private Transform lifebarObj;
	private UnityEngine.UI.Slider lifebarSlider;

	// ------------------------------------------------------------------------------------------------------------
	private void Start() {
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
			OnDeath(this);
			// 取消敌船与地图网格的关联
			LinkWithTile(null);
			NavalTile last = path[path.Count - 1] as NavalTile;
			last.target = false;

			Destroy(this.gameObject);
		}
	}

	/// <summary>
	/// Call this at "end of turn" to reset the unit, like how many moves it got left
	/// </summary>
	public void Resetunit()
	{
		movesLeft = 25;
	}

	/// <summary>
	/// I use this to link the tile and unit with each other so that the
	/// tile and unit knows which unit is on the tile. I will pass null
	/// to simply unlink the tile and unit.
	/// </summary>
	public void LinkWithTile(NavalTile t)
	{
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
	public void UnitSelected()
	{
		transform.GetChild(0).GetComponent<Renderer>().material.color = Color.green;
	}

	/// <summary>
	/// Change the colour of the unit when it is de-selected
	/// </summary>
	public void UnitDeSelected()
	{
		transform.GetChild(0).GetComponent<Renderer>().material.color = new Color(0f, 0.36f, 0.51f, 1f);
	}

	/// <summary>
	/// start moving the unit form node to node
	/// </summary>
	public void Move(List<MapNavNode> path, OnMoveCompleted callback)
	{
		if (path.Count == 0)
		{
			//** callback();
			return;
		}

		this.onMoveCompleted = callback;
		this.path = path;
		this.moving = true;

		// unlink with tile
		LinkWithTile(null);

		// start moving
		targetPos = path[0].position;
		nextPathIdx = 1;
		journeyLength = Vector3.Distance(transform.position, targetPos);
		startTime = Time.time;
		movesLeft--;
	}

	// ------------------------------------------------------------------------------------------------------------

	protected void Update()
	{
		if (moving)
		{
			float distCovered = (Time.time - startTime) * moveSpeed;
			float fracJourney = distCovered / journeyLength;

			transform.position = Vector3.Lerp(transform.position, targetPos, fracJourney);

			if (transform.position == targetPos)
			{
				if (nextPathIdx >= path.Count)
				{
					// reached end of path. link with new tile and tell controller i am done
					moving = false;
					LinkWithTile(path[path.Count - 1] as NavalTile);
					//** onMoveCompleted();
					return;
				}

				// go to next node
				targetPos = path[nextPathIdx].position;
				nextPathIdx++;
				journeyLength = Vector3.Distance(transform.position, targetPos);
				startTime = Time.time;
				movesLeft--;
			}
		}
	}

	// ------------------------------------------------------------------------------------------------------------
}

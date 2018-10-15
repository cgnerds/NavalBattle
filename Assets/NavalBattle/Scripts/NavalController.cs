using System.Collections;
using System.Collections.Generic;
using MapNavKit;
using UnityEngine;
using UnityEngine.SceneManagement;

public class NavalController : MonoBehaviour {
	// ------------------------------------------------------------------------------------------------------------
	#region properties
	public static NavalController Instance;
	#endregion
	// ------------------------------------------------------------------------------------------------------------
	#region enemySpawn
	public GameObject enemyFab; // 敌船预制件
	public float spawnTimer = 0; // 生成计时器
	public int maxEnemyCount = 6; // 最大生成数量
	private int curEnemyCount = 0; // 当前敌船数量
	[HideInInspector]
	public List<EnemyUnit> enemyList = new List<EnemyUnit> (); // 船只列表

	#endregion
	// ------------------------------------------------------------------------------------------------------------
	#region runtime

	protected MapNavBase map; // reference to the mapNav grid
	protected LayerMask clickMask = (1 << 8 | 1 << 9); // in this sample layer 8 = tiles collider and layer 10 = unit's collider

	#endregion
	// ------------------------------------------------------------------------------------------------------------
	#region start

	private void Awake () {
		Instance = this;
	}

	protected void Start () {
		// get reference to the mapnav map
		map = GameObject.Find ("Map").GetComponent<NavalMap> ();
	}
	#endregion
	// ------------------------------------------------------------------------------------------------------------
	#region update/ input

	protected void Update () {
		spawnTimer -= Time.deltaTime;
		// 按Esc键退出
		if (Input.GetKeyDown (KeyCode.Escape)) {
			Application.Quit ();
		}

		// 接收触控墙程序发送的字符串
		// 鼠标左键攻击敌船
		if(Input.GetMouseButtonDown(0))
		{
			RaycastHit hit;
			Ray r = Camera.main.ScreenPointToRay(Input.mousePosition);
			if(Physics.Raycast(r, out hit, Mathf.Infinity, clickMask))
			{
				if(hit.transform.gameObject.layer == 9)
				{
					EnemyUnit unit = hit.transform.GetComponent<EnemyUnit>();
					unit.SetDamage(1);
				}
			}
		}

		if (spawnTimer <= 0) {
			// 重新计时
			spawnTimer = 2.0f;
			// 如果敌船的数量达到最大数量则返回
			if (curEnemyCount >= maxEnemyCount || enemyList.Count >= maxEnemyCount) {
				return;
			}
			// 生成敌船
			SpawnEnemy ();
		}
	}

	void SpawnEnemy () {
		// 在六边形网格地图最右侧随机生成敌船
		NavalTile sourceTile = null;

		while (true) {
			int idx = map.mapHorizontalSize * (map.mapVerticalSize - 1) + Random.Range (0, map.mapHorizontalSize);
			// make sure this is a valid node
			if (map.ValidIDX (idx)) {
				// now check if there is not already a unit on it
				sourceTile = map.grid[idx] as NavalTile;
				if (sourceTile.unit == null) {
					// no unit on it, lets use it
					break;
				}
			}
		}

		// create the unit and place it on the tile
		GameObject go = (GameObject) Instantiate (enemyFab);
		go.transform.position = sourceTile.position;
		go.layer = 9; // Units must be in layer 9
		go.tag = "Enemy";

		// be sure to tell the tile that this Unit is on it
		EnemyUnit unit = go.GetComponent<EnemyUnit> ();
		unit.Resetunit (); // reset its moves now too
		unit.LinkWithTile (sourceTile);
		unit.OnDeath += OnDeath;
		enemyList.Add (unit); // keep a list of all enemies for quick access

		// 设置敌船的随机目标
		NavalTile targetTile = null;
		while (true) {
			int idy = map.mapHorizontalSize * 5 + Random.Range (0, map.mapHorizontalSize);
			// set each enemy's targets
			if (map.ValidIDX (idy)) {
				targetTile = map.grid[idy] as NavalTile;
				if (targetTile.target == false) {
					targetTile.target = true;
					break;
				}
			}
		}

		List<MapNavNode> unitPath = map.Path<MapNavNode> (sourceTile, targetTile, OnNodeCostCallback);
		if (unitPath != null) {
			unit.Move (unitPath, null);
		}

		// 更新敌人数量
		curEnemyCount++;
	}

	#endregion
	// ------------------------------------------------------------------------------------------------------------
	#region callbacks

	/// <summary>
	/// This will be called by map.NodesAround to find out if the target node is valid
	/// and how much it will cost to move to that node. In my case I do not have extra
	/// cost to move from tile to tile, so I will simply return 1. The only condition
	/// is that units may not move to tile that are too high/ low; in that case I 
	/// need to return 0.
	/// </summary>
	protected virtual float OnNodeCostCallback (MapNavNode fromNode, MapNavNode toNode) {
		if ((toNode as NavalTile).unit != null) return 0f; // there is a unit on target node
		int heightDiff = Mathf.Abs (fromNode.h - toNode.h);
		if (heightDiff >= 2) return 0f; // too height
		return 1f;
	}

	private void OnDeath (EnemyUnit enemy) {
		// 更新敌船数量
		curEnemyCount--;
		// 将敌船从列表中删除
		enemyList.Remove (enemy);
	}
	#endregion
	// ------------------------------------------------------------------------------------------------------------
}
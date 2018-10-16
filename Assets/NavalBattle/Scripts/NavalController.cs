using System.Collections;
using System.Collections.Generic;
using MapNavKit;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.Text.RegularExpressions;
using System.Diagnostics;

public class NavalController : MonoBehaviour {
	// ------------------------------------------------------------------------------------------------------------
	#region properties
	public static NavalController Instance;
	private int baseLife = 10000; // 基地生命值
	public UnityEngine.UI.Text  baseLifeUI = null;
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
	
	IEnumerator startTouchWall()
	{
		yield return new WaitForSeconds(3.0f);
		// 开启触控墙
		Process.Start("touch.exe");
		yield return new WaitForSeconds(1.0f);
	}

	protected void Start () {
		// get reference to the mapnav map
		map = GameObject.Find ("Map").GetComponent<NavalMap> ();
		StartCoroutine(startTouchWall());
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
		string recvStr = Camera.main.GetComponent<TcpServer>().recvStr;
		// 获取触控墙坐标，mousePosition[0]为触控点数量，mousePosition[1]和mousePosition[2]分别为第一个点的x轴坐标和y坐标，依此类推
		List<float> touchPosition = new List<float>();
		foreach (Match m in Regex.Matches(recvStr, @"\d+"))
            touchPosition.Add(float.Parse(m.Value));
		
		// 触控墙攻击敌船
		if(touchPosition.Count >= 1)
		{
			for(int i = 0; i < touchPosition[0]; i += 1)
			{
				// 触控墙坐标范围默认为1920*1080，需要根据Unity实际分辨率进行缩放                
				Ray ray = Camera.main.ScreenPointToRay(new Vector3(touchPosition[i * 2 + 1] * Screen.width / 1920.0f, 
				                                                   touchPosition[i * 2 + 2] * Screen.height / 1080.0f, 0));
				RaycastHit hit;
				if(Physics.Raycast(ray, out hit, Mathf.Infinity, clickMask))
				{
					if(hit.transform.gameObject.layer == 9)
				    {
					    EnemyUnit unit = hit.transform.GetComponent<EnemyUnit>();
					    unit.SetDamage(1);
				    }
				}
			}
		}
		else if(Input.GetMouseButtonDown(0)) // 否则，鼠标左键攻击敌船
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


	public void SetDamage(int damage)
	{
		baseLife -= damage;
		if(baseLife <= 0)
		{
			baseLife = 10000;
		}
	    baseLifeUI.text = string.Format("生命: <color=yellow>{0}</color>", baseLife);
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
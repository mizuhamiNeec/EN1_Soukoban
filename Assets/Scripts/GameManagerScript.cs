using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManagerScript : MonoBehaviour {
	[SerializeField] private Transform _cam;

	public GameObject playerPrefab;
	public GameObject boxPrefab;
	public GameObject wallPrefab;
	public GameObject goalPrefab;
	public GameObject particlePrefab;
	public GameObject clearText;

	private int[,] map; // レベルデザイン用の配列
	private GameObject[,] field; // ゲーム管理用の配列
	private List<GameObject[,]> fieldList;
	private bool isCleared = false;

	// Start is called before the first frame update
	void OnEnable() {
		Init();
	}

	void Init() {
		Screen.SetResolution(1920, 1080, false);

		// 0 air
		// 1 player
		// 2 box
		// 3 goal
		// 4 wall

		map = new int[,]
		{
			{ 4, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4 },
			{ 4, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 4 },
			{ 4, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 2, 0, 0, 4 },
			{ 4, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 4 },
			{ 4, 0, 0, 0, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 4 },
			{ 4, 0, 0, 0, 0, 0, 0, 0, 3, 0, 0, 0, 0, 0, 0, 4 },
			{ 4, 0, 0, 3, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 4 },
			{ 4, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 2, 0, 0, 4 },
			{ 4, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 4 },
			{ 4, 0, 2, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 4 },
			{ 4, 0, 0, 0, 0, 0, 0, 0, 2, 0, 0, 0, 0, 0, 0, 4 },
			{ 4, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 4 },
			{ 4, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 3, 4 },
			{ 4, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 4 },
			{ 4, 0, 0, 0, 0, 0, 3, 0, 0, 0, 0, 0, 0, 0, 0, 4 },
			{ 4, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4 },
		};

		field = new GameObject[map.GetLength(0), map.GetLength(1)];

		// 配列の中心にカメラを置く
		_cam.position = new Vector3((map.GetLength(1) - 1) * 0.5f, (map.GetLength(0) + 1) * 0.5f, -10);

		string debugText = string.Empty;

		for(int y = 0; y < map.GetLength(0); y++) {
			for(int x = 0; x < map.GetLength(1); x++) {
				if(map[y, x] == 1) {
					field[y, x] = Instantiate(
						playerPrefab,
						new Vector3(x, map.GetLength(0) - y, 0),
						Quaternion.identity
					);
				}

				if(map[y, x] == 2) {
					field[y, x] = Instantiate(
						boxPrefab,
						new Vector3(x, map.GetLength(0) - y, 0),
						Quaternion.identity
					);
				}

				if(map[y, x] == 3) {
					field[y, x] = Instantiate(
						goalPrefab,
						new Vector3(x, map.GetLength(0) - y, 0),
						Quaternion.identity
					);
				}

				if(map[y, x] == 4) {
					field[y, x] = Instantiate(
						wallPrefab,
						new Vector3(x, map.GetLength(0) - y, 0),
						Quaternion.identity
					);
				}
			}
		}
	}

	// Update is called once per frame
	void Update() {
		if(Input.GetKeyDown(KeyCode.W)) {
			IsCleared();

			Vector2Int playerIndex = GetPlayerIndex();
			MoveNumber("Player", playerIndex, new Vector2Int(playerIndex.x, playerIndex.y - 1));

			fieldList.Add(field);
		}

		if(Input.GetKeyDown(KeyCode.S)) {
			IsCleared();

			Vector2Int playerIndex = GetPlayerIndex();
			MoveNumber("Player", playerIndex, new Vector2Int(playerIndex.x, playerIndex.y + 1));
			fieldList.Add(field);
		}

		if(Input.GetKeyDown(KeyCode.D)) {
			IsCleared();

			Vector2Int playerIndex = GetPlayerIndex();
			MoveNumber("Player", playerIndex, new Vector2Int(playerIndex.x + 1, playerIndex.y));
			fieldList.Add(field);
		}

		if(Input.GetKeyDown(KeyCode.A)) {
			IsCleared();

			Vector2Int playerIndex = GetPlayerIndex();
			MoveNumber("Player", playerIndex, new Vector2Int(playerIndex.x - 1, playerIndex.y));
			fieldList.Add(field);
		}

		if(Input.GetKeyDown(KeyCode.Q)) {
			for(int y = 0; y < map.GetLength(0); y++) {
				for(int x = 0; x < map.GetLength(1); x++) {
					if(field[y, x] != null) {
						Destroy(field[y, x]);
						field[y, x] = null;
					}
				}
			}

			field = fieldList[^1];

			for(int y = 0; y < map.GetLength(0); y++) {
				for(int x = 0; x < map.GetLength(1); x++) {
					if(map[y, x] == 1) {
						field[y, x] = Instantiate(
							playerPrefab,
							new Vector3(x, map.GetLength(0) - y, 0),
							Quaternion.identity
						);
					}

					if(map[y, x] == 2) {
						field[y, x] = Instantiate(
							boxPrefab,
							new Vector3(x, map.GetLength(0) - y, 0),
							Quaternion.identity
						);
					}

					if(map[y, x] == 3) {
						field[y, x] = Instantiate(
							goalPrefab,
							new Vector3(x, map.GetLength(0) - y, 0),
							Quaternion.identity
						);
					}

					if(map[y, x] == 4) {
						field[y, x] = Instantiate(
							wallPrefab,
							new Vector3(x, map.GetLength(0) - y, 0),
							Quaternion.identity
						);
					}
				}
			}
		}

		if(IsCleared()) {
			clearText.SetActive(true);

			foreach(var f in field) {
				if(f != null) {
					// Moveコンポーネントがある場合
					if(f.TryGetComponent(out Move move)) {
						// 移動を停止
						move.StopMove();
					}

					if(f.TryGetComponent(out SpawnAnimation spawn)) {
						spawn.DeSpawnAnimation();
					}
				}
			}
		} else if(Input.GetKeyDown(KeyCode.R)) {
			for(int y = 0; y < map.GetLength(0); y++) {
				for(int x = 0; x < map.GetLength(1); x++) {
					if(field[y, x] != null) {
						Destroy(field[y, x]);
						field[y, x] = null;
					}
				}
			}

			Init();
		}
	}

	bool MoveNumber(string tag, Vector2Int moveFrom, Vector2Int moveTo) {
		// プレイヤーの移動先が配列の外の場合
		if(moveTo.x < 0 || moveTo.x >= map.GetLength(1)) {
			// 移動しないでリターン
			return false;
		}

		// プレイヤーの移動先が配列の外の場合
		if(moveTo.y < 0 || moveTo.y >= map.GetLength(0)) {
			// 移動しないでリターン
			return false;
		}

		// 移動先が2(箱)だったら
		if(field[moveTo.y, moveTo.x] != null && field[moveTo.y, moveTo.x].tag == "Box") {
			// どの方向へ移動するかを算出
			Vector2Int velocity = moveTo - moveFrom;
			// プレイヤーの移動先から、更に先へ2(箱)を移動させる。
			// 箱の移動処理。 MoveNumberメソッド内でMoveNumberメソッドを
			// 呼び、処理が再起している。移動不可をboolで記録
			bool success = MoveNumber(tag, moveTo, moveTo + velocity);
			if(!success) {
				return false;
			}
		}

		if(tag == "Player") {
			for(int i = 0; i < 4; i++) {
				Instantiate(
					particlePrefab,
					field[moveFrom.y, moveFrom.x].transform.position,
					Quaternion.identity
				);
			}
		}

		Vector3 newPos = new Vector3(moveTo.x, field.GetLength(0) - moveTo.y, 0);

		//field[moveFrom.y, moveFrom.x].transform.position = newPos;
		field[moveFrom.y, moveFrom.x].GetComponent<Move>().MoveTo(newPos);

		field[moveTo.y, moveTo.x] = field[moveFrom.y, moveFrom.x];
		field[moveFrom.y, moveFrom.x] = null;
		return true;
	}

	Vector2Int GetPlayerIndex() {
		for(int y = 0; y < field.GetLength(0); y++) {
			for(int x = 0; x < field.GetLength(1); x++) {
				if(field[y, x] == null) {
					continue;
				}

				if(field[y, x].tag == "Player") {
					return new Vector2Int(x, y);
				}
			}
		}

		return new Vector2Int(-1, -1);
	}

	bool IsCleared() {
		List<Vector2Int> goals = new List<Vector2Int>();

		for(int y = 0; y < map.GetLength(0); y++) {
			for(int x = 0; x < map.GetLength(1); x++) {
				if(map[y, x] == 3) {
					goals.Add(new Vector2Int(x, y));
				}
			}
		}

		// 要素数はgoals.Countで取得
		foreach(Vector2Int t in goals) {
			GameObject f = field[t.y, t.x];
			if(f == null || f.tag != "Box") {
				return false;
			}
		}

		isCleared = true;

		// 条件未達成でなければ条件達成
		return true;
	}
}
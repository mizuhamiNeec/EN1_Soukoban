using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManagerScript : MonoBehaviour {
	[SerializeField] private Transform _cam;

	public GameObject playerPrefab;
	private int[,] map;

	// Start is called before the first frame update
	void Start() {
		map = new int[,]
		{
			{ 1, 0, 0, 0, 1 },
			{ 0, 0, 1, 0, 0 },
			{ 1, 0, 0, 0, 0 }
		};

		_cam.position = new Vector3((map.GetLength(1) - 1) * 0.5f, (-map.GetLength(0) - 1) * 0.5f, -5);

		string debugText = string.Empty;

		for (int y = 0; y < map.GetLength(0); y++) {
			for (int x = 0; x < map.GetLength(1); x++) {
				if (map[y, x] == 1 || map[y, x] == 0) {
					GameObject instance = Instantiate(
						playerPrefab,
						new Vector3(x, -y, 0),
						Quaternion.identity
					);
				}
			}
		}
	}

	//// Update is called once per frame
	//void Update() {
	//	if (Input.GetKeyDown(KeyCode.RightArrow)) {
	//		int playerIndex = GetPlayerIndex();

	//		MoveNumber(1, playerIndex, playerIndex + 1);

	//		PrintArray();
	//	}

	//	if (Input.GetKeyDown(KeyCode.LeftArrow)) {
	//		int playerIndex = GetPlayerIndex();

	//		MoveNumber(1, playerIndex, playerIndex - 1);

	//		PrintArray();
	//	}
	//}

	//bool MoveNumber(int number, int moveFrom, int moveTo) {
	//	// 移動先が範囲外なら移動不可
	//	if (moveTo < 0 || moveTo >= map.Length) {
	//		return false;
	//	}

	//	// 移動先に2(箱)が居たら
	//	if (map[moveTo] == 2) {
	//		// どの方向へ移動するかを算出
	//		int velocity = moveTo - moveFrom;
	//		// プレイヤーの移動先から、さらに先へ2(箱)を移動させる。
	//		// 箱の移動処理。MoveNumberメソッド内でMoveNumberメソッドを
	//		// 呼び、処理が再起している。移動不可をboolで記録
	//		bool success = MoveNumber(2, moveTo, moveTo + velocity);
	//		if (!success) {
	//			return false;
	//		}
	//	}

	//	map[moveTo] = number;
	//	map[moveFrom] = 0;
	//	return true;
	//}

	//void PrintArray() {
	//	string debugText = string.Empty;
	//	for (int i = 0; i < map.Length; i++) {
	//		debugText += map[i].ToString() + ", ";
	//	}
	//	Debug.Log(debugText);
	//}

	//int GetPlayerIndex() {
	//	for (int i = 0; i < map.Length; i++) {
	//		if (map[i] == 1) {
	//			return i;
	//		}
	//	}

	//	return -1;
	//}
}
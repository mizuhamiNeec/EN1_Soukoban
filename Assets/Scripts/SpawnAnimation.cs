using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnAnimation : MonoBehaviour {
	private Vector3 endScale;

	private bool needDestroy = false;

	public void DeSpawnAnimation() {
		endScale = Vector3.zero;
		needDestroy = true;
	}

	// Start is called before the first frame update
	void Start() {
		endScale = transform.localScale;
		transform.localScale = Vector3.zero;
	}

	void Update() {
		// 目的の大きさになっていなかったら
		if(transform.localScale != endScale) {
			// スケールを変更する
			transform.localScale = Vector3.Lerp(transform.localScale, endScale, 10.0f * Time.deltaTime);
		} else if(needDestroy) {
			Destroy(this);
		}
	}
}

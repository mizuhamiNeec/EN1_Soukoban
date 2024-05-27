using UnityEngine;

public class Particle : MonoBehaviour {
	// 消滅するまでの時間
	private float lifeTime;
	// 消滅するまでの残り時間
	private float leftLifeTime;
	// 移動量
	private Vector3 velocity;
	// 初期Scale
	private Vector3 defaultScale;

	private void Start() {
		// 消滅するまでの時間を0.3秒とする
		lifeTime = 0.3f;
		leftLifeTime = lifeTime;
		defaultScale = transform.localScale;
		float maxVelocity = 5;

		velocity = new Vector3(
			Random.Range(-maxVelocity, maxVelocity),
			Random.Range(-maxVelocity, maxVelocity),
			0
		);
	}

	private void Update() {
		// 残り時間をカウントダウン
		leftLifeTime -= Time.deltaTime;
		// 自身の座標を移動
		transform.position += velocity * Time.deltaTime;
		// 残り時間により徐々にScaleを小さくする
		transform.localScale = Vector3.Lerp(
			new Vector3(0, 0, 0),
			defaultScale,
			leftLifeTime / lifeTime
			);

		// 回転させる
		transform.rotation = Quaternion.Euler(0.0f, 0.0f, 360 * leftLifeTime / lifeTime);

		// 残り時間が0以下になったら自身のゲームオブジェクトを消滅
		if(leftLifeTime <= 0) {
			Destroy(gameObject);
		}
	}
}

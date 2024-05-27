using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomRotator : MonoBehaviour {
	[SerializeField] private Vector3 minRot = Vector3.zero;
	[SerializeField] private Vector3 maxRot = Vector3.zero;

	void OnValidate() {
		RandomRotate();
	}

	void Start() {
		RandomRotate();
	}

	void RandomRotate() {
		Vector3 newRot = Vector3.zero;

		for(int i = 0; i < 3; ++i) {
			newRot[i] = Random.Range(minRot[i], maxRot[i]);
		}

		transform.rotation = Quaternion.Euler(newRot);
	}
}

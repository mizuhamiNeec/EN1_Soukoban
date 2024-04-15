using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterMovement : MonoBehaviour {

	[SerializeField] private Rigidbody _rb;
	[SerializeField] private Transform _cam;
	private Vector3 _vel = Vector3.zero;
	private Vector2 viewInput = Vector2.zero;
	private Vector2 moveInput = Vector2.zero;
	private float _moveSpeed = 5.0f;

	// Start is called before the first frame update
	private void Awake() {
		_rb = GetComponent<Rigidbody>();
	}

	// Update is called once per frame
	private void Update() {
		moveInput = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));
		viewInput = new Vector2(Input.GetAxisRaw("Mouse X"), Input.GetAxisRaw("Mouse Y"));

		Vector3 forward = _cam.forward;
		Vector3 right = _cam.right;

		forward.y = 0.0f;
		forward.z = 0.0f;

		forward.Normalize();
		right.Normalize();

		_vel = _cam.forward * moveInput.y + _cam.right * moveInput.x;

		_cam.localEulerAngles += new Vector3(-viewInput.y, viewInput.x, 0.0f);

		_cam.localEulerAngles = new Vector3(_cam.localEulerAngles.x, Mathf.Clamp(_cam.localEulerAngles.y, -90, 90), 0.0f);

		_rb.velocity = _vel * _moveSpeed;
	}
}

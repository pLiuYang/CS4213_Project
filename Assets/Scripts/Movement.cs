using UnityEngine;
using System.Collections;

[RequireComponent (typeof(CharacterController))]
public class Movement : MonoBehaviour {
	public float moveSpeed = 5;
	public float runMultiplier = 2;
	public float rotateSpeed = 250;
	public float strafeSpeed = 2.5f;
	
	private Transform _myTransform;
	private CharacterController _controller;
	
	public void Awake() {
		_myTransform = transform;
		_controller = GetComponent<CharacterController>();
	}
	
	// Use this for initialization
	void Start () {
		animation.wrapMode = WrapMode.Loop;
	}
	
	// Update is called once per frame
	void Update () {
		if (!_controller.isGrounded) {
			_controller.Move(Vector3.down * Time.deltaTime);
		}
		
		Turn ();
		Walk ();
		Strafe ();
	}
	
	private void Turn() {
		if (Mathf.Abs (Input.GetAxis("Rotate Player")) > 0) 
			_myTransform.Rotate(0, Input.GetAxis("Rotate Player") * Time.deltaTime * rotateSpeed, 0);
	}
	
	private void Walk() {
		if (Mathf.Abs (Input.GetAxis("Move Forward")) > 0) {
			if (Input.GetButton("Run")) {
				animation["GoodWalk"].speed = 3;
				animation.CrossFade("GoodWalk");
				_controller.SimpleMove(_myTransform.TransformDirection(Vector3.forward) * Input.GetAxis("Move Forward") * moveSpeed * runMultiplier);
			} else {
				animation["GoodWalk"].speed = 1;
				animation.CrossFade("GoodWalk");
				_controller.SimpleMove(_myTransform.TransformDirection(Vector3.forward) * Input.GetAxis("Move Forward") * moveSpeed);
			}
		}
		else {
			animation.CrossFade("GoodIdle");
		}
	}
	
	private void Strafe() {
		if (Mathf.Abs (Input.GetAxis("Strafe")) > 0) 
			_controller.SimpleMove(_myTransform.TransformDirection(Vector3.right) * Input.GetAxis("Strafe") * strafeSpeed);
	}
}

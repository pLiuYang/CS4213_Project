using UnityEngine;
using System.Collections;

[RequireComponent (typeof(CharacterController))]
public class AdvancedMovement : MonoBehaviour {
	public float walkSpeed = 5;
	public float runMultiplier = 2;
	public float rotateSpeed = 250;
	public float strafeSpeed = 2.5f;
	public float gravity = 20;
	public float airTime = 0;
	public float fallTime = .5f;
	public float jumpHeight = 8;
	public float jumpTime = 1.5f;
	
	public CollisionFlags _collisionFlags;
	private Vector3 _moveDirection;
	private Transform _myTransform;
	private CharacterController _controller;
	
	public void Awake() {
		_myTransform = transform;
		_controller = GetComponent<CharacterController>();
	}
	
	// Use this for initialization
	void Start () {
		_moveDirection = Vector3.zero;
		
		animation.Stop();
		
		animation.wrapMode = WrapMode.Loop;
		
		animation["jump"].layer = 1;
		animation["jump"].wrapMode = WrapMode.Once;
		
		animation.Play("NormIdle");
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetButton("Rotate Player")) {
			_myTransform.Rotate(0, Input.GetAxis("Rotate Player") * Time.deltaTime * rotateSpeed, 0);
		}
		
		if (_controller.isGrounded) {
			airTime = 0;
			
			_moveDirection = new Vector3(Input.GetAxis("Strafe"), 0, Input.GetAxis("Move Forward"));
			_moveDirection = _myTransform.TransformDirection(_moveDirection).normalized;
			_moveDirection *= walkSpeed;
			
			if (Input.GetButton("Move Forward")) {
				if (Input.GetButton("Run")) {
					_moveDirection *= runMultiplier;
					animation["GoodWalk"].speed = 3;
					Run ();
				} else {
					animation["GoodWalk"].speed = 1;
					Walk ();
				}
			} else if (Input.GetButton("Strafe")) {
				Strafe();
			} else {
				Idle ();
			}
			
			if (Input.GetButton("Jump")) {
				if (airTime < jumpTime) {
					_moveDirection.y += jumpHeight;
					Jump ();
				}
			}
		}
		else {
			if ((_collisionFlags & CollisionFlags.CollidedBelow) == 0) {
				airTime += Time.deltaTime;
				
				if (airTime > fallTime) {
					Fall();
				}
			}
		}
		
		_moveDirection.y -= gravity * Time.deltaTime;
		
		_collisionFlags = _controller.Move(_moveDirection * Time.deltaTime);
	}
	
	public void Idle() {
		animation.CrossFade("GoodIdle");
	}
	
	public void Walk() {
		animation.CrossFade("GoodWalk");
	}
	
	public void Run() {
		animation.CrossFade("GoodWalk");
	}
	
	public void Strafe() {
		animation.CrossFade("GoodWalk");
	}
	
	public void Jump() {
		animation.CrossFade("NormIdle");
	}
	
	public void Fall() {
		animation.CrossFade("GoodIdle"); // should be "fall"
	}
}

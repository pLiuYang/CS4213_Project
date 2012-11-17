using UnityEngine;
using System.Collections;

[RequireComponent (typeof(CharacterController))]
public class AdvancedMovement : MonoBehaviour {
	public enum State {
		Idle,
		Init,
		Setup,
		Run,
		Attack
	}
	
	public enum Turn {
		left = -1,
		none = 0,
		right = 1
	}
	
	public enum Forward {
		back = -1,
		none = 0,
		forward = 1
	}
	
	public AnimationClip attack;
	public string inCombatIdle;
	
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
	
	private Turn _turn;
	private Forward _forward;
	private Turn _strafe;
	private bool _run;
	private bool _jump;
	
	private State _state;
	
	private BaseCharacter _bc;
	
	public void Awake() {
		_myTransform = transform;
		_controller = GetComponent<CharacterController>();
		
		_state = AdvancedMovement.State.Init;
		
		_bc = gameObject.GetComponent<BaseCharacter>();
	}
	
	// Use this for initialization
	IEnumerator Start () {
		while (true) {
			switch (_state) {
			case State.Init:
				Init ();
				break;
			case State.Setup:
				Setup();
				break;
			case State.Run:
				ActionPicker ();
				break;
			}
			
			yield return null;
		}
	}
	
	private void Init() {
		if (!GetComponent<CharacterController>()) return;
		if (!GetComponent<Animation>())	return;
		
		_state = AdvancedMovement.State.Setup;
	}
	
	private void Setup() {
		_moveDirection = Vector3.zero;
		animation.Stop();
		animation.wrapMode = WrapMode.Loop;
		animation["jump"].layer = 1;
		animation["jump"].wrapMode = WrapMode.Once;
		animation.Play("NormIdle");
		
		_turn = AdvancedMovement.Turn.none;
		_forward = AdvancedMovement.Forward.none;
		_strafe = Turn.none;
		_run = false;
		_jump = false;
		
		_state = AdvancedMovement.State.Run;
	}
	
	private void ActionPicker() {
		//if (Input.GetButton("Rotate Player")) {
		_myTransform.Rotate(0, (int)_turn * Time.deltaTime * rotateSpeed, 0);
		//}
		
		if (_controller.isGrounded) {
			airTime = 0;
			
			_moveDirection = new Vector3((int)_strafe, 0, (int)_forward);
			_moveDirection = _myTransform.TransformDirection(_moveDirection).normalized;
			_moveDirection *= walkSpeed;
			
			if (_forward != Forward.none) {
				if (_run) {
					_moveDirection *= runMultiplier;
					animation["GoodWalk"].speed = 3;
					Run ();
				} else {
					animation["GoodWalk"].speed = 1;
					Walk ();
				}
			} else if (_strafe != AdvancedMovement.Turn.none) {
				Strafe();
			} else {
				Idle ();
			}
			
			if (_jump) {
				if (airTime < jumpTime) {
					_moveDirection.y += jumpHeight;
					Jump ();
					_jump = false;
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
	
	public void MoveMeForward(Forward z) {
		_forward = z;
	}
	
	public void ToggleRun() {
		_run = !_run;
	}
	
	public void RotateMe(Turn y) {
		_turn = y;
	}
	
	public void Strafe(Turn x) {
		_strafe = x;
	}
	
	public void JumpUp() {
		_jump = true;
	}
	
	public void Idle() {
		//if (inCombatIdle == "")
			//return;
		
		if (!_bc.inCombat)
			animation.CrossFade("GoodIdle");
		else if (_myTransform.FindChild("Name") == null)
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
	
	public void PlayMeleeAttack() {
		animation[attack.name].wrapMode = WrapMode.Once;
		
		if (attack == null) {
			Debug.LogWarning("We need an attack animation clip");
			return;
		}
		
		Debug.Log("amin length: " + attack.length);
		
		//animation[attack.name].speed = animation[attack.name].length / 2f;
		
		Debug.Log("amin speed2222222: " + animation[attack.name].speed.ToString());
		
		animation.Play(attack.name);
	}
}

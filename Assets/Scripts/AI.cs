using UnityEngine;
using System.Collections;

[RequireComponent (typeof(AdvancedMovement))]
[RequireComponent (typeof(SphereCollider))]
public class AI : MonoBehaviour {
	private enum State {
		Idle,
		Init,
		Setup,
		Search,
		Attack,
		Retreat,
		Flee
	}
	
	public float perceptionRadius = 10;
	
	public float baseMeleeRange = 2.8f;
	public Transform target;
	
	private Transform _myTransform;
	
	private const float ROTATION_DAMP = 0.3f;
	private const float FORWARD_DAMP = 0.9f;
	
	private Transform _home;
	private State _state;
	private bool _alive = true;
	private SphereCollider _sphereCollider;
	
	void Start() {
		_state = AI.State.Init;
		StartCoroutine("FSM");
		
/*		SphereCollider sc = GetComponent<SphereCollider>();
		CharacterController cc = GetComponent<CharacterController>();
		
		if (sc == null) 
			Debug.LogError("No sphereCollider on this mob");
		else 
			sc.isTrigger = true;
		
		if (cc == null) {
			Debug.LogError("No charactercontroller on this mob");
		} else {
			sc.center = cc.center;
			sc.radius = perceptionRadius;
		}
		
		_myTransform = transform;
*/
		
		//GameObject go = GameObject.FindGameObjectWithTag("Player");
		
		//if (go == null)
			//Debug.LogError("Could not find the player");
		
		//target = go.transform;
	}
	
	private IEnumerator FSM() {
		while (_alive) {
			switch(_state) {
			case State.Init:
				Init();
				break;
			case State.Setup:
				Setup();
				break;
			case State.Search:
				Search();
				break;
			case State.Attack:
				Attack();
				break;
			case State.Retreat:
				Retreat();
				break;
			case State.Flee:
				Flee ();
				break;
			}
			
			yield return null;
		}
	}
	
	private void Init() {
		_myTransform = transform;
		_home = transform.parent.transform;
		
		_sphereCollider = GetComponent<SphereCollider>();
			
		if (_sphereCollider == null) {
			Debug.LogError("Spherecollider not presents");
			return;
		}
		
		_state = AI.State.Setup;
	}
	
	private void Setup() {
		_sphereCollider.center = GetComponent<CharacterController>().center;
		_sphereCollider.radius = perceptionRadius;
		_sphereCollider.isTrigger = true;
		
		_state = AI.State.Search;
		_alive = false;
	}
	
	private void Search() {
		Move ();
		_state = AI.State.Attack;
	}
	
	private void Attack() {
		Move ();
		_state = AI.State.Retreat;
	}
	
	private void Retreat() {
		_myTransform.LookAt(target);
		Move ();
		_state = AI.State.Search;
	}
	
	private void Flee() {
		Move ();
		_state = AI.State.Search;
	}
	
/*	void Update() {
		if (target) {
			Vector3 dir = (target.position - _myTransform.position).normalized;
			float direction = Vector3.Dot(dir, transform.forward);
			
			float dist = Vector3.Distance(target.position, _myTransform.position);
			
			if (direction > FORWARD_DAMP && dist > baseMeleeRange) {
				SendMessage("MoveMeForward", AdvancedMovement.Forward.forward);
			} else {
				SendMessage("MoveMeForward", AdvancedMovement.Forward.none);
			}
			
			
			dir = (target.position - _myTransform.position).normalized;
			direction = Vector3.Dot(dir, transform.right);
			
			if (direction > ROTATION_DAMP) {
				SendMessage("RotateMe", AdvancedMovement.Turn.right);
			} else if (direction < -ROTATION_DAMP) {
				SendMessage("RotateMe", AdvancedMovement.Turn.left);
			} else {
				SendMessage("RotateMe", AdvancedMovement.Turn.none);
			}
		} else {
			SendMessage("MoveMeForward", AdvancedMovement.Forward.none);
			SendMessage("RotateMe", AdvancedMovement.Turn.none);
		}
	}
*/
	private void Move() {
		if (target) {
			Vector3 dir = (target.position - _myTransform.position).normalized;
			float direction = Vector3.Dot(dir, transform.forward);
			
			float dist = Vector3.Distance(target.position, _myTransform.position);
			
			if (direction > FORWARD_DAMP && dist > baseMeleeRange) {
				SendMessage("MoveMeForward", AdvancedMovement.Forward.forward);
			} else {
				SendMessage("MoveMeForward", AdvancedMovement.Forward.none);
			}
			
			
			dir = (target.position - _myTransform.position).normalized;
			direction = Vector3.Dot(dir, transform.right);
			
			if (direction > ROTATION_DAMP) {
				SendMessage("RotateMe", AdvancedMovement.Turn.right);
			} else if (direction < -ROTATION_DAMP) {
				SendMessage("RotateMe", AdvancedMovement.Turn.left);
			} else {
				SendMessage("RotateMe", AdvancedMovement.Turn.none);
			}
		} else {
			SendMessage("MoveMeForward", AdvancedMovement.Forward.none);
			SendMessage("RotateMe", AdvancedMovement.Turn.none);
		}
	}
	
	public void OnTriggerEnter(Collider other) {
		if (other.CompareTag("Player")) {
			target = other.transform;
			_alive = true;
			StartCoroutine("FSM");
		}
	}
	
	public void OnTriggerExit(Collider other) {
		if (other.CompareTag("Player")) {
			target = _home;
			//_alive = false;
		}
	}
}

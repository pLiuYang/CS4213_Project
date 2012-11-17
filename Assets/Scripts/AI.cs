using UnityEngine;
using System.Collections;

[RequireComponent (typeof(AdvancedMovement))]
[RequireComponent (typeof(SphereCollider))]
public class AI : MonoBehaviour {
	private enum State {
		Idle,				//do nothing
		Init,				//make sure that everything we need is here
		Setup,				//assign the values to the things we need
		Search,				//find the player
		Decide				//decide what to do with the targeted player
	}
	
	public float perceptionRadius = 10;
	
	private Transform _target;
	
	private Transform _myTransform;
	
	private const float ROTATION_DAMP = .3f;
	private const float FORWARD_DAMP = .9f;
	
	private Transform _home;
	private State _state;
	private SphereCollider _sphereCollider;
	
	
	private Mob _me;
	
	
	void Awake() {
		_me = gameObject.GetComponent<Mob>(); 
		
		if (_me == null)
			Debug.Log("NULL - +");
		
		Debug.Log( _me.meleeResetTimer + "!!!!!!!!" );
	}
	
	void Start() {
//		Debug.Log( "*** Start ***" );
		_state = AI.State.Init;
		StartCoroutine("FSM");
	}
	
	private IEnumerator FSM() {
//		Debug.Log( "*** FSM ***" );
		
//		Debug.LogWarning ( "Combat: " + _me.InCombat );

		while( _state != AI.State.Idle ) {
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
			case State.Decide:
				Decide();
				break;
			}
			
			yield return null;
		}
	}
	
	private void Init() {
// 		Debug.Log("***Init***");
		_myTransform = transform;
		_home = transform.parent.transform;

		_sphereCollider = GetComponent<SphereCollider>();
		
		if(_sphereCollider == null) {
			Debug.LogError("ShpereCollider not present!!");
			return;
		}

		_state = AI.State.Setup;
	}

	private void Setup() {
//		Debug.Log("***Setup***");
		_sphereCollider.center = GetComponent<CharacterController>().center;
		_sphereCollider.radius = perceptionRadius;
		_sphereCollider.isTrigger = true;
		
		_state = AI.State.Idle;
	}
	
	private void Search() {
		if(_target == null ) {
			Debug.LogError( "Target is null!!" );
			_state = AI.State.Idle;
			
			if( _me.InCombat )
				_me.InCombat = false;
		}
		else {
	//		Debug.Log("***Search*** : " + _target.name );
			
			//do we detect the targeted player yet?
			
			//if so set state to decide
	
			_state = AI.State.Decide;
			
			if( !_me.InCombat )
				_me.InCombat = true;
		}
	}
	

	private void Decide() {
//		Debug.Log("***Decide***");
		Move();
		
		//create a routine to decide what to do with the targeted player
		int opt = 0;
		
		if( _target != null && _target.CompareTag( "Player" ) ) {
			//he can do melee, ranged, and magic attacks
			if ( Vector3.Distance( _myTransform.position, _target.position ) < GameSetting2.BASE_MELEE_RANGE && _me.meleeResetTimer <= 0 ) 
				opt = Random.Range( 0, 3 );
			//he can not use melee attacks
			else {
				
				if( _me.meleeResetTimer > 0 )
					_me.meleeResetTimer -= Time.deltaTime;
				
				opt = Random.Range( 1, 3 );
			}
			
//			Debug.Log( opt );
			
			switch ( opt ) {
			case 0:
				MeleeAttack();
				break;
			case 1:
				RangedAttack();
				break;
			case 2:
				MagicAttack();
				break;
			default:
				Debug.Log( "Option: " + opt + " is not defined."  );
				break;

			//add cases for:
				//retreat - run to nearest mob
				//flee - just run away from the player
			}
		}


		_state = AI.State.Search;
	}
	

	private void MeleeAttack() {
		Debug.Log( "***Melee Attack ***" );
		
		//set attackresettimer to the meleeAttackTimer
		_me.meleeResetTimer = _me.meleeAttackTimer;

		//deal with the animation
		SendMessage( "PlayMeleeAttack" );
		
		//decide if we hit or not first
		if(true) {
			// do something
			Debug.Log( "We Hit!" );
			
		}
		else {
		  	// do something
			Debug.Log( "We Miss!" );
		}
		
		
		
		//meleeAttackTimer - the time it takes between attacks
		//meleeResetTimer  - the time left before we can attack again
		//meleeAttackSpeed - the speed the attack happens at
	}
	

	private void MagicAttack() {
//		Debug.Log( "***Magic Attack ***" );
	}


	private void RangedAttack() {
//		Debug.Log( "***Ranged Attack ***" );
	}


	
	
	
	
	
	
	private void Move() {
//		Debug.Log( "*** Move ***" );

//		Debug.LogWarning ( "Combat: " + _me.InCombat );

		if(_target) {
			float dist = Vector3.Distance(_target.position, _myTransform.position);
			
//			Debug.Log( "Target: " + _target.name + " - Distance: " + dist );
			
			if( _target.name == "Spawn Point" ) {
//				Debug.LogWarning( "Returning home: " + dist );
				
				if( dist < GameSetting2.BASE_MELEE_RANGE ) {
					_target = null;
					_state = AI.State.Idle;

					SendMessage("MoveMeForward", AdvancedMovement.Forward.none);
					SendMessage("RotateMe", AdvancedMovement.Turn.none);

					return;
				}
			}
			
			//we will need to incorporate this new turning code in to the advanced movement script
			//one thing we will have to do is lock the x-axis so we do not tilt up or down
			Quaternion rot = Quaternion.LookRotation( _target.transform.position - _myTransform.position );
			_myTransform.rotation = Quaternion.Slerp( _myTransform.rotation, rot, Time.deltaTime * 6.0f);


			Vector3 dir = (_target.position - _myTransform.position).normalized;
			float direction = Vector3.Dot(dir, transform.forward);
			
			if( direction > FORWARD_DAMP && dist > GameSetting2.BASE_MELEE_RANGE )
				SendMessage("MoveMeForward", AdvancedMovement.Forward.forward);
			else
				SendMessage("MoveMeForward", AdvancedMovement.Forward.none);
			
			
//			dir = (_target.position - _myTransform.position).normalized;
//			direction = Vector3.Dot(dir, transform.right);
			
//			if(direction > ROTATION_DAMP)
//				SendMessage("RotateMe", AdvancedMovement.Turn.right);
//			else if(direction < -ROTATION_DAMP)
//				SendMessage("RotateMe", AdvancedMovement.Turn.left);
//			else
//				SendMessage("RotateMe", AdvancedMovement.Turn.none);
		}
		else {
			SendMessage("MoveMeForward", AdvancedMovement.Forward.none);
			SendMessage("RotateMe", AdvancedMovement.Turn.none);
		}
	}
	
	
	public void OnTriggerEnter(Collider other) {
//		Debug.Log( "*** OnTriggerEnter *** " + " - " + other.name);

		if(other.CompareTag("Player")) {
			_target = other.transform;
			PC.Instance.inCombat = true;
			_state = AI.State.Search;
			StartCoroutine("FSM");
		}
		
	}
	
	public void OnTriggerExit(Collider other) {
		Debug.Log(" *** OnTriggerExit ***" );

		if(other.CompareTag("Player")) {
			_target = _home;
			PC.Instance.inCombat = false;
			if( _me.InCombat ) {
				_me.InCombat = false;
			}
		}
		
		//Debug.LogWarning ( "3 - Combat: " + _me.InCombat );
	}
}

using UnityEngine;
using System.Collections;

[RequireComponent (typeof(AdvancedMovement))]
[RequireComponent (typeof(SphereCollider))]
public class AI : MonoBehaviour {
	public float baseMeleeRange = 2.8f;
	public Transform target;
	
	private Transform _myTransform;
	
	private const float ROTATION_DAMP = 0.3f;
	private const float FORWARD_DAMP = 0.9f;

	void Start() {
		SphereCollider sc = GetComponent<SphereCollider>();
		
		if (sc == null) 
			Debug.LogError("No sphereCollider on this mob");
		else 
			sc.isTrigger = true;
		
		_myTransform = transform;
		
		GameObject go = GameObject.FindGameObjectWithTag("Player");
		
		if (go == null)
			Debug.LogError("Could not find the player");
		
		target = go.transform;
	}
	
	void Update() {
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
		}
	}
}

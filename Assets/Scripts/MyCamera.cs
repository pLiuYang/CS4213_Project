using UnityEngine;
using System.Collections;

public class MyCamera : MonoBehaviour {
	public Transform target;
	public string playerTagName = "Player";
	
	public float walkDistance;
	public float runDistance;
	public float height;
	public float xSpeed = 250.0f;
	public float ySpeed = 120.0f;
	public float heightDamping = 2.0f;
	public float rotationDamping = 3.0f;
	
	private Transform _myTransform;
	private float x;
	private float y;
	private bool camButtonDown = false;
	private bool _rotateCameraKeyPressed = false;
	
	void Awake() {
		_myTransform = transform;
	}
	
	
	// Use this for initialization
	void Start () {
		if (target == null)
			Debug.LogWarning("We do not have a target");
		else {
			CameraSetUp();
		}
	}
	
	void Update() {
		if (Input.GetButtonDown("Rotate Camera Button"))
			camButtonDown = true;
		
		if (Input.GetButtonUp("Rotate Camera Button")) {
			x = 0;
			y = 0;
			camButtonDown = false;
		}
		
		if (Input.GetButtonDown("Rotate Camera Horizontal Buttons") || Input.GetButtonDown("Rotate Camera Vertical Buttons"))
			_rotateCameraKeyPressed = true;
		
		if (Input.GetButtonUp("Rotate Camera Horizontal Buttons") || Input.GetButtonUp("Rotate Camera Vertical Buttons")) {
			x = 0;
			y = 0;
			_rotateCameraKeyPressed = false;
		}
	}
	
	void LateUpdate() {
		if (target != null) {
			if (_rotateCameraKeyPressed) {
				x += Input.GetAxis("Rotate Camera Horizontal Buttons") * xSpeed * 0.02f;
				y -= Input.GetAxis("Rotate Camera Vertical Buttons") * ySpeed * 0.02f;
				
				RotateCamera();
			}
			else if (camButtonDown) {
				x += Input.GetAxis("Mouse X") * xSpeed * 0.02f;
				y -= Input.GetAxis("Mouse Y") * ySpeed * 0.02f;
				
				RotateCamera();
			}
			else {
				float wantedRotationAngle = target.eulerAngles.y;
				float wantedHeight = target.position.y + height;
				
				float currentRotationAngle = _myTransform.eulerAngles.y;
				float currentHeight = _myTransform.position.y;
				
				currentRotationAngle = Mathf.LerpAngle(currentRotationAngle, wantedRotationAngle, rotationDamping * Time.deltaTime);
				currentHeight = Mathf.Lerp(currentHeight, wantedHeight, heightDamping * Time.deltaTime);
				Quaternion currentRotation = Quaternion.Euler(0, currentRotationAngle, 0);
				
				_myTransform.position = target.position;
				_myTransform.position -= currentRotation * Vector3.forward * walkDistance;
				_myTransform.position = new Vector3(_myTransform.position.x, currentHeight, _myTransform.position.z);
				_myTransform.LookAt(target);			
			}
		}
		else {
			GameObject go = GameObject.FindGameObjectWithTag(playerTagName);
			
			if (go == null)
				return;
			
			target = go.transform;
		}
	}
	
	public void RotateCamera() {
		Quaternion rotation = Quaternion.Euler(y, x, 0);
		Vector3 position = rotation * new Vector3(0.0f, 0.0f, -walkDistance) + target.position;
				
		_myTransform.rotation = rotation;
		_myTransform.position = position;
	}
	
	public void CameraSetUp() {
		_myTransform.position = new Vector3(target.position.x, target.position.y + height, target.position.z - walkDistance);
		_myTransform.LookAt(target);
	}
}

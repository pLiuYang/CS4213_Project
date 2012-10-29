using UnityEngine;
using System.Collections;

[RequireComponent (typeof(AdvancedMovement))]
public class PlayerInput : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetButtonUp("Toggle Inventory")) {
			Messenger.Broadcast("ToggleInventory");
		}
		
		if (Input.GetButtonUp("Toggle Character Window")) {
			Messenger.Broadcast("ToggleCharacterWindow");
		}
		
		if (Input.GetButton("Move Forward")) {
			if (Input.GetAxis("Move Forward") > 0) {
				SendMessage("MoveMeForward", AdvancedMovement.Forward.forward);
			} else {
				SendMessage("MoveMeForward", AdvancedMovement.Forward.back);
			}
		}
		
		if (Input.GetButtonUp("Move Forward")) {
			SendMessage("MoveMeForward", AdvancedMovement.Forward.none);
		}
		
		if (Input.GetButton("Rotate Player")) {
			if (Input.GetAxis("Rotate Player") > 0) {
				SendMessage("RotateMe", AdvancedMovement.Turn.right);
			} else {
				SendMessage("RotateMe", AdvancedMovement.Turn.left);
			}
		}
		
		if (Input.GetButtonUp("Rotate Player")) {
			SendMessage("RotateMe", AdvancedMovement.Turn.none);
		}
		
		if (Input.GetButton("Strafe")) {
			if (Input.GetAxis("Strafe") > 0) {
				SendMessage("Strafe", AdvancedMovement.Turn.right);
			} else {
				SendMessage("Strafe", AdvancedMovement.Turn.left);
			}
		}
		
		if (Input.GetButtonUp("Strafe")) {
			SendMessage("Strafe", AdvancedMovement.Turn.none);
		}
		
		if (Input.GetButtonDown("Jump")) {
			SendMessage("JumpUp");
		}
		
		if (Input.GetButtonDown("Run")) {
			SendMessage("ToggleRun");
		}
	}
}

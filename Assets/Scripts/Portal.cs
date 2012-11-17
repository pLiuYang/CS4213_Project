using UnityEngine;
using System.Collections;

public class Portal : MonoBehaviour {
	private const string DEFAULT_DROP_ZONE_NAME = "dz_StartPoint";
	public GameObject destination;

	// Use this for initialization
	void Start () {
		if( destination == null ) {
			destination = GameObject.Find( DEFAULT_DROP_ZONE_NAME );	
			Debug.LogWarning("NULL destination");
		}
	}
	
	public void OnTriggerEnter( Collider other ) {
		if( other.transform.CompareTag("Player") ) {
			Debug.LogWarning("Find player");
			other.transform.position = destination.transform.position;
		}
	}
}

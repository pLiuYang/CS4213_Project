using UnityEngine;
using System.Collections;

[AddComponentMenu("Hack And Slash Tutorial/Objects/Spawn Point")]
public class SpawnPoint : MonoBehaviour {
	
	//check this flag to see if we can spawn a new mob or not.
	public bool available = true;	//value currently not being used

	public void OnDrawGizmos() {
		Gizmos.color = Color.red;
		Gizmos.DrawWireSphere(transform.position, 2);
	}
}

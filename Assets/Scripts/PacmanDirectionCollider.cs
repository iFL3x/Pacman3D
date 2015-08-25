using UnityEngine;
using System.Collections;

public class PacmanDirectionCollider : MonoBehaviour {

	PacmanController pc;

	void Start(){
		pc = GetComponentInParent<PacmanController> ();
	}

	void OnTriggerEnter(Collider col){
		if (col.tag == "Tile") {
			pc.ChildTriggerEnter (col);
		}
	}

	void OnTriggerExit(Collider col){
		pc.ChildTriggerExit (col);
	}
}

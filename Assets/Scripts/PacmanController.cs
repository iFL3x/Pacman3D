using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PacmanController : MonoBehaviour {

	public float defSpeed = 0.02f;
	private float speed;
	private Vector3 curDirection;
	public Vector3 plannedDirection;
	public List<string> validDirections = new List<string>();
	public LayerMask TileMask;
	private GameObject curTile;
	private bool stopping = false;
	
	void Start () {
		curDirection = Vector3.right;
		speed = defSpeed;
	}

	void FixedUpdate () {

		if (Input.GetKey ("w")) {
			plannedDirection = Vector3.forward;
			StartPacman();
		}

		if (Input.GetKey ("a")) {
			plannedDirection = Vector3.left;
			StartPacman();
		}

		if (Input.GetKey ("s")) {
			//curDirection = Vector3.back;
			StartPacman();
		}

		if (Input.GetKey ("d")) {
			plannedDirection = Vector3.right;
			StartPacman();
		}

		if (stopping && Vector3.Distance(new Vector3(curTile.transform.position.x, transform.position.y, curTile.transform.position.z), transform.position) < 0.1f) {
			StopPacman();
		}
		transform.Translate (Vector3.forward * speed);


	}

	private void StartPacman(){
		stopping = false;
		speed = defSpeed;
	}

	private void StopPacman(){
		speed = 0;
	}

	void OnTriggerEnter(Collider col){
		col.transform.tag = "Tile_Pacman";
	}

	void OnTriggerExit(Collider col){
		col.transform.tag = "Tile";
	}

	public void ChildTriggerEnter(Collider col){
		validDirections.Clear ();
		col.tag = "Tile_Pacman";
		curTile = col.gameObject;
		CheckNeighbourTiles ();
		CheckDirection ();
	}

	public void ChildTriggerExit(Collider col){
		col.tag = "Tile";
	}

	//Check which neighbour tiles are free
	private void CheckNeighbourTiles(){
		Vector3 pacmanTilePos = curTile.transform.position;

		//North
		if (Physics.Raycast (new Vector3 (pacmanTilePos.x - 1, 1f, pacmanTilePos.z), Vector3.down, 1.5f, TileMask)) {
			validDirections.Add("North");
		}

		//South
		if (Physics.Raycast (new Vector3 (pacmanTilePos.x + 1, 1f, pacmanTilePos.z), Vector3.down, 1.5f, TileMask)) {
			validDirections.Add("South");
		}

		//West
		if (Physics.Raycast (new Vector3 (pacmanTilePos.x, 1f, pacmanTilePos.z - 1), Vector3.down, 1.5f, TileMask)) {
			validDirections.Add("West");
		}

		//East
		if (Physics.Raycast (new Vector3 (pacmanTilePos.x, 1f, pacmanTilePos.z + 1), Vector3.down, 1.5f, TileMask)) {
			validDirections.Add("East");
		}
	}

	private void CheckDirection(){
		//If there is no direction command
		if (plannedDirection == Vector3.zero) {
			//Check if pacman can go forward, else stop in the middle of the current tile
			if (curDirection == Vector3.up && !validDirections.Contains ("North") ||
				curDirection == Vector3.right && !validDirections.Contains ("East") ||
				curDirection == Vector3.down && !validDirections.Contains ("South") ||
				curDirection == Vector3.left && !validDirections.Contains ("West")) {
				stopping = true;
			}  
		} else if(validDirections.Count >= 3){
			//Turn pacman based on plannedDirection and current Direction

		}
	}
	
}

using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PacmanController : MonoBehaviour {

	public float defSpeed = 0.02f;
	private float speed;
	public Direction curDirection = Direction.Left;
	public Direction plannedDirection = Direction.None;
	public List<Direction> validDirections = new List<Direction>();
	public LayerMask TileMask;
	private GameObject curTile;
	private bool stopping = false;
	
	void Start () {
		speed = defSpeed;
	}

	void Update () {

		if (Input.GetKey ("w")) {
			plannedDirection = Direction.Forward;
			StartPacman();
		}

		if (Input.GetKey ("a")) {
			plannedDirection =	Direction.Left;
			StartPacman();
		}

		if (Input.GetKeyDown("s")) {
			if(validDirections.Contains(GetBackDirection(curDirection))) {
				transform.Rotate(0,180,0);
			}
			plannedDirection = Direction.None;
			StartPacman();
		}

		if (Input.GetKey ("d")) {
			plannedDirection = Direction.Right;
			StartPacman();
		}

		if (stopping && Vector3.Distance(new Vector3(curTile.transform.position.x, transform.position.y, curTile.transform.position.z), transform.position) < 0.1f) {
			StopPacman();
		}
		transform.Translate (Vector3.forward * speed * Time.deltaTime);
	
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

		//Up
		if (Physics.Raycast (new Vector3 (pacmanTilePos.x - 1, 1f, pacmanTilePos.z), Vector3.down, 1.5f, TileMask)) {
			validDirections.Add(Direction.Up);
		}

		//Down
		if (Physics.Raycast (new Vector3 (pacmanTilePos.x + 1, 1f, pacmanTilePos.z), Vector3.down, 1.5f, TileMask)) {
			validDirections.Add(Direction.Down);
		}

		//Left
		if (Physics.Raycast (new Vector3 (pacmanTilePos.x, 1f, pacmanTilePos.z - 1), Vector3.down, 1.5f, TileMask)) {
			validDirections.Add(Direction.Left);
		}

		//Right
		if (Physics.Raycast (new Vector3 (pacmanTilePos.x, 1f, pacmanTilePos.z + 1), Vector3.down, 1.5f, TileMask)) {
			validDirections.Add(Direction.Right);
		}
	}

	private void CheckDirection(){
		//If there is no direction command
		if (plannedDirection == Direction.None) {
			//Check if pacman can go forward, else stop in the middle of the current tile
			if (curDirection == Direction.Up && !validDirections.Contains (Direction.Up) ||
			    curDirection == Direction.Right && !validDirections.Contains (Direction.Right) ||
			    curDirection == Direction.Down && !validDirections.Contains (Direction.Down) ||
			    curDirection == Direction.Left && !validDirections.Contains (Direction.Left)) {
				stopping = true;
			}  
		} else if(validDirections.Count >= 3){
			//Turn pacman based on plannedDirection and current Direction
			if(plannedDirection == Direction.Left && validDirections.Contains(GetLeftDirection(curDirection))){
				transform.Rotate(0,270,0);
			} else if(plannedDirection == Direction.Right && validDirections.Contains(GetRightDirection(curDirection))){
				transform.Rotate(0,90,0);
			} 
		}
	}

	private Direction GetLeftDirection(Direction currentDirection){
		Direction dir = Direction.None;
		if(currentDirection == Direction.Up){
			dir = Direction.Left;
		} else if(currentDirection == Direction.Right){
			dir = Direction.Up;
		} else if(currentDirection == Direction.Down){
			dir = Direction.Right;
		} else if(currentDirection == Direction.Left){
			dir = Direction.Down;
		} else {
			dir = Direction.None;
		}
		return dir;
	}
	private Direction GetRightDirection(Direction currentDirection){
		Direction dir = Direction.None;
		if(currentDirection == Direction.Up){
			dir = Direction.Right;
		} else if(currentDirection == Direction.Right){
			dir = Direction.Down;
		} else if(currentDirection == Direction.Down){
			dir = Direction.Left;
		} else if(currentDirection == Direction.Left){
			dir = Direction.Up;
		} else {
			dir = Direction.None;
		}
		return dir;
	}

	private Direction GetBackDirection(Direction currentDirection){
		Direction dir = Direction.None;
		if(currentDirection == Direction.Up){
			dir = Direction.Down;
		} else if(currentDirection == Direction.Right){
			dir = Direction.Left;
		} else if(currentDirection == Direction.Down){
			dir = Direction.Up;
		} else if(currentDirection == Direction.Left){
			dir = Direction.Right;
		} else {
			dir = Direction.None;
		}
		return dir;
	}

	
}

public enum Direction {
	None,
	Up,
	Right,
	Down,
	Left,
	Forward,
	Back
}

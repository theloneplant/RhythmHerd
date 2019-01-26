using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour {
	private enum Direction {
		Up, Down, Left, Right, None
	};

	[SerializeField] private GameManager manager;

	private HerdMember[] herd;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		Direction input = Direction.None;
		if (Input.GetButtonDown("Up")) {
			input = Direction.Up;
		}
		else if (Input.GetButtonDown("Down")) {
			input = Direction.Down;
		}
		else if (Input.GetButtonDown("Left")) {
			input = Direction.Left;
		}
		else if (Input.GetButtonDown("Right")) {
			input = Direction.Right;
		}

		if (input != Direction.None) {
			float score = manager.getBeatScore();
            //Debug.Log(score);
		}
	}
}

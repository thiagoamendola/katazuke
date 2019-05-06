using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class InputScreen : MonoBehaviour {

	enum InputStage{
		AssigningInput, AssignComplete
	}

	InputStage inputStage;

	PlayerNumber playerNumber;

	IEnumerator processControlFlow = null;

	void Start() {
		InitiateScreen();
	}

	// Controls the whole control assignment flow
	IEnumerator ProcessControlAssignment(){
		inputStage = InputStage.AssigningInput;
		// Gets list of all valid players
		var playerList = Enum.GetValues(typeof(PlayerNumber)).Cast<PlayerNumber>();
		yield return new WaitForSeconds(1f);
		foreach (PlayerNumber playerNumber in playerList)
		{
			Debug.Log("Choosing "+playerNumber.ToString());
			bool successfulAssignment = false;
			while(!successfulAssignment){
				//yield return null;
				yield return new WaitUntil(() => Input.anyKeyDown);
				// Check if input corresponds to any input type
				InputType? detectedType = InputManager.DetectInputType();
				if (detectedType != null){
					// Assign detected input for the player
					Debug.Log("detectedType = "+detectedType.ToString());
					InputManager.SetPlayer(playerNumber, (InputType)detectedType);
					successfulAssignment = true;
				}
			}
		}
		Debug.Log("FIN");
		inputStage = InputStage.AssignComplete;
		processControlFlow = null;
	}


	void InitiateScreen(){
		Debug.Log("Clear player 1 UI");
		Debug.Log("Clear player 2 UI");
		inputStage = InputStage.AssigningInput;
		playerNumber = PlayerNumber.Player1;
		if (processControlFlow != null)
			StopCoroutine(processControlFlow);
		processControlFlow = ProcessControlAssignment();
		StartCoroutine(processControlFlow);
	}

	public void Reset(){
		Debug.Log("Reset");
		InputManager.Reset();
		InitiateScreen();
	}
}

using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class InputScreen : MonoBehaviour {

	enum InputStep{
		AssigningInput, AssignComplete
	}

	public List<TextMeshProUGUI> controlLabels;

	InputStep inputStep;

	PlayerNumber playerNumber;

	IEnumerator processControlFlow = null;

	void Start() {
		InitiateScreen();
	}

	// Controls the whole control assignment flow
	IEnumerator ProcessControlAssignment(){
		inputStep = InputStep.AssigningInput;
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
				ControlInput? detectedType = InputManager.DetectControlInput();
				if (detectedType != null){
					// Assign detected input for the player
					controlLabels[(int)playerNumber].text = detectedType.ToString();
					InputManager.SetPlayer(playerNumber, (ControlInput)detectedType);
					successfulAssignment = true;
				}
			}
		}
		Debug.Log("FIN");
		inputStep = InputStep.AssignComplete;
		processControlFlow = null;
        SceneManager.LoadScene("MainScene");
    }

	void InitiateScreen(){
		Debug.Log("Clear player 1 UI");
		Debug.Log("Clear player 2 UI");
		inputStep = InputStep.AssigningInput;
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

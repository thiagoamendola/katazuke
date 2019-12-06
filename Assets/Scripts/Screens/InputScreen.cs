using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class InputScreen : GenericScreen {

	enum InputStep{
		AssigningInput, AssignComplete
	}

    [Header("Input Screen")]
	public List<Animator> wardrobeList;
	public List<InputSampler> inputSamplerList;
	public List<TextMeshProUGUI> controlLabels;

	InputStep inputStep;

	PlayerNumber playerNumber;

	IEnumerator processControlFlow = null;


    public override void Open(){
        FocusCamera();
        InitiateScreen();
    }

    public override void Close(){

    }

	// Controls the whole control assignment flow
	IEnumerator ProcessControlAssignment(){
		inputStep = InputStep.AssigningInput;
		// Gets list of all valid players
		var playerList = Enum.GetValues(typeof(PlayerNumber)).Cast<PlayerNumber>();
		// Close all wardrobes
		foreach (PlayerNumber playerNumber in playerList){
			wardrobeList[(int)playerNumber].ResetTrigger("Open");
			wardrobeList[(int)playerNumber].SetTrigger("Close");
		}
		yield return new WaitForSeconds(transitionDuration + 0f);
		foreach (PlayerNumber playerNumber in playerList){
			Debug.Log("Choosing "+playerNumber.ToString());
			bool successfulAssignment = false;
			int detectedTypeIndex = 0;
			while(!successfulAssignment){
				// Open player's wardrobe
				wardrobeList[(int)playerNumber].SetTrigger("Open");
				// Wait for input
				yield return new WaitUntil(() => Input.anyKeyDown);
				// Check if input corresponds to any input type
				ControlInput? detectedType = InputManager.DetectControlInput();
				if (detectedType != null){
					// Assign detected input for the player
					controlLabels[(int)playerNumber].text = detectedType.ToString();
					InputManager.SetPlayer(playerNumber, (ControlInput)detectedType);
					detectedTypeIndex = (int)(ControlInput)detectedType;
					successfulAssignment = true;
				}
			}
			// Close player's wardrobe
			wardrobeList[(int)playerNumber].ResetTrigger("Open");
			wardrobeList[(int)playerNumber].SetTrigger("Close");
			inputSamplerList[(int)playerNumber].SelectInput(detectedTypeIndex);
		}
		Debug.Log("FIN");
		// Add countdown
		yield return new WaitForSeconds(2f);
		// Disappear with HUD
		// Maybe improve here: only go to next scene when interaction input is pressed.
		inputStep = InputStep.AssignComplete;
		processControlFlow = null;
		ScreenManager.GoToGameScreen();
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

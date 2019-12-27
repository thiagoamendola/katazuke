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
	public List<GameObject> characterList;
	public TextMeshProUGUI escText;
	public Transform loadingCameraRef;

	InputStep inputStep;

	PlayerNumber playerNumber;

	IEnumerator processControlFlow = null;

	bool started = false;
	bool resetable = true;

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
		}
		if(!started){
			escText.gameObject.transform.parent.GetComponent<CanvasGroup>().alpha = 0;
			yield return new WaitForSeconds(TRANSITIONDURATION);
			escText.gameObject.transform.parent.GetComponent<CanvasGroup>().alpha = 1;
		}
		started = true;
		resetable = true;
		foreach (PlayerNumber playerNumber in playerList){
			bool successfulAssignment = false;
			int detectedTypeIndex = 0;
			this.playerNumber = playerNumber;
			// Open player's wardrobe.
			wardrobeList[(int)playerNumber].ResetTrigger("Close");
			wardrobeList[(int)playerNumber].ResetTrigger("Open");
			wardrobeList[(int)playerNumber].SetTrigger("Open");
			// Wait for input assignment.
			while(!successfulAssignment){
				// Wait for input
				yield return new WaitUntil(() => Input.anyKeyDown);
				// Check if input corresponds to any input type
				ControlInput? detectedType = InputManager.DetectControlInput();
				if (detectedType != null){
					// Assign detected input for the player
					InputManager.SetPlayer(playerNumber, (ControlInput)detectedType);
					detectedTypeIndex = (int)(ControlInput)detectedType;
					successfulAssignment = true;
				}
			}
			// Close player's wardrobe
			wardrobeList[(int)playerNumber].ResetTrigger("Open");
			wardrobeList[(int)playerNumber].SetTrigger("Close");
			inputSamplerList[(int)playerNumber].SelectInput(detectedTypeIndex);
			//
			escText.text = "to Restart";
		}
		// Add countdown <--
		// Maybe improve here: only go to next scene when interaction input is pressed.
		yield return new WaitForSeconds(1.5f);
		resetable = false;
		escText.gameObject.transform.parent.GetComponent<CanvasGroup>().alpha = 0;
		// Start pre loading animation
        StartCoroutine(AsyncFocusCamera(ScreenManager.activeCamera, loadingCameraRef, 0.75f));
		foreach(GameObject character in characterList){
			character.SetActive(true);
			character.transform.Find("Character").GetComponent<Animator>().SetTrigger("angry");
			character.GetComponent<Animator>().SetTrigger("engage");
		}
		yield return new WaitForSeconds(2.25f);
		// Go to game screen.
		inputStep = InputStep.AssignComplete;
		processControlFlow = null;
		ScreenManager.GoToGameScreen();
    }

	void InitiateScreen(){
		inputStep = InputStep.AssigningInput;
		playerNumber = PlayerNumber.Player1;
		escText.text = "to Exit";
		resetable = false;
		if (processControlFlow != null)
			StopCoroutine(processControlFlow);
		processControlFlow = ProcessControlAssignment();
		StartCoroutine(processControlFlow);
	}

	public void Reset(){
		inputStep = InputStep.AssigningInput;
		playerNumber = PlayerNumber.Player1;
		resetable = false;
		escText.text = "to Exit";
		var playerList = Enum.GetValues(typeof(PlayerNumber)).Cast<PlayerNumber>();
		foreach (PlayerNumber playerNumber in playerList){
			if((int)playerNumber != 0){
				wardrobeList[(int)playerNumber].ResetTrigger("Close");
				wardrobeList[(int)playerNumber].SetTrigger("Close");
			}
			inputSamplerList[(int)playerNumber].ResetInput();
		}
		InputManager.Reset();
		InitiateScreen();
	}

	void Update() {
		if(resetable && Input.GetKeyDown(KeyCode.Escape)){
			if (playerNumber == PlayerNumber.Player1){
				wardrobeList[(int)PlayerNumber.Player1].ResetTrigger("Open");
				wardrobeList[(int)PlayerNumber.Player1].ResetTrigger("Close");
				wardrobeList[(int)PlayerNumber.Player1].SetTrigger("Close");
				resetable = false;
				ScreenManager.GoToTitleScreen();
			}else{
				Reset();
			}
		}
	}



}

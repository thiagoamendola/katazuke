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
	public List<GameObject> characterList;

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
		if(!started)
			yield return new WaitForSeconds(TRANSITIONDURATION);
		started = true;
		resetable = true;
		foreach (PlayerNumber playerNumber in playerList){
			Debug.Log("Choosing "+playerNumber.ToString());
			bool successfulAssignment = false;
			int detectedTypeIndex = 0;
			while(!successfulAssignment){
				// Open player's wardrobe
				wardrobeList[(int)playerNumber].ResetTrigger("Close");
				wardrobeList[(int)playerNumber].ResetTrigger("Open");
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
		// Add countdown
		// Maybe improve here: only go to next scene when interaction input is pressed.
		yield return new WaitForSeconds(1.5f);
		resetable = false;
		// Start pre loading animation
        StartCoroutine(AsyncFocusCamera(ScreenManager.activeCamera, loadingCameraRef, 0.75f));
		foreach(GameObject character in characterList){
			character.SetActive(true);
			character.transform.Find("Character").GetComponent<Animator>().SetTrigger("angry");
			character.GetComponent<Animator>().SetTrigger("engage");
		}
		yield return new WaitForSeconds(2.25f);
		//
		inputStep = InputStep.AssignComplete;
		processControlFlow = null;
		ScreenManager.GoToGameScreen();
    }

	void InitiateScreen(){
		inputStep = InputStep.AssigningInput;
		playerNumber = PlayerNumber.Player1;
		resetable = false;
		if (processControlFlow != null)
			StopCoroutine(processControlFlow);
		processControlFlow = ProcessControlAssignment();
		StartCoroutine(processControlFlow);
	}

	public void Reset(){
		Debug.Log("Reset");
		resetable = false;
		var playerList = Enum.GetValues(typeof(PlayerNumber)).Cast<PlayerNumber>();
		foreach (PlayerNumber playerNumber in playerList){
			print((int)playerNumber);
			if((int)playerNumber != 0){
				print("ENTER");
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
			Reset();
		}
	}



}

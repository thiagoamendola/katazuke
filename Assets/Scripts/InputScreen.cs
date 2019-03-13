using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputScreen : MonoBehaviour {

    enum InputStage{
        AssigningInput, AssignComplete
    }

    InputStage inputStage;

    PlayerNumber playerNumber;

    
    void Start() {
        InitiateScreen();
    }

    void Update() {
        // Check for reset
        if(Input.GetKey(KeyCode.Escape)){
            Reset();
            return;
        }

        if(inputStage == InputStage.AssigningInput && Input.anyKey){
            
            // Check any controller input
                // Find a way to toggle players
            
            // Check for keyboard inputs

        }
    }



    void InitiateScreen(){
        Debug.Log("Clear player 1 UI");
        Debug.Log("Clear player 2 UI");
        inputStage = InputStage.AssigningInput;
        playerNumber = PlayerNumber.Player1;
    }

    public void Reset(){
        Debug.Log("Reset");
        InputManager.Reset();
        InitiateScreen();
    }
}

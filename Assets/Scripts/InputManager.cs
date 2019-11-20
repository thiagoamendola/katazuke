using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public enum PlayerNumber {
	Player1, Player2
};

public enum ControlInput {
	Key1, Key2, Ctrl1, Ctrl2
};

public class InputManager : MonoBehaviour{

	// Maybe store here checks for the button inputs

	static Dictionary<PlayerNumber,ControlInput?> inputMappings = new Dictionary<PlayerNumber, ControlInput?>();

	static List<string> commandNameList = new List<string>{
		"Horizontal",
		"Vertical",
		"Action",
	};

	// Player input setup

	// Sets an input type for a givem player.
	public static void SetPlayer(PlayerNumber playerNumber, ControlInput controlInput){
		PlayerNumber? currentPlayer = GetPlayer(controlInput);
		if (currentPlayer != null){
			inputMappings[(PlayerNumber)currentPlayer] = null;
		}
		inputMappings.Add(playerNumber, controlInput);
	}

	// Resets input mappings for players.
	public static void Reset(){
		inputMappings.Clear();
	}

	// Input detection

	public static ControlInput? DetectControlInput(){
		var controlInputList = Enum.GetValues(typeof(ControlInput)).Cast<ControlInput>();
		foreach (ControlInput controlInput in controlInputList){
			foreach(string commandName in commandNameList){
				string fullCommandName = commandName+controlInput.ToString();
				if (Input.GetAxisRaw(fullCommandName) != 0){
					// Check if this input was already assigned.
					var currentPlayer = GetPlayer(controlInput);
					if (currentPlayer != null){
						//print("Already used "+fullCommandName + " -> "+currentPlayer.ToString());
						continue;
					}
					// Returns valid input type.
					return controlInput;
				}
			}
		}
		// No valid input type found.
		return null;
	}

	// Getters

	// Gets input type assigned for provided player.
	public static ControlInput? GetControlInput(PlayerNumber playerNumber){
		if(inputMappings.ContainsKey(playerNumber))
			return inputMappings[playerNumber];
		return (ControlInput?) ControlInput.Key1;
	}

	// Gets player assigned for provided input type.
	public static PlayerNumber? GetPlayer(ControlInput controlInput){
		if (inputMappings.ContainsValue(controlInput)){
			return inputMappings.Where(x => x.Value == controlInput).FirstOrDefault().Key;
		}
		return null;
	}


}
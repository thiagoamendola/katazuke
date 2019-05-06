using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public enum PlayerNumber {
	Player1, Player2
};

public enum InputType {
	Key1, Key2, Ctrl1, Ctrl2
};

public class InputManager : MonoBehaviour{

	// Maybe store here checks for the button inputs

	static Dictionary<PlayerNumber,InputType?> inputMappings = new Dictionary<PlayerNumber, InputType?>();

	static List<string> commandNameList = new List<string>{
		"Horizontal",
		"Vertical",
		"Action",
	};

	// Player input setup

	// Sets an input type for a givem player.
	public static void SetPlayer(PlayerNumber playerNumber, InputType inputType){
		PlayerNumber? currentPlayer = GetPlayer(inputType);
		if (currentPlayer != null){
			inputMappings[(PlayerNumber)currentPlayer] = null;
		}
		inputMappings.Add(playerNumber, inputType);
	}

	// Resets input mappings for players.
	public static void Reset(){
		inputMappings.Clear();
	}

	// Input detection

	public static InputType? DetectInputType(){
		var inputTypeList = Enum.GetValues(typeof(InputType)).Cast<InputType>();
		foreach (InputType inputType in inputTypeList){
			foreach(string commandName in commandNameList){
				string fullCommandName = commandName+inputType.ToString();
				if (Input.GetAxisRaw(fullCommandName) != 0){
					// Check if this input was already assigned.
					var currentPlayer = GetPlayer(inputType);
					if (currentPlayer != null){
						//print("Already used "+fullCommandName + " -> "+currentPlayer.ToString());
						continue;
					}
					// Returns valid input type.
					return inputType;
				}
			}
		}
		// No valid input type found.
		return null;
	}

	// Getters

	// Gets input type assigned for provided player.
	public static InputType? GetInputType(PlayerNumber playerNumber){
		return inputMappings[playerNumber];
	}

	// Gets player assigned for provided input type.
	public static PlayerNumber? GetPlayer(InputType inputType){
		if (inputMappings.ContainsValue(inputType)){
			return inputMappings.Where(x => x.Value == inputType).FirstOrDefault().Key;
		}
		return null;
	}


}
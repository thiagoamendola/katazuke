using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public enum PlayerNumber {
    Player1, Player2
};

public enum InputType {
    None, Key1, Key2, Ctrl1, Ctrl2
};

public class InputManager : MonoBehaviour{

    // Maybe store here checks for the button inputs

    static Dictionary<PlayerNumber,InputType> inputMappings;

    public static void SetPlayer(PlayerNumber playerNumber, InputType inputType){
        if(inputMappings.ContainsValue(inputType)){
            inputMappings[GetInputPlayer(inputType)] = InputType.None;
        }
        inputMappings.Add(playerNumber, inputType);
    }

    public static InputType GetPlayerInput(PlayerNumber playerNumber){
        return inputMappings[playerNumber];
    }

    public static PlayerNumber GetInputPlayer(InputType inputType){
        return inputMappings.FirstOrDefault(x => x.Value == inputType).Key;
    }

    public static void Reset(){
        inputMappings.Clear();
    }

}
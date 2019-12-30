using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class TutorialScreen : MonoBehaviour
{

    public TutorialAnimator tutorialAnimator;

    public TextMeshProUGUI tutorialText;

    [Header("Tutorial Steps")]

    public List<string> animationNameList;

    [TextArea(2,25)]
    public List<string> tutorialTextList;

    int stepIndex;


    void Start(){
        stepIndex = -1;
        GoToStep(0);
    }

    void Update() {
		if(Input.GetKeyDown(KeyCode.Escape)){
            ScreenManager.GoToTitleScreen();
		} else if (Input.GetButtonDown("ActionCtrl1") || Input.GetButtonDown("ActionCtrl1") || 
            Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.Return)){
            GoToStep(stepIndex+1);
        }
	}

    void GoToStep(int newStepIndex){
        if(animationNameList.Count <= newStepIndex){
            ScreenManager.GoToTitleScreen();
            return;
        }
        tutorialText.text = tutorialTextList[newStepIndex];
        if( stepIndex < 0 || animationNameList[stepIndex] != animationNameList[newStepIndex]){
            tutorialAnimator.StopGreetParticle();
            if(stepIndex >= 0)
                tutorialAnimator.animator.SetBool(animationNameList[stepIndex], false);
            tutorialAnimator.animator.SetBool(animationNameList[newStepIndex], true);
        }
        stepIndex = newStepIndex;
    }

}

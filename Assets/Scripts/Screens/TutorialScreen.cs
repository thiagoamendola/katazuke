using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class TutorialScreen : GenericScreen
{

    [Header("Tutorial Screen")]
    public GameObject globalCanvas;
    public TutorialAnimator tutorialAnimator;
    public TextMeshProUGUI tutorialText;

    [Header("Tutorial Steps")]

    public List<string> animationNameList;

    [TextArea(2,25)]
    public List<string> tutorialTextList;

    int stepIndex;

    bool exiting = false;

    public override void Open(){
        globalCanvas.SetActive(false);
        FocusCamera();
        StartCoroutine(OpenAsync());
    }

    IEnumerator OpenAsync(){
        yield return new WaitForSeconds(TRANSITIONDURATION);
        stepIndex = -1;
        GoToStep(0);
        globalCanvas.SetActive(true);
    }

    public override void Close(){

    }

    void Update() {
		if(!exiting && Input.GetKeyDown(KeyCode.Escape) && !ScreenManager.IsTransitioning()){
            globalCanvas.SetActive(false);
            ScreenManager.GoToTitleScreen();
            exiting = true;
		} else if (Input.GetButtonDown("ActionCtrl1") || Input.GetButtonDown("ActionCtrl1") || 
            Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.Return)){
            GoToStep(stepIndex+1);
        }
	}

    void GoToStep(int newStepIndex){
        if(animationNameList.Count <= newStepIndex){
            globalCanvas.SetActive(false);
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

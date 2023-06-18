using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TitleScreen : GenericScreen {

    [Header("Loading Screen")]
    public GameObject loadingScreen;

    [Header("UI")]
    public GameObject uiCanvas;

    [Header("Title Screen")]
    public GameObject credits;
    
    bool visibleCredits = false;

    
    public override void Open(){
        FocusCamera();
    }

    public override void Close(){

    }

    protected override void Start(){
        base.Start();
        StartCoroutine(StartAsync());
    }

    IEnumerator StartAsync(){
        if (!SceneManager.GetSceneByName("MainScenario").isLoaded) {
            loadingScreen.SetActive(true);
            yield return new WaitUntil(() => SceneManager.GetSceneByName("MainScenario").isLoaded);
            loadingScreen.SetActive(false);
        }
    }

    public void ButtonPlay() {
        // Ignore if scene is in the middle of a transition.
        if (ScreenManager.IsTransitioning()) {
            return;
        }
        // Disable UI
        uiCanvas.SetActive(false);
        // Call Input Screen
        ScreenManager.GoToInputScreen();
    }

    public void ButtonTutorial() {
        // Ignore if scene is in the middle of a transition.
        if (ScreenManager.IsTransitioning()) {
            return;
        }
        // Disable UI
        uiCanvas.SetActive(false);
        // Call Input Screen
        ScreenManager.GoToTutorialScreen();
    }

    public void ButtonCredits() {
        if (visibleCredits == true) {
            credits.gameObject.SetActive(false);
            visibleCredits = false;
        } else {
            credits.gameObject.SetActive(true);
            visibleCredits = true;

        }
    }


	public void ButtonExit(){
		Application.Quit();
	}


}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class ScreenManager : MonoBehaviour{

	public static Camera activeCamera;

	public static GenericScreen activeScreen;

	LoadSceneParameters loadSceneParameters;


	#region Singleton

	private static ScreenManager instance = null;

	// Gets Singleton instance.
	public static ScreenManager Instance {
		get{
			if (instance == null) {
				GameObject go = new GameObject("ScreenManager");
				instance = go.AddComponent<ScreenManager>();
				DontDestroyOnLoad(go);
			}
			return instance;
		}
	}

	private void Awake(){
		// If already on the scene, reference itself as the static instance.
		if (instance != this){
			instance = this;
			DontDestroyOnLoad(gameObject);
		}
		Setup();
	}

	#endregion

	#region Monobehaviour Methods

	public void Setup(){
		// Set up additive load scene parameter
		loadSceneParameters = new LoadSceneParameters(LoadSceneMode.Additive);
		// Get active scene, camera and screen, if necessary
		if (activeScreen == null)
			activeScreen = GameObject.FindObjectOfType<GenericScreen>();
		if (activeCamera == null)
			activeCamera = GameObject.FindObjectOfType<Camera>();
        	activeCamera.transform.SetParent(transform);
	}

	// IEnumerator AsyncStart(){
	//     Scene loadedScene = SceneManager.LoadScene("TestScreen", loadSceneParameters);
	//     yield return null; // Wait one frame for the scene to load
	//     print(loadedScene.GetRootGameObjects().Length.ToString());
	//     print(GameObject.Find("TestScreen"));
	//     activeScene = loadedScene;
	//     loadedScene.GetRootGameObjects()[0].GetComponent<GenericScreen>().Open();
	// }

	IEnumerator GoToScreen(string screenName){
		Scene oldScene = SceneManager.GetActiveScene();
		GenericScreen oldScreen = activeScreen;
		// Load new scene and set as active.
		Debug.Log(loadSceneParameters);
        print(oldScene.name);
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(screenName, loadSceneParameters);
		yield return asyncLoad;
		Scene loadedScene = SceneManager.GetSceneByName(screenName);
		SceneManager.SetActiveScene(loadedScene);
		// Get and open new screen
		activeScreen = loadedScene.GetRootGameObjects()[0].GetComponent<GenericScreen>();
		activeScreen.Open();
		// Close old screen.
		if (oldScreen != null)
		    oldScreen.Close();
        // Wait and unload old scene.
        yield return new WaitForSeconds(GenericScreen.transitionDuration);
        if(oldScene != null)
			print(oldScene.name);
            SceneManager.UnloadSceneAsync(oldScene);
    }

	#endregion

	#region Public Static Accessors

	public static void GoToTitleScreen(){
		Instance.StartCoroutine(Instance.GoToScreen("TitleScreen"));
	}

	public static void GoToInputScreen(){
		Instance.StartCoroutine(Instance.GoToScreen("InputScreen"));
	}

	public static void GoToGameScreen(){
		Instance.StartCoroutine(Instance.GoToScreen("GameScreen"));
	}


	#endregion

}

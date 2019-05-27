using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class ScreenManager : MonoBehaviour{

    public static Camera activeCamera;


    LoadSceneParameters loadSceneParameters;

    Scene loadedScene;

    void Start() {
        // Setup
        activeCamera = GameObject.Find("Camera").GetComponent<Camera>();
        loadSceneParameters = new LoadSceneParameters(LoadSceneMode.Additive);

        // Open
        StartCoroutine(AsyncStart());
    }

    IEnumerator AsyncStart(){
        loadedScene = SceneManager.LoadScene("TestScreen", loadSceneParameters);
        yield return null; // Wait one frame for the scene to load
        print(loadedScene.GetRootGameObjects().Length.ToString());
        print(GameObject.Find("TestScreen"));
        loadedScene.GetRootGameObjects()[0].GetComponent<GenericScreen>().Open();
    }

}

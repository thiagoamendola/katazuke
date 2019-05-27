using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class ScreenManager : MonoBehaviour{

    public static Camera activeCamera;

    public static Scene activeScene;

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

    #endregion

    #region Monobehaviour Methods

    void Start() {
        // Setup
        activeCamera = GameObject.Find("Camera").GetComponent<Camera>();
        loadSceneParameters = new LoadSceneParameters(LoadSceneMode.Additive);
        // Open
        StartCoroutine(AsyncStart());
    }

    IEnumerator AsyncStart(){
        Scene loadedScene = SceneManager.LoadScene("TestScreen", loadSceneParameters);
        yield return null; // Wait one frame for the scene to load
        print(loadedScene.GetRootGameObjects().Length.ToString());
        print(GameObject.Find("TestScreen"));
        activeScene = loadedScene;
        loadedScene.GetRootGameObjects()[0].GetComponent<GenericScreen>().Open();
    }

    #endregion

    #region Public Static Accessors


    #endregion

}

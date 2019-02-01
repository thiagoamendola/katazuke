using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class play : MonoBehaviour {

    public GameObject credits;
    public bool visible = false;

    // Start is called before the first frame update
    void Start() {
        
    }

    // Update is called once per frame
    void Update() {

    }

    public void ButtonPlay() {
        SceneManager.LoadScene("Stage1");
    }

    public void ButtonCredits() {
        if (visible == true) {
            credits.gameObject.SetActive(false);
            visible = false;
        } else {
            credits.gameObject.SetActive(true);
            visible = true;

        }
    }
}

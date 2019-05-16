using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TitleScreen : MonoBehaviour {

    public GameObject credits;
    public bool visible = false;

    public void ButtonPlay() {
        SceneManager.LoadScene("InputScreen");
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

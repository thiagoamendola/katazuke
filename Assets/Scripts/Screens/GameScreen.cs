using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class GameScreen : GenericScreen {

    public Vector3 characterFocusDistance = new Vector3(-5f,5f,0f);
    public float characterFocusSpeed = 4.0f;

    public float waitBeforeTitleScreen = 5.0f;

    public override void Open() {
        FocusCamera();
    }

    public override void Close() {

    }

    public void EndGame(Player winner){
        // Disable all particles.
        foreach(ParticleSystem ps in GameObject.FindObjectsOfType<ParticleSystem>())
            ps.Stop();
        // Animate winner and loser players.
        foreach(Player p in GameObject.FindObjectsOfType<Player>()){
            p.controlEnabled = false;
            if(p != winner)
                p.animator.SetTrigger("angry");
        }
        winner.animator.SetTrigger("praise");
        // Prepare texts on screen.
        GameObject.Find("WinMessage").GetComponent<TextMeshProUGUI>().text = winner.playerNumber.ToString()+" Wins";
        GameObject.Find("WinMessage").GetComponent<AudioSource>().Play();
        // Zoom camera into winner player.
        FocusWinner(winner.transform);
        // Wait to exit.
        StartCoroutine(EndGameAsync());
    }

    IEnumerator EndGameAsync(){
        yield return new WaitForSeconds(waitBeforeTitleScreen);
        GameObject.Find("WinMessage").SetActive(false);
        ScreenManager.GoToTitleScreen();
    }

    void FocusWinner(Transform winnerTransform){
        GameObject refObj = new GameObject("WinnerRef");
        refObj.transform.rotation = ScreenManager.activeCamera.transform.rotation;
        refObj.transform.localScale = ScreenManager.activeCamera.transform.localScale;
        refObj.transform.position = winnerTransform.position + characterFocusDistance;
        StartCoroutine(AsyncFocusCamera(ScreenManager.activeCamera, refObj.transform, characterFocusSpeed));
    }


}

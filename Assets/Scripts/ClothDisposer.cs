using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.Linq;
using TMPro;


public class ClothDisposer : InterationSpot {

    public const float DISCARDTIME = .75f;

    public bool requiresJoy;

    public override void TriggerInteraction(Player player)
    {
        if (player.playerNumber == playerNumber) {
            if (player.holdingObject != null && player.holdingObject.name == "FoldedCloth" && 
                requiresJoy == player.holdingObject.GetComponent<ClothInfo>().joy){
                // Fold cloth.
                Destroy(player.holdingObject);
                GameObject.Find("Music"+player.playerNumber.ToString()).GetComponent<AudioSource>().volume = 0.1f + (float)(player.totalClothesQuantity-player.clothesQuantity)/(float)player.totalClothesQuantity;
                GetComponent<AudioSource>().clip = AssetReferences.instance.clothDropClip;
                GetComponent<AudioSource>().Play();
                if(player.clothesQuantity <= 0){
                    Debug.Log("Go pray!");//<--
                    List <ClothDisposer> allDisposersList = new List<ClothDisposer>(GameObject.FindObjectsOfType<ClothDisposer>());
                    allDisposersList = allDisposersList.Where(cd => cd.playerNumber == player.playerNumber && !cd.requiresJoy).ToList();
                    allDisposersList[0].GetComponent<ParticleSystem>().Play();
                }
                player.animator.SetBool("hold", false);
                StartCoroutine(HaltForAnimation(player, DISCARDTIME));
            }else if(!requiresJoy && player.holdingObject == null && player.clothesQuantity <= 0){
                // Ending the game.
                foreach(ParticleSystem ps in GameObject.FindObjectsOfType<ParticleSystem>())
                    ps.Stop();
                // Animate winner and loser players.
                foreach(Player p in GameObject.FindObjectsOfType<Player>()){
                    p.controlEnabled = false;
                    if(p != player)
                        p.animator.SetTrigger("angry");
                }
                player.animator.SetTrigger("praise");
                GameObject.Find("WinMessage").GetComponent<TextMeshProUGUI>().text = player.playerNumber.ToString()+" Wins";
                GameObject.Find("WinMessage").GetComponent<AudioSource>().Play();
                StartCoroutine(ReturnToTitle());
            }
        }

    }

    IEnumerator ReturnToTitle(){
        yield return new WaitForSeconds(5f);
        ScreenManager.GoToGameScreen();
    }


}

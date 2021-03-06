using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.Linq;
using TMPro;


public class ClothDisposer : InteractionSpot {

    public const float DISCARDTIME = .75f;

    public bool requiresJoy;

    public override bool TriggerInteraction(Player player){
        if (player.playerNumber == playerNumber) {
            if (player.holdingObject != null && player.holdingObject.name == "FoldedCloth" && 
                requiresJoy == player.holdingObject.GetComponent<ClothInfo>().joy){
                // Trigger animation
                GetComponentsInChildren<Animator>()[1].SetTrigger("OpenClose");
                // Fold cloth.
                Destroy(player.holdingObject);
                GameObject.Find("Music"+player.playerNumber.ToString()).GetComponent<AudioSource>().volume = 0.1f + (float)(player.totalClothesQuantity-player.clothesQuantity)/(float)player.totalClothesQuantity;
                GetComponent<AudioSource>().clip = AssetReferences.instance.clothDropClip;
                GetComponent<AudioSource>().Play();
                if(player.clothesQuantity <= 0){
                    List <ClothDisposer> allDisposersList = new List<ClothDisposer>(GameObject.FindObjectsOfType<ClothDisposer>());
                    allDisposersList = allDisposersList.Where(cd => cd.playerNumber == player.playerNumber && !cd.requiresJoy).ToList();
                    allDisposersList[0].GetComponent<ParticleSystem>().Play();
                }
                player.animator.SetBool("hold", false);
                StartCoroutine(HaltForAnimation(player, DISCARDTIME));
                return true;
            }else if(!requiresJoy && player.holdingObject == null && player.clothesQuantity <= 0){
                // End the game.
                hint.Hide();
                ((GameScreen)ScreenManager.activeScreen).EndGame(player);
                return true;
            }
        }
        return false;
    }

    public override List<InteractionSpot> GetHintableNextSpots(){
        List<ClothesPile> list = new List<ClothesPile>(transform.parent.gameObject.GetComponentsInChildren<ClothesPile>());
        for(int i=list.Count-1 ; i >= 0 ; i--){
            if(list[i].playerNumber != playerNumber){
                list.RemoveAt(i);
            }
        }
        return list.Cast<InteractionSpot>().ToList();
    }


}

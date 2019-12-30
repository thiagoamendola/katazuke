using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialAnimator : MonoBehaviour
{

    public GameObject playerCharacter;
    public GameObject wardrobe;
    public GameObject discardBox;
    
    string lastPlayerAnim;

    public void TriggerPlayerAnimation(string animName){
        Animator playerAnimator = playerCharacter.GetComponent<Animator>();
        if (lastPlayerAnim != "")
            playerAnimator.SetBool(lastPlayerAnim, false);
        switch(animName){
            case "pick":
            case "fold":
                playerAnimator.ResetTrigger(animName);
                playerAnimator.SetTrigger(animName);
                lastPlayerAnim = "";
                break;
            default:
                playerAnimator.SetBool(animName, true);
                lastPlayerAnim = animName;
                break;
        }
    }

    public void PlayJoyParticle(bool joyful){
        List<ParticleSystem> pss = new List<ParticleSystem>(playerCharacter.GetComponentsInChildren<ParticleSystem>());
        if (joyful){
            pss[0].Play();
        }else{
            pss[1].Play();
        }
    }


    public void OpenWardrobe(){
        wardrobe.GetComponent<Animator>().SetTrigger("OpenClose");
    }

    public void OpenDiscardBox(){
        discardBox.GetComponent<Animator>().SetTrigger("OpenClose");
    }

}

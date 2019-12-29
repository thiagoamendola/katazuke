using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialAnimator : MonoBehaviour
{
    
    string lastPlayerAnim;

    public void TriggerPlayerAnimation(string animName){
        Animator playerAnimator = transform.Find("Player1").Find("Character").gameObject.GetComponent<Animator>();
        if (lastPlayerAnim != "")
            playerAnimator.SetBool(lastPlayerAnim, false);
        switch(animName){
            case "hold":
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




}

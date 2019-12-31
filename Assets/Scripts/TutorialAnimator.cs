using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class TutorialAnimator : MonoBehaviour
{

    [HideInInspector]
    public Animator animator;

    public GameObject playerCharacter;
    public GameObject wardrobe;
    public GameObject discardBox;
    
    string lastPlayerAnim;


    void Awake(){
        animator = GetComponent<Animator>();
        lastPlayerAnim = "";
    }

    void Start(){
        List<Animator> arrowAnimList = new List<Animator>(GetComponentsInChildren<Animator>().Where(a => a.name == "Arrow").Cast<Animator>().ToList());
        foreach (Animator arrowAnim in arrowAnimList){
            arrowAnim.SetBool("ShowLoop", true);
        }
    }

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


    public void OpenWardrobe(){
        wardrobe.GetComponent<Animator>().SetTrigger("OpenClose");
    }

    public void OpenDiscardBox(){
        discardBox.GetComponent<Animator>().SetTrigger("OpenClose");
    }

    public void ToggleGreetParticle(bool joyful){
        discardBox.GetComponent<Animator>().SetTrigger("OpenClose");
    }

    public void PlayGreetParticle(){
        discardBox.transform.parent.gameObject.GetComponent<ParticleSystem>().Play();
    }

    public void StopGreetParticle(){
        discardBox.transform.parent.gameObject.GetComponent<ParticleSystem>().Stop();
    }

}

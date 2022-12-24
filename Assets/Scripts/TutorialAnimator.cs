using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class TutorialAnimator : MonoBehaviour
{

    [HideInInspector]
    public Animator animator;
    public Animator playerAnimator;

    public GameObject playerCharacter;
    public GameObject wardrobe;
    public GameObject discardBox;
    
    void Awake(){
        animator = GetComponent<Animator>();
    }

    void Start(){
        List<Animator> arrowAnimList = new List<Animator>(GetComponentsInChildren<Animator>().Where(a => a.name == "Arrow").Cast<Animator>().ToList());
        foreach (Animator arrowAnim in arrowAnimList){
            arrowAnim.SetBool("ShowLoop", true);
        }
        playerAnimator = playerCharacter.GetComponent<Animator>();
    }

    public void PlayAnimator(string stateName){
        playerAnimator.Play(stateName);
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

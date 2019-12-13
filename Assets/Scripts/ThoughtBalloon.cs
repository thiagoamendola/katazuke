using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThoughtBalloon : MonoBehaviour
{

    public bool loop = false;

    void Update(){
        if(GetComponent<SpriteRenderer>() != null){
            transform.LookAt(ScreenManager.activeCamera.transform.position, -Vector3.up);
        }
    }

    public void Show(){
        gameObject.SetActive(true);
        if(!loop){
            if(!GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).IsName("thoughtballoon_show")){
                GetComponent<Animator>().SetTrigger("Show");
            }
        }else{
            GetComponent<Animator>().SetBool("ShowLoop", true);
        }
    }

    public void Hide(){
        if(loop){
            GetComponent<Animator>().SetBool("ShowLoop", false);
        }
        gameObject.SetActive(false);
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThoughtBalloon : MonoBehaviour
{

    private Color SELECTABLECOLOR = Color.white;

    public bool modulatable = true;

    public bool loop = false;

    Color mainColor;

    void Start(){
        if(modulatable)
            mainColor = GetComponent<SpriteRenderer>().color;
    }

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

    public void SetSelectable(bool inRange){
        if(inRange)
            GetComponent<SpriteRenderer>().color = SELECTABLECOLOR;
        else
            GetComponent<SpriteRenderer>().color = mainColor;
    }
}

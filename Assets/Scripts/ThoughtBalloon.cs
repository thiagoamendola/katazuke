using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThoughtBalloon : MonoBehaviour
{

    public bool loop = false;

    void Update(){
        if(GetComponent<SpriteRenderer>() != null)
            transform.LookAt(ScreenManager.activeCamera.transform.position, -Vector3.up);
    }
}

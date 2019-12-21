using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputSampler : MonoBehaviour
{

    public List<Animator> selectedInputIcons;

    
    public void SelectInput(int index){
        foreach(Animator selectedInputIcon in selectedInputIcons){
            selectedInputIcon.gameObject.GetComponent<CanvasGroup>().alpha = 0;
        }
        selectedInputIcons[index].gameObject.SetActive(true);
        selectedInputIcons[index].SetTrigger("SelectedInput");
    }

    public void ResetInput(){
        print("AAAAAAAAAAAAAAAAAAAAAAAAAAAAAAA");
        print(name);
        foreach(Animator selectedInputIcon in selectedInputIcons){
            selectedInputIcon.gameObject.SetActive(false);
            selectedInputIcon.ResetTrigger("SelectedInput");
            selectedInputIcon.gameObject.GetComponent<CanvasGroup>().alpha = 0; //<--
        }
    }

}

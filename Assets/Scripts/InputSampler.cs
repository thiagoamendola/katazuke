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
        selectedInputIcons[index].SetTrigger("SelectedInput");
    }

}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClothesPile : InterationSpot {

    public const float disappearAmount = 0.4f;

    public GameObject ClothPrefab;


    public override void TriggerInteraction(Player player){
        if(player.playerNumber == playerNumber){
            if(player.holdingObject == null && player.clothesQuantity>0){
                player.holdingObject = (GameObject) Instantiate(ClothPrefab, player.transform.Find("HoldingPoint").position, player.transform.rotation, player.transform);
                player.holdingObject.name = "Cloth";
                player.holdingObject.GetComponent<ClothInfo>().joy = Random.Range(0f,1f) > 0.5f;
                player.clothesQuantity--;
                if(player.clothesQuantity > 0)
                    transform.Translate(0,-disappearAmount/player.totalClothesQuantity,0);
                else
                    transform.localScale = Vector3.zero;
                player.animator.SetBool("hold", true);
            }
        }
    }


}

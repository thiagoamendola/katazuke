using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClothesPile : InterationSpot {

    public const float DISAPPEARAMOUNT = 0.4f;
    public const float PICKUPTIME = .75f;

    public GameObject ClothPrefab;


    public override bool TriggerInteraction(Player player){
        if(player.playerNumber == playerNumber){
            if(player.holdingObject == null && player.clothesQuantity>0){
                player.clothesQuantity--;
                if(player.clothesQuantity > 0)
                    transform.Translate(0,-DISAPPEARAMOUNT/player.totalClothesQuantity,0);
                else
                    transform.localScale = Vector3.zero;
                player.animator.SetBool("hold", true);
                StartCoroutine(HaltForAnimation(player, PICKUPTIME));
                return true;
            }
        }
        return false;
    }

    public override IEnumerator HaltForAnimation(Player player, float time){
        yield return base.HaltForAnimation(player, time);
        player.holdingObject = (GameObject) Instantiate(ClothPrefab, player.holdingPoint.transform.position, player.transform.rotation, player.holdingPoint.transform);
        player.holdingObject.name = "Cloth";
    }


}

using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ClothesPile : InterationSpot {

    public const float DISAPPEARAMOUNT = 0.4f;
    public const float PICKUPTIME = .75f;

    public GameObject ClothPrefab;


    void Start(){
        hint.Show();
    }

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


    public override List<InterationSpot> GetHintableNextSpots(){
        List<FoldingTable> list = new List<FoldingTable>(
            transform.parent.gameObject.GetComponentsInChildren<FoldingTable>());
        for(int i=list.Count-1 ; i >= 0 ; i--){
            if(list[i].playerNumber != playerNumber){
                list.RemoveAt(i);
            }
        }
        return list.Cast<InterationSpot>().ToList();
    }


}

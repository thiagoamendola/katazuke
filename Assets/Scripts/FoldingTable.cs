using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class FoldingTable : InteractionSpot{

    public const float FOLDINGTIME = 1.5f;

    public GameObject foldedClothPrefab;

    public override bool TriggerInteraction(Player player) {
        if (player.playerNumber == playerNumber) {
            if (player.holdingObject != null && player.holdingObject.name == "Cloth") {
                Destroy(player.holdingObject);
                player.holdingObject = (GameObject)Instantiate(foldedClothPrefab, player.holdingPoint.transform.position, player.transform.rotation, player.holdingPoint.transform);
                player.holdingObject.name = "FoldedCloth";
                bool joy = Random.Range(0f,1f) > 0.5f;
                player.holdingObject.GetComponent<ClothInfo>().joy = joy;
                player.animator.SetTrigger("fold");
                StartCoroutine(ShowThoughtBalloon(joy));
                StartCoroutine(HaltForAnimation(player, FOLDINGTIME));
                return true;
            }
        }
        return false;
    }

    IEnumerator ShowThoughtBalloon(bool joy){
        List<GameObject> balloonsList = joy ? AssetReferences.instance.joyBalloons : AssetReferences.instance.sadBalloons;
        GameObject balloonPrefab = balloonsList[(int)Random.Range((int)0, (int)balloonsList.Count)];
        GameObject balloon = (GameObject) Instantiate(balloonPrefab, transform.position + balloonPrefab.transform.position, Quaternion.identity, gameObject.transform);//<--
        yield return new WaitForSeconds(2f);//<--
        Destroy(balloon);
    }

    public override List<InteractionSpot> GetHintableNextSpots(){
        List<ClothDisposer> list = new List<ClothDisposer>(
            transform.parent.gameObject.GetComponentsInChildren<ClothDisposer>());
        List<int> removeList = new List<int>();
        for(int i=list.Count-1 ; i >= 0 ; i--){
            if(list[i].playerNumber != playerNumber){
                list.RemoveAt(i);
            }
        }
        return list.Cast<InteractionSpot>().ToList();
    }

}

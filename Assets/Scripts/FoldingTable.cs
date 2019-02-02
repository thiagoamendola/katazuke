using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FoldingTable : InterationSpot{

    public const float FOLDINGTIME = 1.5f;

    public GameObject foldedClothPrefab;

    public override void TriggerInteraction(Player player) {
        if (player.playerNumber == playerNumber) {
            if (player.holdingObject != null && player.holdingObject.name == "Cloth") {
                bool joy = player.holdingObject.GetComponent<ClothInfo>().joy;
                Destroy(player.holdingObject);
                player.holdingObject = (GameObject)Instantiate(foldedClothPrefab, player.holdingPoint.transform.position, player.transform.rotation, player.holdingPoint.transform);
                player.holdingObject.name = "FoldedCloth";
                player.holdingObject.GetComponent<ClothInfo>().joy = joy;
                player.animator.SetTrigger("fold");
                StartCoroutine(ShowThoughtBalloon(joy));
                StartCoroutine(HaltForAnimation(player, FOLDINGTIME));
            }
        }
    }

    IEnumerator ShowThoughtBalloon(bool joy){
        List<GameObject> balloonsList = joy ? AssetReferences.instance.joyBalloons : AssetReferences.instance.sadBalloons;
        GameObject balloonPrefab = balloonsList[(int)Random.Range((int)0, (int)balloonsList.Count)];
        GameObject balloon = (GameObject) Instantiate(balloonPrefab, transform.position + balloonPrefab.transform.position, Quaternion.identity, gameObject.transform);//<--
        yield return new WaitForSeconds(2f);//<--
        Destroy(balloon);
    }

}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FoldingTable : InterationSpot{

    public GameObject FoldedClothPrefab;

    public override void TriggerInteraction(Player player)
    {
        if (player.playerNumber == playerNumber) {
            if (player.holdingObject != null && player.holdingObject.name == "Cloth") {
                bool joy = player.holdingObject.GetComponent<ClothInfo>().joy;
                Destroy(player.holdingObject);
                player.holdingObject = (GameObject)Instantiate(FoldedClothPrefab, player.transform.Find("HoldingPoint").position, player.transform.rotation, player.transform);
                player.holdingObject.name = "FoldedCloth";
                player.holdingObject.GetComponent<ClothInfo>().joy = joy;
                Debug.Log("Joy: "+joy);
                StartCoroutine(ShowThoughtBalloon(joy));
                player.animator.SetTrigger("fold");
                // Play animation. //<--
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

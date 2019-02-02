using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class InterationSpot : MonoBehaviour {

    public PlayerNumber playerNumber;

    public abstract void TriggerInteraction(Player player);

    public virtual IEnumerator HaltForAnimation(Player player, float time) {
        Debug.Log("WOW "+time.ToString());
        player.softControlEnabled = false;
        yield return new WaitForSeconds(time);
        player.softControlEnabled = true;
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class InterationSpot : MonoBehaviour {

    public PlayerNumber playerNumber;

    public abstract bool TriggerInteraction(Player player);

    public virtual IEnumerator HaltForAnimation(Player player, float time) {
        player.softControlEnabled = false;
        yield return new WaitForSeconds(time);
        player.softControlEnabled = true;
    }
}

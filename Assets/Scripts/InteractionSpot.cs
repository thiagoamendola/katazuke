using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class InteractionSpot : MonoBehaviour {

    public PlayerNumber playerNumber;

    public ThoughtBalloon hint;

    public abstract bool TriggerInteraction(Player player);

    public abstract List<InteractionSpot> GetHintableNextSpots();

    public virtual IEnumerator HaltForAnimation(Player player, float time) {
        player.softControlEnabled = false;
        yield return new WaitForSeconds(time);
        player.softControlEnabled = true;
    }

}

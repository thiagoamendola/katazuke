using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class InterationSpot : MonoBehaviour {

    public PlayerNumber playerNumber;

    public abstract void TriggerInteraction(Player player);

}

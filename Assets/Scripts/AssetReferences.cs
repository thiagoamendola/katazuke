using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AssetReferences : MonoBehaviour {

    public static AssetReferences instance;

    [Header("Prefabs")]
    public List<GameObject> joyBalloons;
    public List<GameObject> sadBalloons;

    [Header("Audio Clips")]
    public AudioClip clothDropClip;

    void Awake() {
        instance = this;
    }

}
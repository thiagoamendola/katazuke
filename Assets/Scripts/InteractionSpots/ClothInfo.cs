using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClothInfo : MonoBehaviour {

	const float timeBeforeParticle = 1.5f;

	public bool joy;

	public ParticleSystem joyfulParticles;
	public ParticleSystem joylessParticles;

	void Start(){
		StartCoroutine("StartAsync");
	}

	IEnumerator StartAsync(){
		yield return new WaitForSeconds(timeBeforeParticle);
		if (joy){
			joyfulParticles.Play();
		}else{
			joylessParticles.Play();
		}
	}

}

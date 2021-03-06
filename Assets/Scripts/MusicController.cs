﻿using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MusicController : MonoBehaviour {

	private AudioSource audioSource;

	#region Singleton

	private static MusicController instance = null;

	// Gets Singleton instance.
	public static MusicController Instance {
		get{
			if (instance == null) {
				GameObject go = new GameObject("MusicController");
				instance = go.AddComponent<MusicController>();
				go.AddComponent<AudioSource>();
				DontDestroyOnLoad(go);
			}
			return instance;
		}
	}

	private void Awake(){
		// If already on the scene, reference itself as the static instance.
		if (instance == null){
			instance = this;
			DontDestroyOnLoad(gameObject);
		}else if(instance != this){
			// Before autodestroying, call the instance and pass its values
			// if (GetComponent<AudioSource>() != null)
			instance.GetComponent<AudioSource>().Stop();
			Destroy(instance.gameObject);
			instance = this;
			DontDestroyOnLoad(gameObject);
        }
		audioSource = GetComponent<AudioSource>();
	}

	#endregion

	public void UpdateAudioSource(AudioSource sceneAudioSource){
		audioSource.Stop();
		audioSource.clip = sceneAudioSource.clip;
		audioSource.mute = sceneAudioSource.mute;
		audioSource.playOnAwake = sceneAudioSource.playOnAwake;
		audioSource.loop = sceneAudioSource.loop;
		audioSource.priority = sceneAudioSource.priority;
		audioSource.volume = sceneAudioSource.volume;
		audioSource.pitch = sceneAudioSource.pitch;
		audioSource.Play();
	}

}

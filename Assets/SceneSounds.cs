using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

public class SceneSounds : MonoBehaviour {
    private AudioClip[] buttonSounds, interactSounds, BGMs;
    private AudioSource audioSourceNormal = null, audioSourceLoop = null;
    private System.Random random;
    private bool isSceneSoundsReady = false;
    
    void Start() {
        string listenPosGameObjectName = "Main Camera";
        GameObject sceneAudio = GameObject.Find(listenPosGameObjectName);
        if (sceneAudio == null) sceneAudio = new GameObject(listenPosGameObjectName);
        audioSourceNormal = sceneAudio.GetComponent<AudioSource>();
        if (audioSourceNormal == null) audioSourceNormal = sceneAudio.AddComponent<AudioSource>();
        audioSourceNormal.loop = false;
        audioSourceLoop = sceneAudio.AddComponent<AudioSource>(); audioSourceLoop.loop = true;
        AudioListener audioListener = sceneAudio.GetComponent<AudioListener>();
        if (audioListener == null) audioListener = sceneAudio.AddComponent<AudioListener>();
        buttonSounds = new AudioClip[7];
        interactSounds = new AudioClip[3];
        BGMs = new AudioClip[5];
        for (int i = 0; i < 7; i++)
            buttonSounds[i] = Resources.Load<AudioClip>($"scene_button_0{i+1}");
        for (int i = 0; i < 3; i++)
            interactSounds[i] = Resources.Load<AudioClip>($"scene_interact_0{i+1}");
        BGMs[0] = Resources.Load<AudioClip>("home_talking_bgm");
        BGMs[1] = Resources.Load<AudioClip>("main_game_bgm_01");
        BGMs[2] = Resources.Load<AudioClip>("dark");
        BGMs[3] = Resources.Load<AudioClip>("dark_run");
        BGMs[4] = Resources.Load<AudioClip>("bgm_final");
        random = new System.Random();
        isSceneSoundsReady = true;
    }

    public void PlayButtonSound() {
        if (!isSceneSoundsReady) Start();
        audioSourceNormal.PlayOneShot(buttonSounds[0], 1f);
    }

    public void PlayButtonSoundNPC() {
        if (!isSceneSoundsReady) Start();
        audioSourceNormal.PlayOneShot(buttonSounds[random.Next(0, buttonSounds.Length)], 1f);
    }

    public void PlayClickSoundNPC() {
        if (!isSceneSoundsReady) Start();
        audioSourceNormal.PlayOneShot(buttonSounds[random.Next(0, buttonSounds.Length)], 1f);
    }

    public void PlayInteractSound() {
        if (!isSceneSoundsReady) Start();
        audioSourceNormal.PlayOneShot(interactSounds[random.Next(0, interactSounds.Length)], 1f);
    }

    public void PlayBGMSound(int idx) {
        if (!isSceneSoundsReady) Start();
        audioSourceLoop.clip = BGMs[idx];
        audioSourceLoop.Play();
    }

    public void PlayBGMOnce(int idx) {
        if (!isSceneSoundsReady) Start();
        audioSourceNormal.PlayOneShot(BGMs[idx], 1f);
    }

    public float GetBGMSoundVolume() {
        if (!isSceneSoundsReady) Start();
        return audioSourceLoop.volume;
    }

    public void SetBGMSoundVolume(float volume) {
        if (!isSceneSoundsReady) Start();
        audioSourceLoop.volume = volume;
    }
}

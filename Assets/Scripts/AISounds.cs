using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using System.Linq;


public class AISounds : MonoBehaviour {
    
    public AudioClip LandingAudioClip;
    public AudioClip[] FootstepAudioClips;
    private AudioSource audioSource = null;
    private bool isInitialized = false;

    void Start() {
        audioSource = GetComponent<AudioSource>();
        if (isInitialized) return;
        isInitialized = true;
        // string footstepPath = "Assets/StarterAssets/ThirdPersonController/Character/Sfx";
        string landingClipName = "Player_Land";
        string footstepClipNames = "Player_Footstep_0";

        // Load the landing audio clip
        // LandingAudioClip = AssetDatabase.LoadAssetAtPath<AudioClip>($"{footstepPath}/{landingClipName}");
        LandingAudioClip = Resources.Load<AudioClip>($"{landingClipName}");

        // Load the footstep audio clips
        FootstepAudioClips = new AudioClip[10];
        for (int i = 0; i < 9; i++) {
            //  FootstepAudioClips[i] = AssetDatabase.LoadAssetAtPath<AudioClip>($"{footstepPath}/{footstepClipNames}{i + 1}.wav");
            FootstepAudioClips[i] = Resources.Load<AudioClip>($"{footstepClipNames}{i + 1}");
        }
        FootstepAudioClips[9] = Resources.Load<AudioClip>("Player_Footstep_10");
    }

    private void OnFootstep(AnimationEvent animationEvent) {
        if (audioSource.enabled) {
            if (animationEvent.animatorClipInfo.weight > 0.5f) {
                if (FootstepAudioClips.Length > 0) {
                    var index = Random.Range(0, FootstepAudioClips.Length);
                    audioSource.PlayOneShot(FootstepAudioClips[index], 1f);
                }
            }
        }
    }

    private void OnLand(AnimationEvent animationEvent) {
        if (audioSource.enabled) {
            if (animationEvent.animatorClipInfo.weight > 0.5f) {
                audioSource.PlayOneShot(LandingAudioClip, 1f);
            }
        }
    }

}

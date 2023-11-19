using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEditor; // This is only for use within the Unity Editor
using System.Linq;


public class AISounds : MonoBehaviour {
    
    private AudioClip LandingAudioClip;
    private AudioClip[] FootstepAudioClips;
    private AudioSource audioSource;
    private bool isInitialized = false;

    void Update() {
        if (isInitialized) return;
        isInitialized = true;
        string footstepPath = "Assets/StarterAssets/ThirdPersonController/Character/Sfx";
        string landingClipName = "Player_Land.wav";
        string footstepClipNames = "Player_Footstep_0";

        // Load the landing audio clip
        LandingAudioClip = AssetDatabase.LoadAssetAtPath<AudioClip>($"{footstepPath}/{landingClipName}");

        // Load the footstep audio clips
        FootstepAudioClips = new AudioClip[10];
        for (int i = 0; i < 9; i++) {
            FootstepAudioClips[i] = AssetDatabase.LoadAssetAtPath<AudioClip>($"{footstepPath}/{footstepClipNames}{i+1}.wav");
        }
        FootstepAudioClips[9] = AssetDatabase.LoadAssetAtPath<AudioClip>($"{footstepPath}/Player_Footstep_10.wav");
        audioSource = GetComponent<AudioSource>();
    }

    private void OnFootstep(AnimationEvent animationEvent) {
        if (animationEvent.animatorClipInfo.weight > 0.5f) {
            if (FootstepAudioClips.Length > 0) {
                var index = Random.Range(0, FootstepAudioClips.Length);
                AudioSource.PlayClipAtPoint(FootstepAudioClips[index], transform.position, 1); // Use the class name here
            }
        }
    }

    private void OnLand(AnimationEvent animationEvent) {
        if (animationEvent.animatorClipInfo.weight > 0.5f) {
            AudioSource.PlayClipAtPoint(LandingAudioClip, transform.position, 1); // Use the class name here
        }
    }

}

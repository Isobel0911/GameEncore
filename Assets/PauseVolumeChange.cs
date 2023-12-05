using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PauseVolumeChange : MonoBehaviour {

    public Slider slider;
    private AudioSource audioSource = null;

    // Start is called before the first frame update

    // Update is called once per frame
    void Update() {
        if (audioSource == null) {
            audioSource = GameObject.Find("In-Game Transition")?.GetComponent<SceneSounds>()?.audioSourceLoop;
        }
        if (audioSource != null) slider.value = audioSource.volume;
        else slider.value = 1f;
        slider.onValueChanged.AddListener(OnSliderValueChanged);
    }

    public void OnSliderValueChanged(float value) {
        if (audioSource != null) audioSource.volume = value;
    }
}

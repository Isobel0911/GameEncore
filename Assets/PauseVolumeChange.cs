using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PauseVolumeChange : MonoBehaviour {

    public Slider slider;
    private AudioSource audioSource = null;
    public GameObject texts;
    private Text myText = null;

    // Start is called before the first frame update
    void Awake() {
        if (slider == null) slider = slider.GetComponent<Slider>();
        if (slider != null) slider.value = 1f;
    }

    // Update is called once per frame
    void Update() {
        if (audioSource == null) {
            audioSource = GameObject.Find("In-Game Transition")?.GetComponent<SceneSounds>()?.audioSourceLoop;
        }
        if (myText == null) {
            myText = texts?.GetComponent<Text>();
        }
        if (audioSource != null && slider != null && audioSource.volume != slider.value) {
            audioSource.volume = slider.value;
            if (myText != null) myText.text = (slider.value * 100).ToString("F2") + "%";
        }
        slider.onValueChanged.AddListener(OnSliderValueChanged);
    }

    public void OnSliderValueChanged(float value) {
        if (audioSource != null) audioSource.volume = value;
        if (myText != null) myText.text = (slider.value * 100).ToString("F2") + "%";
    }
}

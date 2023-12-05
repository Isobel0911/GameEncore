using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseVolumeToggle : MonoBehaviour {
    
    public bool isVolumeEnabled = false;
    public GameObject original, volumeOne;

    // Update is called once per frame
    void Update() {
        original?.SetActive(!isVolumeEnabled);
        volumeOne?.SetActive(isVolumeEnabled);
    }

    public void switching() {
        isVolumeEnabled = !isVolumeEnabled;
    }
}

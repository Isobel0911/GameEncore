using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomSubDetector : MonoBehaviour {
    private RoomSoundController controller;
    public int idx;

    void OnTriggerStay(Collider other) {
        if (other.gameObject.name == "PlayerArmature") {
            controller.ableMain(idx);
        } else {
            controller.ableNPC(other.gameObject.name, idx);
        }
    }
    void OnTriggerExit(Collider other) {
        if (other.gameObject.name == "PlayerArmature") {
            controller.disableMain(idx);
        } else {
            controller.disableNPC(other.gameObject.name, idx);
        }
    }
    public void SetController(RoomSoundController rs) {
        this.controller = rs;
    }
}

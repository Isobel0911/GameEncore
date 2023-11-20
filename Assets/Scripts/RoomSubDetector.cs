using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomSubDetector : MonoBehaviour {
    private RoomSoundController controller;
    public int idx;

    void OnTriggerStay(Collider other) {
        if (other.gameObject.name == "PlayerArmature") {
            Debug.Log($"Able Main: {controller.getName()}.{this.gameObject.name}");
            controller.ableMain(idx);
        } else {
            Debug.Log($"Able NPC: {controller.getName()}.{this.gameObject.name}\t|\t{other.gameObject.name}");
            controller.ableNPC(other.gameObject.name, idx);
        }
    }
    void OnTriggerExit(Collider other) {
        if (other.gameObject.name == "PlayerArmature") {
            Debug.Log($"Disable Main: {controller.getName()}.{this.gameObject.name}");
            controller.disableMain(idx);
        } else {
            Debug.Log($"Disable NPC: {controller.getName()}.{this.gameObject.name}\t|\t{other.gameObject.name}");
            controller.disableNPC(other.gameObject.name, idx);
        }
    }
    public void SetController(RoomSoundController rs) {
        this.controller = rs;
    }
}

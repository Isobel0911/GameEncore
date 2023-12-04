using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class Interactor : MonoBehaviour {
    [SerializeField] private Transform _interactionPoint;
    [SerializeField] private float _interactionPointRadius = 0.8f;

    private readonly Collider[] _colliders = new Collider[4];
    [SerializeField] private int _numFound;
    private Dictionary<int, int> dict;
    private string typeInt;
    [HideInInspector]public GameObject doorUI, otherUI;//, pickUI;
    private InventorySelf selfInteractor;

    private void Start() {
        dict = new Dictionary<int, int>();
        dict.Add(1, 2); dict.Add(2, 1);
        dict.Add(3, 4); dict.Add(4, 3);
        dict.Add(7, 8); dict.Add(8, 7);
        dict.Add(9, 10); dict.Add(10, 9);
        dict.Add(13, 14); dict.Add(14, 13);
        dict.Add(17, 18); dict.Add(18, 17);
        typeInt = "D2";
        doorUI = GameObject.Find("Canvas/SafeAreaPanel/NotifyMsg/OpenDoor");
        // pickUI = GameObject.Find("Canvas/NotifyMsg/PickUp");
        otherUI = GameObject.Find("Canvas/SafeAreaPanel/NotifyMsg/Other");
        selfInteractor = this.gameObject.GetComponent<InventorySelf>();
    }
    
    private void Update() {
        _numFound = Physics.OverlapSphereNonAlloc(
            _interactionPoint.position, _interactionPointRadius, _colliders, (1 << LayerMask.NameToLayer("Interactable"))
            );
        // take only one interactable
        if (_numFound > 0) {
            if (checkDoor(_colliders, _numFound)) {

                if (selfInteractor.hasKey1) {
                    if (!doorUI.activeSelf) doorUI.SetActive(true);
                } else {
                    if (doorUI.activeSelf) doorUI.SetActive(false);
                }
                // if (pickUI.activeSelf) pickUI.SetActive(false);
                if (otherUI.activeSelf) otherUI.SetActive(false);
                HashSet<int> doubleDoors = new HashSet<int>();

                for (int i = 0; i < _numFound; i++) {
                    string colliderName = _colliders[i].gameObject.name;
                    if (!colliderName.StartsWith("SM_Env_Door_")) continue;
                    var interactable = _colliders[i].GetComponent<IInteractable>();
                    int doorIdx = 0;
                    int.TryParse(colliderName.Substring(colliderName.Length - 2), out doorIdx);
                    if (interactable != null && Input.GetKeyDown(KeyCode.E)) {
                        if (dict.ContainsKey(doorIdx)) {
                            if (doubleDoors.Contains(dict[doorIdx])) {
                                doubleDoors.Remove(dict[doorIdx]);
                            } else {
                                doubleDoors.Add(doorIdx);
                            }
                        }
                        interactable.Interact(this);
                    }
                }
                foreach (int doorIdx in doubleDoors) {
                    int newDoorIdx = dict[doorIdx];
                    string doorName = $"SM_Env_Door_{newDoorIdx.ToString(typeInt)}";
                    GameObject door = GameObject.Find(doorName);
                    var interactable = door.GetComponent<IInteractable>();
                    if (interactable != null && Input.GetKeyDown(KeyCode.E)) {
                        interactable.Interact(this);
                    }
                }
            } else {
                var interactable = _colliders[0].GetComponent<IInteractable>();
                if (doorUI.activeSelf) doorUI.SetActive(false);
                // if (_colliders[0].gameObject.name == "SM_Prop_Plant_13") {
                // // if (_colliders[0].gameObject.tag == "keyplant") {
                //     var player = gameObject.GetComponent<InventorySelf>();
                //     if (player == null || player.hasKey1) return;
                //     if (pickUI.activeSelf) pickUI.SetActive(false);
                //     if (otherUI.activeSelf) otherUI.SetActive(false);
                // } else if (_colliders[0].gameObject.name == "SM_Prop_Computer_03" ||
                //            _colliders[0].gameObject.name == "FrontDesk_Boss") {
                //     if (pickUI.activeSelf) pickUI.SetActive(false);
                //     if (!otherUI.activeSelf) otherUI.SetActive(true);
                // } else {
                //     if (!pickUI.activeSelf) pickUI.SetActive(true);
                //     if (otherUI.activeSelf) otherUI.SetActive(false);
                // }
                if (!otherUI.activeSelf) otherUI.SetActive(true);
                if (_colliders[0].gameObject.CompareTag("plant")
                        || _colliders[0].gameObject.CompareTag("keyplant"))
                {
                    if(!FindObjectOfType<DialogueManager>().talkedToJessica
                        || GameObject.Find("NestedParentArmature_Unpack/PlayerArmature").GetComponent<InventorySelf>().hasKey1)
                    {
                        otherUI.SetActive(false);
                    }
                    
                }
                if (_colliders[0].gameObject.CompareTag("computer"))
                {
                    if(PuzzleInteract.hasStarted && !PuzzleInteract.hasSolved)
                    otherUI.SetActive(false);
                }
                if (interactable != null && Input.GetKeyDown(KeyCode.E)) {
                    interactable.Interact(this);
                }
            }
        } else {
            if (doorUI.activeSelf) doorUI.SetActive(false);
            // if (pickUI.activeSelf) pickUI.SetActive(false);
            if (otherUI.activeSelf) otherUI.SetActive(false);
        }
    }

    private bool checkDoor(Collider[] _colliders, int _numFound) {
        for (int i = 0; i < _numFound; i++) {
            if (_colliders[i].gameObject.name.StartsWith("SM_Env_Door_") ||
                _colliders[i].gameObject.name == "SM_Env_VaultDoor_Lid_01") {
                return true;
            }
        }
        return false;
    }

    private void OnDrawGizmos() {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(_interactionPoint.position, _interactionPointRadius);
    }
}
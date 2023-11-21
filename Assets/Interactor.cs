using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class Interactor : MonoBehaviour
{
    [SerializeField] private Transform _interactionPoint;
    [SerializeField] private float _interactionPointRadius = 0.8f;

    private readonly Collider[] _colliders = new Collider[4];
    [SerializeField] private int _numFound;
    private Dictionary<int, int> dict;
    private string typeInt;

    private void Start() {
        dict = new Dictionary<int, int>();
        dict.Add(1, 2); dict.Add(2, 1);
        dict.Add(3, 4); dict.Add(4, 3);
        dict.Add(7, 8); dict.Add(8, 7);
        dict.Add(9, 10); dict.Add(10, 9);
        dict.Add(13, 14); dict.Add(14, 13);
        dict.Add(17, 18); dict.Add(18, 17);
        typeInt = "D2";
    }
    
    private void Update() {
        _numFound = Physics.OverlapSphereNonAlloc(
            _interactionPoint.position, _interactionPointRadius, _colliders, (1 << LayerMask.NameToLayer("Interactable"))
            );
        // take only one interactable
        if (_numFound > 0) {
            if (checkDoor(_colliders, _numFound)) {
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
                if (interactable != null && Input.GetKeyDown(KeyCode.E)) {
                    interactable.Interact(this);
                }
            }
        }
    }

    private bool checkDoor(Collider[] _colliders, int _numFound) {
        for (int i = 0; i < _numFound; i++) {
            if (_colliders[i].gameObject.name.StartsWith("SM_Env_Door_")) {
                return true;
            }
        }
        return false;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(_interactionPoint.position, _interactionPointRadius);
    }
}
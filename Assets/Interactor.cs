using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class Interactor : MonoBehaviour
{
    [SerializeField] private Transform _interactionPoint;
    [SerializeField] private float _interactionPointRadius = 0.8f;
    [SerializeField] private LayerMask _interactionMask;

    private readonly Collider[] _colliders = new Collider[4];
    [SerializeField] private int _numFound;
    
    private void Update()
    {
        _numFound = Physics.OverlapSphereNonAlloc(
            _interactionPoint.position, _interactionPointRadius, _colliders, _interactionMask
            );
        // take only one interactable
        if (_numFound > 0)
        {
            //Debug.Log("In interactor");
            var interactble = _colliders[0].GetComponent<IInteractable>();

            if (interactble != null && Input.GetKeyDown(KeyCode.E))
            {
                interactble.Interact(this);
            }
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(_interactionPoint.position, _interactionPointRadius);
    }
}
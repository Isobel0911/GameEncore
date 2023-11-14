using UnityEngine;
using UnityEditor.AI;
using System.Collections;
using System.Collections.Generic;
using NavMeshBuilder = UnityEditor.AI.NavMeshBuilder;

public class NavMeshBaker : MonoBehaviour {

    public void Bake() {
        NavMeshBuilder.ClearAllNavMeshes();
        NavMeshBuilder.BuildNavMesh();
    }
}


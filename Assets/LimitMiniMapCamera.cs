using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LimitMiniMapCamera : MonoBehaviour
{
    public GameObject Player;

    private void LateUpdate()
    {
        transform.position = new Vector3(Player.transform.position.x, 6.2f, Player.transform.position.z);
    }
}

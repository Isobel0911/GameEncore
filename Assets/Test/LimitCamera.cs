using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LimitCamera : MonoBehaviour
{
    public GameObject Player;

    private void LateUpdate()
    {
        this.transform.position = new Vector3(Player.transform.position.x, 8.7f, Player.transform.position.z);
    }
}

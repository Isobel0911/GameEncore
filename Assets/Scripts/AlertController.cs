using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AlertController : MonoBehaviour
{
    public ProgressBar pb;
    public int alert = 0;

    // Update is called once per frame
    void Update()
    {
        pb.BarValue = alert;
    }
}

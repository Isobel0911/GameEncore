using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class AlertController : MonoBehaviour
{
    public ProgressBar pb;
    public int alert = 0;

    public TMP_Text moneyText;
    public int money = 0;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        moneyText.text = money.ToString();

        pb.BarValue = alert;
    }
}

using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class DamagePoint : MonoBehaviour
{
    public int Point { get; set; }
    private TMP_Text pointText; 

    void Start()
    {
        pointText = GetComponent<TMP_Text>();
        pointText.text = Point.ToString();
    }

    public void Distroy()
    {
        Destroy(this.gameObject);
    }
}

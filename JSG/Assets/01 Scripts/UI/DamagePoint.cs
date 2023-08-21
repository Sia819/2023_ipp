using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

/// <summary>
/// 받은 데미지 포인트의 UI스크립트 입니다.
/// </summary>
public class DamagePoint : MonoBehaviour
{
    public int Point { get; set; }

    private TMP_Text pointText; 

    /// <summary> 오브젝트가 생성될 때 포인트가 표시되도록 받은 데미지의 값을 가져와 설정합니다. </summary>
    private void Start()
    {
        pointText = GetComponent<TMP_Text>();
        pointText.text = Point.ToString();
    }

    /// <summary> 애니메이션으로 객체를 파괴할 클립 함수입니다. </summary>
    public void Distroy()
    {
        Destroy(this.gameObject);
    }
}

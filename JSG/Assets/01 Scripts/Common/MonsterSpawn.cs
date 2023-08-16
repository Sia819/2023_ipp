using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UIElements;

public class MonsterSpawn : MonoBehaviour
{
    [SerializeField] private BoxCollider spawnFloor;
    [SerializeField] private BoxCollider spawnHole;
    [SerializeField] private Transform player;
    [SerializeField] private GameObject mobZomBear;
    [SerializeField] private GameObject mobZomBunny;
    [SerializeField] private GameObject mobHellephant;
    [SerializeField] private int spawnMax = 50;
    [SerializeField] private float spawnRate = 1.2f;

    private WaitForSeconds spawnTime;

    void Start()
    {
        spawnTime = new WaitForSeconds(spawnRate);
        GameManager.Instance.OnGameStarted += ResetMonster;
        GameManager.Instance.OnStageStarted += SpawnMonster;
    }

    // 게임이 초기화 되었을 때, 모든 몬스터를 삭제합니다.
    private void ResetMonster(object sender, EventArgs args)
    {
        foreach (Transform child in transform)
        {
            Destroy(child.gameObject);
        }
    }

    private void SpawnMonster(object sender, EventArgs args)
    {
        StartCoroutine(Spawn());
    }

    /// <summary> 몬스터를 생성합니다. </summary>
    private IEnumerator Spawn()
    {
        while (GameManager.Instance.IsPlaying)
        {
            if (spawnMax <= GameManager.Instance.MonsterCount)
            {
                yield return spawnTime;
                continue;
            }

            Vector3 spawnPoint;
            int rand = RandomRatio(3, 7);

            if (rand == 1) // 쥐구멍에서 몬스터 스폰
            {
                spawnPoint = GenRandomPoint(spawnHole, false);

                rand = RandomRatio(4, 6);
                if (rand == 1) Instantiate(mobZomBunny, spawnPoint, Quaternion.identity).transform.SetParent(this.transform);
                else  Instantiate(mobZomBear, spawnPoint, Quaternion.identity).transform.SetParent(this.transform);
            }
            else // 바닥에서 몬스터 생성
            {
                spawnPoint = GenRandomPoint(spawnFloor, true);

                rand = RandomRatio(2, 6, 8);
                if (rand == 1) Instantiate(mobHellephant, spawnPoint, Quaternion.identity).transform.SetParent(this.transform);
                else if (rand == 2) Instantiate(mobZomBear, spawnPoint, Quaternion.identity).transform.SetParent(this.transform);
                else
                {
                    Instantiate(mobZomBunny, spawnPoint, Quaternion.identity).transform.SetParent(this.transform);
                }
            }
            GameManager.Instance.MonsterCount++;
            yield return spawnTime;
        }
    }

    /// <summary>
    /// 파라미터에 해당하는 비율에 따라 랜덤으로 값을 반환하는 함수 입니다.
    /// </summary>
    /// <param name="ratios"> 비율에 해당하는 파라미터 입니다. </param>
    /// <returns> 예를들어 RandomRatio(3, 4, 1) 을 입력했다면, 3의 확률로 1, 4의 확률로 2, 1의 확률로 3이 반환됩니다. </returns>
    public int RandomRatio(params int[] ratios)
    {
        int totalRate = 0;
        foreach (int ratio in ratios)
        {
            totalRate += ratio;
        }

        int randomValue = UnityEngine.Random.Range(0, totalRate);
        int accumulatedRate = 0;

        for (int i = 0; i < ratios.Length; i++)
        {
            accumulatedRate += ratios[i];
            if (randomValue < accumulatedRate)
            {
                return i + 1;
            }
        }

        return -1; // 이 경우는 발생하지 않아야 합니다. 잘못된 비율 입력의 경우를 처리하기 위한 예외 값.
    }

    /// <summary> 몬스터를 스폰할 지점을 생성합니다. </summary>
    /// <returns> 스폰가능한 영역의 좌표를 리턴합니다. </returns>
    private Vector3 GenRandomPoint(BoxCollider spawnArea, bool considerCamera)
    {
        Vector3 spawnPoint;

        do
        {
            // 로컬 공간에서 랜덤한 점을 생성합니다.
            float x = UnityEngine.Random.Range(-spawnArea.size.x / 2, spawnArea.size.x / 2);
            float z = UnityEngine.Random.Range(-spawnArea.size.z / 2, spawnArea.size.z / 2);
            Vector3 localPoint = new Vector3(x, 0, z);

            // 로컬 공간에서의 점을 월드 공간으로 변환합니다.
            spawnPoint = spawnArea.transform.TransformPoint(localPoint);
        }
        while (IsPointInExpertArea(spawnPoint, spawnArea) && IsPointInCameraView(spawnPoint, considerCamera));

        return spawnPoint;
    }

    /// <summary> 해당 위치에 스폰 가능한지 장애물 여부를 체크합니다. </summary>
    /// <returns> true = 스폰 가능한 영역입니다. false = 스폰 불가능한 영역입니다. </returns>
    private bool IsPointInExpertArea(Vector3 point, BoxCollider spawnArea)
    {
        // point 위치에서 아래로 레이를 발사합니다.
        Ray ray = new Ray(point + Vector3.up * 100, Vector3.down);

        // 레이가 어떤 콜라이더와 부딛히는지 확인합니다.
        if (Physics.Raycast(ray, out RaycastHit hit))
        {
            // 레이가 부딛힌 콜라이더가 spawnArea와 같다면 true를 반환합니다.
            if (hit.collider == spawnArea)
            {
                return true;
            }
        }

        return false; // 그 외의 경우에는 false를 반환합니다.
    }

    /// <summary> 해당 위치가 카메라에 보여지는 여부를 체크합니다. </summary>
    /// <returns> true = 위치는 카메라에 보여집니다. false = 위치는 카메라에 보여지지 않습니다. </returns>
    private bool IsPointInCameraView(Vector3 point, bool considerCamera)
    {
        if (considerCamera == false)
            return false;

        Vector3 viewportPoint = Camera.main.WorldToViewportPoint(point);
        return viewportPoint.x >= 0 && viewportPoint.x <= 1 && viewportPoint.y >= 0 && viewportPoint.y <= 1;
    }
}

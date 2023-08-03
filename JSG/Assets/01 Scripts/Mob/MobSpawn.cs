using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class MobSpawn : MonoBehaviour
{
    [SerializeField] private BoxCollider spawnFloor;
    [SerializeField] private BoxCollider spawnHole;
    [SerializeField] private Transform player;
    [SerializeField] private GameObject mobZomBear;
    [SerializeField] private GameObject mobZomBunny;
    [SerializeField] private GameObject mobHellephant;
    
    private WaitForSeconds spawnTime = new WaitForSeconds(1f);

    // 첫 번째 프레임 업데이트 전에 시작이 호출됩니다.
    void Start()
    {
        if (GameManager.Instance.isPlaying)
        {
            StartCoroutine(Spawn());
        }
    }

    IEnumerator Spawn()
    {
        while (GameManager.Instance.isPlaying)
        {
            Vector3 spawnPoint;
            int spawnType = Random.Range(0, 20);    // 랜덤 값으로 어디에 어떤 몬스터가 스폰될 지 결정됩니다.

            if (spawnType < 6) // 쥐구멍에서 몹 생성
            {
                spawnPoint = GenRandomPoint(spawnHole);

                if (spawnType < 4) Instantiate(mobZomBear, spawnPoint, Quaternion.identity);
                else  Instantiate(mobZomBear, spawnPoint, Quaternion.identity);
            }
            else // 바닥에서 몹 생성
            {
                spawnPoint = GenRandomPoint(spawnFloor, true);

                if (spawnType == 19) Instantiate(mobHellephant, spawnPoint, Quaternion.identity);
                if (spawnType > 14) Instantiate(mobZomBear, spawnPoint, Quaternion.identity);
                else Instantiate(mobZomBunny, spawnPoint, Quaternion.identity);
            }

            yield return spawnTime;
        }
    }

    /// <summary>
    /// 몹을 스폰할 지점을 생성합니다.
    /// </summary>
    Vector3 GenRandomPoint(BoxCollider spawnArea, bool considerCamera = false)
    {
        Vector3 spawnPoint;

        do
        {
            // 로컬 공간에서 랜덤한 점을 생성합니다.
            float x = Random.Range(-spawnArea.size.x / 2, spawnArea.size.x / 2);
            float z = Random.Range(-spawnArea.size.z / 2, spawnArea.size.z / 2);
            Vector3 localPoint = new Vector3(x, 0, z);

            // 로컬 공간에서의 점을 월드 공간으로 변환합니다.
            spawnPoint = spawnArea.transform.TransformPoint(localPoint);
        }
        while (!IsPointInExpertArea(spawnPoint, spawnArea) && (!considerCamera || IsPointInCameraView(spawnPoint)));

        return spawnPoint;
    }

    bool IsPointInExpertArea(Vector3 point, BoxCollider spawnArea)
    {
        // point 위치에서 아래로 레이를 발사합니다.
        Ray ray = new Ray(point + Vector3.up * 100, Vector3.down);
        RaycastHit hit;

        // 레이가 어떤 콜라이더와 부딛히는지 확인합니다.
        if (Physics.Raycast(ray, out hit))
        {
            // 레이가 부딛힌 콜라이더가 spawnArea와 같다면 true를 반환합니다.
            if (hit.collider == spawnArea)
            {
                return true;
            }
        }

        return false; // 그 외의 경우에는 false를 반환합니다.
    }

    /// <summary>
    /// 해당 지점이 카메라 내에 있으면 true를 반환합니다.
    /// </summary>
    bool IsPointInCameraView(Vector3 point)
    {
        Vector3 viewportPoint = Camera.main.WorldToViewportPoint(point);
        return viewportPoint.x >= 0 && viewportPoint.x <= 1 && viewportPoint.y >= 0 && viewportPoint.y <= 1;
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class MobSpawn : MonoBehaviour
{
    [field: SerializeField] public Collider SpawnFloor { get; set; }
    [field: SerializeField] public Collider SpawnHole { get; set; }
    [field: SerializeField] public Transform Player { get; set; }
    [field: SerializeField] public GameObject MobZomBear { get; set; }
    [field: SerializeField] public GameObject MobZomBunny { get; set; }
    [field: SerializeField] public GameObject MobHellephant { get; set; }

    // Start is called before the first frame update
    void Start()
    {
        if (GameManager.Instance.IsPlaying)
        {
            StartCoroutine(Spawn());
        }
    }

    IEnumerator Spawn()
    {
        while (GameManager.Instance.IsPlaying)
        {
            Vector3 SpawnPoint;

            // 官蹿 积己
            SpawnPoint = GenRandomPoint(SpawnFloor);

            // 零备港 积己
            //SpawnPoint = GenRandomPoint(SpawnHole);
            Instantiate(MobZomBear, SpawnPoint, Quaternion.identity);

            yield return new WaitForSeconds(0.01f);
        }
    }

    Vector3 GenRandomPoint(Collider spawnArea)
    {
        Vector3 newPoint;

        float rangeX = spawnArea.bounds.size.x / 4;
        float rangeZ = spawnArea.bounds.size.z / 4;

        // 官款靛冠胶 郴 罚待 困摹 蔼 积己
        float randomX = Random.Range(-rangeX, rangeX);
        float randomZ = Random.Range(-rangeZ, rangeZ);

        newPoint = spawnArea.bounds.center + new Vector3(randomX, 0f, randomZ);

        return newPoint;
    }
}

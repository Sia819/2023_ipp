using System.Collections;
using System.Collections.Generic;
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
    
    private WaitForSeconds spawnTime = new WaitForSeconds(1f);

    // ù ��° ������ ������Ʈ ���� ������ ȣ��˴ϴ�.
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
            int spawnType = Random.Range(0, 20);    // ���� ������ ��� � ���Ͱ� ������ �� �����˴ϴ�.

            if (spawnType < 6) // �㱸�ۿ��� �� ����
            {
                spawnPoint = GenRandomPoint(spawnHole);

                if (spawnType < 4) Instantiate(mobZomBear, spawnPoint, Quaternion.identity)?.transform.SetParent(this.transform);
                else  Instantiate(mobZomBear, spawnPoint, Quaternion.identity)?.transform.SetParent(this.transform);
            }
            else // �ٴڿ��� �� ����
            {
                spawnPoint = GenRandomPoint(spawnFloor, true);

                if (spawnType == 19) Instantiate(mobHellephant, spawnPoint, Quaternion.identity)?.transform.SetParent(this.transform);
                if (spawnType > 14) Instantiate(mobZomBear, spawnPoint, Quaternion.identity)?.transform.SetParent(this.transform);
                else Instantiate(mobZomBunny, spawnPoint, Quaternion.identity)?.transform.SetParent(this.transform);
            }

            yield return spawnTime;
        }
    }

    /// <summary>
    /// ���� ������ ������ �����մϴ�.
    /// </summary>
    Vector3 GenRandomPoint(BoxCollider spawnArea, bool considerCamera = false)
    {
        Vector3 spawnPoint;

        do
        {
            // ���� �������� ������ ���� �����մϴ�.
            float x = Random.Range(-spawnArea.size.x / 2, spawnArea.size.x / 2);
            float z = Random.Range(-spawnArea.size.z / 2, spawnArea.size.z / 2);
            Vector3 localPoint = new Vector3(x, 0, z);

            // ���� ���������� ���� ���� �������� ��ȯ�մϴ�.
            spawnPoint = spawnArea.transform.TransformPoint(localPoint);
        }
        while (IsPointInExpertArea(spawnPoint, spawnArea) && IsPointInCameraView(spawnPoint, considerCamera));

        return spawnPoint;
    }

    /// <summary> �ش� ��ġ�� ���� �������� ��ֹ� ���θ� üũ�մϴ�. </summary>
    /// <returns> true = ���� ������ �����Դϴ�. false = ���� �Ұ����� �����Դϴ�. </returns>
    bool IsPointInExpertArea(Vector3 point, BoxCollider spawnArea)
    {
        // point ��ġ���� �Ʒ��� ���̸� �߻��մϴ�.
        Ray ray = new Ray(point + Vector3.up * 100, Vector3.down);
        RaycastHit hit;

        // ���̰� � �ݶ��̴��� �ε������� Ȯ���մϴ�.
        if (Physics.Raycast(ray, out hit))
        {
            // ���̰� �ε��� �ݶ��̴��� spawnArea�� ���ٸ� true�� ��ȯ�մϴ�.
            if (hit.collider == spawnArea)
            {
                return true;
            }
        }

        return false; // �� ���� ��쿡�� false�� ��ȯ�մϴ�.
    }

    /// <summary> �ش� ��ġ�� ī�޶� �������� ���θ� üũ�մϴ�. </summary>
    /// <returns> true = ��ġ�� ī�޶� �������ϴ�. false = ��ġ�� ī�޶� �������� �ʽ��ϴ�. </returns>
    bool IsPointInCameraView(Vector3 point, bool considerCamera)
    {
        if (considerCamera == false)
            return false;

        Vector3 viewportPoint = Camera.main.WorldToViewportPoint(point);
        return viewportPoint.x >= 0 && viewportPoint.x <= 1 && viewportPoint.y >= 0 && viewportPoint.y <= 1;
    }
}

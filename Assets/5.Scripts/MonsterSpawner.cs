using System.Collections;
using UnityEngine;
using static UnityEditor.Experimental.GraphView.GraphView;

public class MonsterSpawner : MonoBehaviour
{
    [SerializeField] private GameObject monsterPrefab;
    [SerializeField] private Transform spawnPoint;

    private float createTime = 3.0f;

    private int currentMonsterCount = 0;
    private int maxMonsterCount = 100;
    
    private float[] yPos = new[] { 0f, 0.25f, 0.5f };
    void Start()
    {
        StartCoroutine("CreateMonster");
    }

    private IEnumerator CreateMonster()
    {
        while (currentMonsterCount < maxMonsterCount)
        {
            yield return new WaitForSeconds(createTime);

            var monster = Instantiate(monsterPrefab);

            currentMonsterCount++;

            var randomRange = Random.Range(0, 3);
            var newPosition = spawnPoint.position;
            newPosition.y = yPos[randomRange];
            newPosition.z = randomRange;
            monster.name = currentMonsterCount.ToString();

            SetLayerRecursively(monster, randomRange + 6);

            monster.transform.position = newPosition;
        }
    }

    void SetLayerRecursively(GameObject obj, int layer)
    {
        foreach (Transform child in obj.transform)
        {
            SetLayerRecursively(child.gameObject, layer);
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class SpawnManager : MonoBehaviour
{
    private System.Random rand = new System.Random();
    public GameConstants gameConstants;
    // public EnemyController enemyController;
    // Start is called before the first frame update
    void Start()
    {
        // spawn two gombaEnemy
        for (int j = 0; j < 2; j++)
            spawnFromPooler(ObjectType.gombaEnemy);
        // spawn one Koopa
        spawnFromPooler(ObjectType.greenEnemy);
        // subscribe and hit that notification bell
        GameManager.OnEnemyDeath += spawnMore;
    }

    // Update is called once per frame
    void Update()
    {

    }

    void spawnFromPooler(ObjectType i)
    {
        // static method access
        GameObject item = ObjectPooler.SharedInstance.GetPooledObject(i);
        if (item != null)
        {
            //set position, and other necessary states
            item.transform.position = gameConstants.gombaSpawnPointStart + new Vector3(rand.Next(-gameConstants.gombaOffset, gameConstants.gombaOffset), 0, 0);
            item.SetActive(true);

        }
        else
        {
            Debug.Log("not enough items in the pool.");
        }
    }

    void spawnMore()
    // spawns a random enemy when the previous one gets beat
    {
        if (rand.NextDouble() > 0.5)
        {
            spawnFromPooler(ObjectType.gombaEnemy);
        }
        else
        {
            spawnFromPooler(ObjectType.greenEnemy);
        }
    }
    void Awake()
    {

    }

    void OnDestroy()
    {
        GameManager.OnEnemyDeath -= spawnMore;
    }

}

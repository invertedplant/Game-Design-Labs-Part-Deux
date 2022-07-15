using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SpawnEnemyEV : MonoBehaviour
{
    // Start is called before the first frame update
    private System.Random rand = new System.Random();
    public GameConstants gameConstants;
    void Start()
    {
        Debug.Log("Spawnmanager start");
        // spawn two gombaEnemy
        for (int j = 0; j < 2; j++)
            spawnFromPooler(ObjectType.gombaEnemy);
        // spawn one Koopa
        spawnFromPooler(ObjectType.greenEnemy);        
    }
    void startSpawn(Scene scene, LoadSceneMode mode)
    {
        for (int j = 0; j < 2; j++)
            spawnFromPooler(ObjectType.gombaEnemy);
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
            item.transform.position = gameConstants.gombaSpawnPointStart + new Vector3(rand.Next(-gameConstants.gombaOffset, gameConstants.gombaOffset), gameConstants.groundSurface, 0);
            item.SetActive(true);

        }
        else
        {
            Debug.Log("not enough items in the pool.");
        }
    }

    public void spawnMore()
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
}

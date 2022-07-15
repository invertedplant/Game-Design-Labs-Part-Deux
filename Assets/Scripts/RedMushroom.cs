using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RedMushroom : MonoBehaviour, ConsumableInterface
{
    public Texture t;
    public GameConstants gameConstants;
    public void consumedBy(GameObject player)
    {
        // give player jump boost
        player.GetComponent<PlayerController>().gameConstants.upSpeed += gameConstants.redMushroomBoost;
        StartCoroutine(removeEffect(player));
    }

    IEnumerator removeEffect(GameObject player)
    {
        yield return new WaitForSeconds(5.0f);
        player.GetComponent<PlayerController>().gameConstants.upSpeed -= gameConstants.redMushroomBoost;
    }

    void OnCollisionEnter2D(Collision2D col)
    {
        if (col.gameObject.CompareTag("Player"))
        {
            // update UI
            CentralManager.centralManagerInstance.addPowerup(t, 0, this);
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OrangeMushroom : MonoBehaviour, ConsumableInterface
{
    public Texture t;
    public GameConstants gameConstants;
    public void consumedBy(GameObject player)
    {
        // give player speed boost
        player.GetComponent<PlayerController>().gameConstants.maxSpeed *= gameConstants.orangeMushroomMultiplier;
		player.GetComponent<PlayerController>().gameConstants.speed *= gameConstants.orangeMushroomMultiplier;
        player.GetComponent<PlayerController>().gameConstants.maxAirSpeed *= gameConstants.orangeMushroomMultiplier;
        StartCoroutine(removeEffect(player));
    }

    IEnumerator removeEffect(GameObject player)
    {
        yield return new WaitForSeconds(5.0f);
        player.GetComponent<PlayerController>().gameConstants.maxSpeed /= gameConstants.orangeMushroomMultiplier;
		player.GetComponent<PlayerController>().gameConstants.speed /= gameConstants.orangeMushroomMultiplier;
        player.GetComponent<PlayerController>().gameConstants.maxAirSpeed /= gameConstants.orangeMushroomMultiplier;
    }

    void OnCollisionEnter2D(Collision2D col)
    {
        if (col.gameObject.CompareTag("Player"))
        {
            // update UI
            CentralManager.centralManagerInstance.addPowerup(t, 1, this);
        }
    }
}

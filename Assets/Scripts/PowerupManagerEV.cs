using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum PowerupIndex
{
    ORANGEMUSHROOM = 0,
    REDMUSHROOM = 1
}
public class PowerupManagerEV : MonoBehaviour
{
    // reference of all player stats affected
    public IntVariable marioJumpSpeed;
    public IntVariable marioMaxSpeed;
    public PowerupInventory powerupInventory;
    public List<GameObject> powerupIcons;

    void Start()
    {
        if (!powerupInventory.gameStarted)
        {
            powerupInventory.gameStarted = true;
            powerupInventory.Setup(powerupIcons.Count);
            resetPowerup();
        }
        else
        {
            // re-render the contents of the powerup from the previous time
            for (int i = 0; i < powerupInventory.Items.Count; i++)
            {
                Powerup p = powerupInventory.Get(i);
                if (p != null)
                {
                    AddPowerupUI(i, p.powerupTexture);
                }
            }
        }
    }

    public void resetPowerup()
    {
        for (int i = 0; i < powerupIcons.Count; i++)
        {
            powerupIcons[i].SetActive(false);
        }
    }

    public void ResetValues()
    {
        powerupInventory.Clear();
        resetPowerup();
    }

    void AddPowerupUI(int index, Texture t)
    {
        powerupIcons[index].GetComponent<RawImage>().texture = t;
        powerupIcons[index].SetActive(true);
    }

    public void AddPowerup(Powerup p)
    {
        powerupInventory.Add(p, (int)p.index);
        AddPowerupUI((int)p.index, p.powerupTexture);
    }

    public void ConsumePowerup(int i)
    {
        Powerup p = powerupInventory.Get(i);
        if (p != null)
        {
            marioMaxSpeed.ApplyChange(p.aboluteSpeedBooster);
            marioJumpSpeed.ApplyChange(p.absoluteJumpBooster);
            StartCoroutine(removeEffect(i, p));
        }
    }

    void RemovePowerupIcon(int i)
    {
        powerupIcons[i].SetActive(false);
    }

    public void CheckPowerup(int i)
    {
        if (powerupIcons[i].activeSelf == true)
        {
            RemovePowerupIcon(i);
            ConsumePowerup(i);
        }
    }

    public void TryConsumePowerup(KeyCode Key)
    {
        Debug.Log("Key pressed: " + Key.ToString());
        switch(Key)
        {
            case KeyCode.Z:
                CheckPowerup(0);
                break;
            case KeyCode.X:
                CheckPowerup(1);
                break;
            default:
                break;
        }
    }

    public void OnApplicationQuit()
    {
        ResetValues();
    }

    IEnumerator removeEffect(int idx, Powerup p){
        yield return new WaitForSeconds(p.duration);
        marioMaxSpeed.ApplyChange(-p.aboluteSpeedBooster);
        marioJumpSpeed.ApplyChange(-p.absoluteJumpBooster);
        powerupInventory.Remove(idx);
    }
}

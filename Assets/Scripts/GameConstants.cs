using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "GameConstants", menuName = "ScriptableObjects/GameConstants", order = 1)]
public class GameConstants : ScriptableObject
{
    // reset values
    public Vector3 gombaSpawnPointStart = new Vector3(18f, 0f, 0); // hardcoded location
    public int gombaOffset = 2;                                                              

    // for Break.cs
    public int breakTimeStep = 30;
    public int breakDebrisTorque = 10;
    public int breakDebrisForce = 10;

    // for SpawnDebris.cs
    public int spawnNumberOfDebris = 10;

    // for Rotator.cs
    public int rotatorRotateSpeed = 6;

    // for testing
    public int testValue;
    public bool godMode = false;

    // for EnemyController.cs
    public float enemySpeed = 1.5f;
    public float enemySpeedOffset = 0.75f;
    public float killHeight = 0.75f;

    // for flattening enemies
    public int enemyFlattenSteps = 5;
    public float enemyFlattenLimit = 1.0f;

    // for PlayerController.cs
    public int speed = 70; 
    public int upSpeed = 40;
    public int maxSpeed = 12;
    public int maxAirSpeed = 8;
    public int AirSpeedOffset = -4;
    public float skidLimit = 0.3f;
    public Vector2 deathForce = (Vector2.up * 20);

    // for MushroomController.cs
    public float mushroomSpeed = 3.5f;
    public Vector2 mushroomLaunchForce = Vector2.up*16;
    public int mushroomAbsorbSteps = 8;
    public float mushroomExpandSize = 0.4f; 

    // for Environment positions
    public int groundSurface = 0;

    // game settings
    public int targetFrameRate = 30;

    // for Powerups
    public int orangeMushroomMultiplier = 2;
    public int redMushroomBoost = 15;

    // for Springs
    public int springMultiplier = 20;

    // for Scriptable Objects
    // Mario basic starting values
    public int playerMaxSpeed = 12;
    public int playerMaxJumpSpeed = 40;
    public int playerDefaultForce = 80;
}

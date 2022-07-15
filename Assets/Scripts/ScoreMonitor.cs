using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class ScoreMonitor : MonoBehaviour
{
    public IntVariable marioScore;
    public TMP_Text score;
    public void UpdateScore()
    {
        score.text = "Score: " + marioScore.Value.ToString();
    }
    // Start is called before the first frame update
    void Start()
    {
        score = GetComponent<TMP_Text>();
        UpdateScore();
    }

    // Update is called once per frame
    void Update()
    {

    }

    // kinda funky
    void OnApplicationQuit()
    {
        marioScore.SetValue(0);
    }
}

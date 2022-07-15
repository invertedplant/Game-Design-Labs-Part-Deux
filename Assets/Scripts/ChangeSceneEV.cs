using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class ChangeSceneEV : MonoBehaviour
{
    public AudioSource changeSceneSound;
    private BoxCollider2D doorBox;
    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Player")
        {
            changeSceneSound.PlayOneShot(changeSceneSound.clip);
            doorBox.enabled = false;
            StartCoroutine(WaitSoundClip("Level2"));
        }
    }

    IEnumerator WaitSoundClip(string sceneName)
    {
        yield return new WaitUntil(() => !changeSceneSound.isPlaying);
        StartCoroutine(ChangeScene("Level2"));

    }
    IEnumerator ChangeScene(string sceneName)
    {
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(sceneName, LoadSceneMode.Single);
        // Wait until the asynchronous scene fully loads
        while (!asyncLoad.isDone)
        {
            yield return null;
        }
    }
    void Start() {
        changeSceneSound = GetComponent<AudioSource>();
        doorBox = GetComponent<BoxCollider2D>();
    }
}

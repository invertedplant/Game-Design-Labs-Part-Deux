using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BreakBrick : MonoBehaviour
{
    public bool broken = false;
    public GameObject debrisObject;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void  OnTriggerEnter2D(Collider2D col){
	if (col.gameObject.CompareTag("Player") &&  !broken){
		broken  =  true;
		debrisObject.SetActive(true);
		gameObject.transform.parent.GetComponent<SpriteRenderer>().enabled  =  false;
		gameObject.transform.parent.GetComponent<BoxCollider2D>().enabled  =  false;
        GetComponent<AudioSource>().Play();
		GetComponent<EdgeCollider2D>().enabled  =  false;
        CentralManager.centralManagerInstance.increaseScore();
	}
}
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatformDes : MonoBehaviour
{

    public GameObject platformDestructionPoint;

    // Start is called before the first frame update
    void Start()
    {
        platformDestructionPoint = GameObject.Find("PlatformDesPoint");
    }

    // Update is called once per frame
    void Update()
    {
        if(transform.position.x < platformDestructionPoint.transform.position.x)
        {
            gameObject.SetActive(false);
            //Destroy(gameObject);
        }
    }
}

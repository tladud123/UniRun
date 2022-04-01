using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScrollingObject : MonoBehaviour

{
    public float speed = 0.5f;
    // Start is called before the first frame update


    // Update is called once per frame
    void Update()
        
    {
        if(!Gamemanager.instance.isGameover)
        {
            transform.Translate(Vector3.left * speed * Time.deltaTime);
        }
       
    }
}

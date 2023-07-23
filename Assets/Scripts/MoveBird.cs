using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveBird : MonoBehaviour
{
    float x = 0.00f;
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
     // Rotation

     Vector3 mousePos = Input.mousePosition;
        mousePos.z = 5.23f;

        Vector3 objectPos = Camera.main.WorldToScreenPoint(transform.position);
        mousePos.x = mousePos.x - objectPos.x;
        mousePos.y = mousePos.y - objectPos.y;

        float angle = Mathf.Atan2(mousePos.y, mousePos.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(new Vector3(0, 0, angle));


        // Movement
        

        if (Input.GetMouseButtonDown(0) == false)
        {
         transform.position = new Vector3(x, 0, 0);

        }

        if (Input.GetMouseButton(0) == true)
        {
            x  = x +   0.2f * Time.deltaTime;
                        

        }
    }
}

using System.Runtime.InteropServices;
using System.ComponentModel;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Slingshot : MonoBehaviour
{
    Vector3 initialPoint;
    Rigidbody2D rb;
    LineRenderer lineRenderer;

    // Start is called before the first frame update
    void Start()
    {
        rb = gameObject.GetComponent<Rigidbody2D>();
         lineRenderer = rb.GetComponent<LineRenderer>();
    }
    


    // Update is called once per frame
    void Update()
    {

        //    if (Input.GetMouseButtonDown(0))
        //    {
        //        initialPoint = Input.mousePosition;
        //    }
        //    if (Input.GetMouseButtonUp(0))
        //    {
        //        Vector3 startPoint = Input.mousePosition;
        //        Vector3 newVector = startPoint - initialPoint;
        //        rb.AddForce(newVector, ForceMode2D.Impulse);
        //    }

        if (Input.GetMouseButton(0))
        {
            initialPoint = rb.position;
            initialPoint.y += 2;
            lineRenderer.SetPosition(0, initialPoint);
            Camera c = Camera.main;
            //Vector3 startPoint = c.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, c.nearClipPlane));
            
            Vector3 endPoint = c.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, c.nearClipPlane));
            endPoint.z = 0;


            Vector3 newVector = initialPoint - endPoint;

            var distance = Mathf.Min(newVector.magnitude, 4);
            var direction = newVector.normalized;

            var endPos = initialPoint + distance * direction * -1;

            endPos.x = Mathf.Clamp(endPos.x, -100, rb.position.x);
            lineRenderer.SetPosition(1, endPos);
        }
    }

    //Vector3 newVector = startPoint - initialPoint;
}

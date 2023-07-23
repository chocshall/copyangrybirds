using System.Runtime.InteropServices;
using System.ComponentModel;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Slingshot : MonoBehaviour
{
    Vector3 initialPoint;
    [SerializeField]Rigidbody2D birdRb;
    LineRenderer lineRenderer;

    public float strengthMultiplier = 2.0f;


    Vector3 endPos;

    // Start is called before the first frame update
    void Start()
    {
        
         lineRenderer = gameObject.GetComponent<LineRenderer>();
    }
    


    // Update is called once per frame
    void Update()
    {

        if (Input.GetMouseButtonDown(0))
        {
            initialPoint = Input.mousePosition;
            birdRb.isKinematic = true;
            birdRb.velocity = new Vector2();
        }
        if (Input.GetMouseButtonUp(0))
        {
            Vector3 startPoint = Input.mousePosition;
            Vector3 newVector = initialPoint - startPoint;
            birdRb.isKinematic = false;
            newVector *= strengthMultiplier;
            //print("length: " + newVector.magnitude);
            //print("dir: " + newVector);
            birdRb.AddForce(newVector, ForceMode2D.Impulse);
        }

        if (Input.GetMouseButton(0))
        {
            var initialPoint = transform.position;
            initialPoint.y += 2;
            lineRenderer.SetPosition(0, initialPoint);
            Camera c = Camera.main;
            //Vector3 startPoint = c.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, c.nearClipPlane));

            Vector3 endPoint = c.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, c.nearClipPlane));
            endPoint.z = 0;


            Vector3 newVector = initialPoint - endPoint;

            int maxRadius = 4;
            var distance = Mathf.Min(newVector.magnitude, maxRadius); 
            Vector3 direction = newVector.normalized;

            endPos = initialPoint + distance * direction * -1;

            endPos.x = Mathf.Clamp(endPos.x, -100, transform.position.x);
            

            lineRenderer.SetPosition(1, endPos);

            birdRb.position = endPos;
        }
    }

    //Vector3 newVector = startPoint - initialPoint;
}

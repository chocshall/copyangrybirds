using System.Runtime.InteropServices;
using System.ComponentModel;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class Slingshot : MonoBehaviour
{
    Vector3 fireVector;
    [SerializeField]Rigidbody2D birdRb;
    [SerializeField] LineRenderer trajectoryLine;


    LineRenderer lineRenderer;

    public float strengthMultiplier = 5.0f;


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
            birdRb.isKinematic = true;
            birdRb.velocity = new Vector2(0, 0);
        }
        if (Input.GetMouseButtonUp(0))
        {
            
            birdRb.isKinematic = false;
            //newVector *= strengthMultiplier;
            fireVector *= strengthMultiplier;
            print("length: " + fireVector.magnitude);
            print("Vector: " + fireVector);
            //birdRb.AddForce(fireVector, ForceMode2D.Impulse);
            birdRb.velocity = fireVector;
        }

        if (Input.GetMouseButton(0))
        {
            birdRb.velocity = new Vector2(0, 0);
            var initialPoint = transform.position;
            initialPoint.y += 2;
            lineRenderer.SetPosition(0, initialPoint);
            //Vector3 startPoint = c.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, c.nearClipPlane));

            Vector3 endPoint = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, 0));
            endPoint.z = 0;

            Vector3 newVector = endPoint - initialPoint;

            int maxRadius = 4;
            float distance = Mathf.Min(newVector.magnitude, maxRadius); 
            Vector3 direction = newVector.normalized;

            endPos = initialPoint + distance * direction;
            endPos.x = Mathf.Clamp(endPos.x, -100, transform.position.x);
            

            lineRenderer.SetPosition(1, endPos);

            birdRb.position = endPos;

            fireVector = initialPoint - endPos;
            PlotLine(birdRb, trajectoryLine, fireVector * strengthMultiplier);
            //ForcePlot(birdRb, fireVector * strengthMultiplier, 500, trajectoryLine);
        }
    }
    
    public void PlotLine(Rigidbody2D rb, LineRenderer lr, Vector2 velocity)
    {
        Vector2[] trajectory = Plot(rb, (Vector2)rb.position, velocity, 500);
        lr.positionCount = trajectory.Length;

        Vector3[] positions = new Vector3[trajectory.Length];
        for (int i = 0; i < positions.Length; i++)
        {
            positions[i] = trajectory[i];
        }
        lr.SetPositions(positions);
    }

    public Vector2[] Plot(Rigidbody2D rigidbody, Vector2 pos, Vector2 velocity, int steps)
    {
        Vector2[] results = new Vector2[steps];

        float timestep = Time.fixedDeltaTime / Physics2D.velocityIterations;
        Vector2 gravityAccel = Physics2D.gravity * rigidbody.gravityScale * timestep * timestep;

        float drag = 1f - timestep * rigidbody.drag;
        Vector2 moveStep = velocity * timestep;

        for (int i = 0; i < steps; i++)
        {
            moveStep += gravityAccel;
            moveStep *= drag;
            pos += moveStep;
            results[i] = pos;

        }

        return results;
    }

       
    //void ForcePlot(Rigidbody2D rb, Vector2 velocity, int steps, LineRenderer lr)
    //{
    //    float dotSpacing = 2f;
    //    float timeStamp = 0;

    //    Vector3[] positions = new Vector3[steps];
    //    for (int i = 0; i < positions.Length; i++)
    //    {
    //        positions[i].x = rb.position.x + velocity.x * timeStamp;
    //        positions[i].y = (rb.position.y + velocity.y * timeStamp) - (Physics2D.gravity.magnitude * timeStamp * timeStamp) / 2f;
    //        timeStamp += dotSpacing;
    //    }
    //    lr.SetPositions(positions);
    //}
}

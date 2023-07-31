using UnityEngine;


public enum SlingshotState
{ 
    Idle,
    Pulling,
    Fired
}



public class Slingshot : MonoBehaviour
{
    [Header("Bird Settings")]
    [HideInInspector]
    public GameObject BirdToThrow;
    private LineRenderer trajectoryLine;

    //[SerializeField] private Rigidbody2D birdRb;
    //private MoveBird birdScript;
    [SerializeField] private float strengthMultiplier = 5.0f;

    [Header("Slingshot Settings")]
    [SerializeField] private float maxStretchRadius = 2;
    [SerializeField] private int numPoints = 50;
    [SerializeField] private float timeStep = 0.1f;

    [HideInInspector]
    public SlingshotState slingshotState = SlingshotState.Idle;

    private LineRenderer lineRenderer;
    
    private Vector3 fireVector;
    private Vector3 endPos;
    

    private void Start()
    {
        InitializeComponents();
    }

    private void InitializeComponents()
    {
        
        
        lineRenderer = GetComponent<LineRenderer>();
        //birdScript = birdRb.GetComponent<MoveBird>();


    }

    //PULLING STATE
    private void OnMouseDown()
    {
        slingshotState = SlingshotState.Pulling;

        trajectoryLine = BirdToThrow.GetComponent<LineRenderer>();
        trajectoryLine.positionCount = numPoints;

    }

    //FIRED STATE
    private void OnMouseUp()
    {
        slingshotState = SlingshotState.Fired;


        BirdToThrow.GetComponent<MoveBird>().OnThrow();
        fireVector *= strengthMultiplier;
        BirdToThrow.GetComponent<Rigidbody2D>().velocity = fireVector;
    }

    private void OnMouseDrag()
    {
        //birdRb.velocity = Vector2.zero;
        var initialPoint = transform.position + Vector3.up * 0.7f;
        lineRenderer.SetPosition(0, initialPoint);

        Vector3 endPoint = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, 0));
        endPoint.z = 0;

        Vector3 newVector = endPoint - initialPoint;
        float distance = Mathf.Min(newVector.magnitude, maxStretchRadius);
        Vector3 direction = newVector.normalized;

        endPos = initialPoint + distance * direction;
        endPos.x = Mathf.Clamp(endPos.x, -100, transform.position.x);

        lineRenderer.SetPosition(1, endPos);

        BirdToThrow.transform.position = endPos;

        fireVector = initialPoint - endPos;
        PlotLine(BirdToThrow.GetComponent<Rigidbody2D>(), trajectoryLine, fireVector * strengthMultiplier);
    }

    private void PlotLine(Rigidbody2D rb, LineRenderer lr, Vector2 velocity)
    {
        Vector3[] trajectory = Plot(rb, rb.position, velocity, numPoints);
        Vector3[] positions = new Vector3[trajectory.Length];

        for (int i = 0; i < trajectory.Length; i++)
        {
            positions[i] = trajectory[i];
        }

        lr.SetPositions(positions);
    }

    private Vector3[] Plot(Rigidbody2D rigidbody, Vector2 pos, Vector2 velocity, int steps)
    {
        Vector3[] results = new Vector3[steps];

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
}

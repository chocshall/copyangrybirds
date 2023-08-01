using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public enum BirdState
{
    Idle,
    Flying,
    Stopped
}

public class MoveBird : MonoBehaviour
{
    private TrailRenderer trailrenderer;
    Vector3 startingPos;
    Rigidbody2D rb;
    PolygonCollider2D box2D;

    [SerializeField] float timeAlive = 5;
    [SerializeField] float minVelocity = 1;
    [SerializeField] Transform slingshotPos;

    [SerializeField] public BirdState _birdState{ get; private set; }


    private void Awake()
    {
        //trailrenderer = gameObject.GetComponent<TrailRenderer>();
       
        rb = GetComponent<Rigidbody2D>();
        box2D = GetComponent<PolygonCollider2D>();
        trailrenderer = GetComponent<TrailRenderer>();
        startingPos = transform.position;

        rb.isKinematic = true;
        _birdState = BirdState.Idle;
        box2D.enabled = false;

        //StartCoroutine(MoveToSlingshot());
    }

    [SerializeField] GameObject bigCircle;
    [SerializeField] GameObject smollCircle;
    private Coroutine _moveCoroutine;
    private Coroutine _moveCoroutine2;

    float spawnIntervalA = 0.1f;
    float spawnIntervalB = 0.1f;

    private float timeSinceLastSpawn;
    private bool isPrefabA = true;
    List<GameObject> trailArray = new List<GameObject>();
    [SerializeField] Transform trailStorage;
    [SerializeField] Transform particleStorage;

    [SerializeField] ParticleSystem hitParticle;
    [SerializeField] ParticleSystem cloudParticle;
    bool isTrail = true;
    private void Update()
    {

        //transform.position += new Vector3(transform.position.x + 2, 1.26f);

        if (_birdState == BirdState.Flying && isTrail)
        {
            
            if(Input.GetMouseButtonDown(0))
            {
                Instantiate(cloudParticle, transform.position, Quaternion.identity, particleStorage);
            }

            timeSinceLastSpawn += Time.deltaTime;

            if (isPrefabA && timeSinceLastSpawn >= spawnIntervalA)
            {
                SpawnTrailElement(bigCircle);
                isPrefabA = false;
                timeSinceLastSpawn = 0f;
            }
            else if (!isPrefabA && timeSinceLastSpawn >= spawnIntervalB)
            {
                SpawnTrailElement(smollCircle);
                isPrefabA = true;
                timeSinceLastSpawn = 0f;
            }
        }
    }
    IEnumerator SummonTrail(float count, GameObject circle, string size)
    {
        

        yield return new WaitForSeconds(count);

        Vector2 particlePos = rb.position;
        Instantiate(circle, particlePos, Quaternion.identity);
        //do something 
        // print("works?");



        if(size == "big")
         _moveCoroutine = null;

        if (size == "smoll")
            _moveCoroutine2 = null;
    }

    private void SpawnTrailElement(GameObject prefab)
    {
        trailArray.Add(Instantiate(prefab, transform.position, Quaternion.identity, trailStorage));
        // Optionally parent the new trail element to a container or parent object for organization.
    }


    [SerializeField]float lerpDuration = 1;
    public IEnumerator MoveToSlingshot()
    {
        float timeElapsed = 0;

        while (timeElapsed < lerpDuration)
        {
            // The center of the arc
            Vector3 center = (startingPos + slingshotPos.position) * 0.5F;

            // move the center a bit downwards to make the arc vertical
             center -= new Vector3(0, 1, 0);

            // Interpolate over the arc relative to center
            Vector3 riseRelCenter = startingPos - center;
            Vector3 setRelCenter = slingshotPos.position - center;


            transform.position = Vector3.Slerp(riseRelCenter, setRelCenter, timeElapsed / lerpDuration);
            transform.position += center;

            timeElapsed += Time.deltaTime;
            yield return null;
        }
        transform.position = slingshotPos.position;



    }

    public void OnThrow()
    {
        rb.isKinematic = false;
        _birdState = BirdState.Flying;
        box2D.enabled = true;
       // trailrenderer.enabled = true;
    }
    

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(_birdState == BirdState.Flying)
        {
            Instantiate(hitParticle, transform.position, Quaternion.identity, particleStorage);
        }
        isTrail = false;
        //trailrenderer.emitting = false;
        //MOVE TO GAME MANAGER
        //_cinemachineScript.StopFollowing();
        //  StartCoroutine(ResetCooldown(2));
        //  Start destruction Cycle
        if (rb.velocity.magnitude <= minVelocity && _birdState == BirdState.Flying)
        {
            _birdState = BirdState.Stopped;

            StartCoroutine(DestructionCooldown(timeAlive));
        }
        //
    }


    IEnumerator DestructionCooldown(float timer)
    {
        yield return new WaitForSeconds(timer);
        gameObject.SetActive(false);
        foreach(var trailElement in trailArray)
        {
            trailElement.SetActive(false);
        }
    }
}





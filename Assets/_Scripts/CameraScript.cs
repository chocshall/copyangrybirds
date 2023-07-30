using System.Collections;
using UnityEngine;

public class CameraScript : MonoBehaviour
{
    [SerializeField] private Transform target;
    [SerializeField] private float height = 2;
    [SerializeField] private float maxExpansionSize = 14;
    private bool isFollowing;
    private Vector3 startingPos;
    private float startingSize;

    private Camera mainCam;
    private Rigidbody2D targetRb;

    // Start is called before the first frame update
    private void Start()
    {
        targetRb = target.GetComponent<Rigidbody2D>();
        startingPos = transform.position;
        mainCam = Camera.main;
        startingSize = mainCam.orthographicSize;
    }

    private void LateUpdate()
    {
        if (isFollowing && target.position.x > startingPos.x)
        {
            FollowTarget();
        }
    }

    private void FollowTarget()
    {
        Vector3 newPosition = new Vector3(target.position.x, transform.position.y, transform.position.z);
        transform.position = newPosition;

        //mainCam.orthographicSize += 10 * Time.deltaTime; // reikia limito but too funny

        float size = mainCam.orthographicSize + 10 * Time.deltaTime;
        mainCam.orthographicSize = Mathf.Min(maxExpansionSize, size);

        if (targetRb.velocity.x < 1)
        {
            isFollowing = false;
            StartCoroutine(ResetCameraAfterDelay(1));
        }
    }

    public void SetFollowBool(bool value)
    {
        isFollowing = value;
    }

    IEnumerator ResetCameraAfterDelay(float count)
    {
        yield return new WaitForSeconds(count);
        transform.position = startingPos;
        mainCam.orthographicSize = startingSize;
    }
}

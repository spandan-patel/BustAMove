using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class ShooterController : MonoBehaviour
{
    public enum State
    {
        Shoot,
        Wait
    }

    public State currentState;

    [SerializeField]
    private LineRenderer lineRendererObject;

    [SerializeField]
    private Transform spawnPoint;

    [SerializeField]
    private GameObject canonRendererObject;

    [SerializeField]
    private LayerMask TrajectoryWallLayerMask;

    [SerializeField]
    private int numberOfReflaction = 5;

    [SerializeField]
    private GameObject bubblePrefab;

    private float radiusOfBubble;

    private Ray2D ray;

    // Start is called before the first frame update
    void Start()
    {
        lineRendererObject.enabled = false;

        Physics2D.queriesStartInColliders = false;

        radiusOfBubble = bubblePrefab.GetComponent<CircleCollider2D>().radius;

        currentState = State.Shoot;
    }

    // Update is called once per frame
    void Update()
    {
        if (currentState == State.Shoot)
        {
            //rotate canon to mouse position
            RotateCanon();

            //show trajectory
            ShowTrajectoryLine();
        }

        else
        {
            lineRendererObject.enabled = false;
        }
    }

    void RotateCanon()
    {
        Vector2 mousePosition = Camera.main.ScreenToWorldPoint(Mouse.current.position.ReadValue());

        Vector2 direction = new Vector2(mousePosition.x - canonRendererObject.transform.position.x, 
            mousePosition.y - canonRendererObject.transform.position.y);

        canonRendererObject.transform.up = direction;
    }

    public void ShootBubble(InputAction.CallbackContext callbackContext)
    {
        if (callbackContext.performed && currentState == State.Shoot)
        {
            GameObject bubble = Instantiate(bubblePrefab, spawnPoint.position, canonRendererObject.transform.rotation);

            bubble.GetComponent<BubbleProjectile>().SetShootController(this);

            currentState = State.Wait;
        }
    }

    public void ChangeState(State newState)
    {
        currentState = newState;
    }

    void ShowTrajectoryLine()
    {
        ray = new Ray2D(new Vector2(spawnPoint.position.x, spawnPoint.position.y), new Vector2(spawnPoint.up.x, spawnPoint.up.y));

        lineRendererObject.enabled = true;
        float width = lineRendererObject.startWidth;
        lineRendererObject.material.mainTextureScale = new Vector2(1f / width, 1.0f);

        lineRendererObject.positionCount = 1;
        lineRendererObject.SetPosition(0, new Vector3(spawnPoint.position.x, spawnPoint.position.y, -2.0f));

        for(int i = 0; i < numberOfReflaction; i++)
        {
            Vector3 hitLocation = Vector3.zero;

            RaycastHit2D raycastHit = Physics2D.CircleCast(ray.origin, radiusOfBubble, ray.direction, Mathf.Infinity, TrajectoryWallLayerMask);

            if (raycastHit.collider != null)
            {
                if(raycastHit.collider.gameObject.CompareTag("Wall"))
                {
                    lineRendererObject.positionCount += 1;
                    hitLocation = new Vector3(raycastHit.point.x, raycastHit.point.y, -2.0f);
                    lineRendererObject.SetPosition(lineRendererObject.positionCount - 1, hitLocation);

                    ray = new Ray2D(raycastHit.point, Vector2.Reflect(ray.direction, raycastHit.normal));
                }

                if (raycastHit.collider.gameObject.CompareTag("Bubble") ||
                    raycastHit.collider.gameObject.CompareTag("TopWall"))
                {
                    lineRendererObject.positionCount += 1;
                    hitLocation = new Vector3(raycastHit.point.x, raycastHit.point.y, -2.0f);
                    lineRendererObject.SetPosition(lineRendererObject.positionCount - 1, hitLocation);
                }
            }

            else
            {
                lineRendererObject.positionCount += 1;
                lineRendererObject.SetPosition(lineRendererObject.positionCount - 1, ray.origin + (ray.direction * 100.0f));
            }
        }
    }
}

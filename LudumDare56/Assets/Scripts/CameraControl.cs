using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class CameraControl : MonoBehaviour
{

    [SerializeField] float minZoom = 1f;
    [SerializeField] float maxZoom = 1000f;
    [SerializeField] float speed = 2.5f;
    SpriteRenderer background;
    Camera camera;

    float leftBound;
    float rightBound;
    float topBound;
    float bottomBound;


    // Start is called before the first frame update
    void Start()
    {
        
    }

    private void Awake()
    {
        camera = GetComponent<Camera>();
        background = GameObject.FindWithTag("background").GetComponent<SpriteRenderer>();
        AssignBounds();
    }

    // Update is called once per frame
    void Update()
    {
        if (background != null)
        {
            if (Input.mouseScrollDelta != Vector2.zero)
            {
                if (camera.orthographicSize > minZoom && camera.orthographicSize < maxZoom)
                    camera.orthographicSize += speed * camera.orthographicSize * Time.deltaTime * Input.mouseScrollDelta.y;
                else if (camera.orthographicSize >= maxZoom && Input.mouseScrollDelta.y < 0)
                    camera.orthographicSize -= speed * camera.orthographicSize * Time.deltaTime;
                else if (camera.orthographicSize <= minZoom && Input.mouseScrollDelta.y > 0)
                    camera.orthographicSize += speed * camera.orthographicSize * Time.deltaTime;

            }

            AssignBounds();
            float newX = camera.transform.position.x;
            float newY = camera.transform.position.y;

            if (Input.GetKey(KeyCode.W))
            {
                newY = camera.transform.position.y + camera.orthographicSize * speed * Time.deltaTime;
                
            }
            if (Input.GetKey(KeyCode.A))
            {
                newX = camera.transform.position.x - camera.orthographicSize * speed * Time.deltaTime;
            }
            if (Input.GetKey(KeyCode.S))
            {
                newY = camera.transform.position.y - camera.orthographicSize * speed * Time.deltaTime;

            }
            if (Input.GetKey(KeyCode.D))
            {
                newX = camera.transform.position.x + camera.orthographicSize * speed * Time.deltaTime;
            }

            //assign new transform
            float camX = Mathf.Clamp(newX, leftBound, rightBound);
            float camY = Mathf.Clamp(newY, bottomBound, topBound);

            camera.transform.position = new Vector3(camX, camY, -10);

        }


    }

    private void AssignBounds()
    {

        float camVertExtent = camera.orthographicSize;
        float camHorzExtent = camera.aspect * camVertExtent;

        leftBound = background.bounds.min.x + camHorzExtent;
        rightBound = background.bounds.max.x - camHorzExtent;
        bottomBound = background.bounds.min.y + camVertExtent;
        topBound = background.bounds.max.y - camVertExtent;

    }
}

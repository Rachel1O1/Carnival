using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraBounds : MonoBehaviour
{
    [SerializeField]
    private GameObject followMe;

    [SerializeField]
    private Vector2 inBounds;

    [SerializeField]
    private float skyAllow;

    private float minX;
    private float maxX;
    private float minY;
    private float maxY;

    private float objW;
    private float objH;

    // Start is called before the first frame update
    void Start()
    {
        float vertSee = (GetComponent<Camera>().orthographicSize);    
        float horzSee = vertSee * Screen.width / Screen.height;
        minX = (float)(horzSee - inBounds.x / 2.0);
        maxX = (float)(inBounds.x / 2.0 - horzSee);
        minY = (float)(vertSee - inBounds.y / 2.0);
        maxY = (float)((inBounds.y + skyAllow) / 2.0 - vertSee);

        objW = 0;
        objH = 0;
        objW = followMe.transform.GetComponent<SpriteRenderer>().bounds.size.x / 2;
        objH = followMe.transform.GetComponent<SpriteRenderer>().bounds.size.y / 2;
    }

    // Update is called once per frame
    void LateUpdate()
    {
        Vector3 viewPos = followMe.transform.position;
        Vector2 peepBounds = -(inBounds / 2);
        viewPos.x = Mathf.Clamp(viewPos.x, peepBounds.x + objW, peepBounds.x * -1 - objW);
        viewPos.y = Mathf.Clamp(viewPos.y, peepBounds.y + objH, peepBounds.y * -1 - objH);
        followMe.transform.position = viewPos;

        viewPos = followMe.transform.position;
        viewPos.x = Mathf.Clamp(viewPos.x, minX, maxX);
        viewPos.y = Mathf.Clamp(viewPos.y, minY, maxY);
        viewPos.z = transform.position.z;
        transform.position = viewPos;
    }
}

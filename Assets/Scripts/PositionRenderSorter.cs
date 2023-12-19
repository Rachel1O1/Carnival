using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PositionRenderSorter : MonoBehaviour
{
    [SerializeField]
    private int sortingOrderBase = 50000;
    [SerializeField]
    private int offset = 0;
    [SerializeField]
    private bool runOnce = false;
    [SerializeField]
    private bool accessory = false;
    private float timer;
    private float timerMax = .1f;
    private Renderer myRenderer;

    private void Awake()
    {
        myRenderer = gameObject.GetComponent<Renderer>();
    }

    private void LateUpdate()
    {
        timer -= Time.deltaTime;
        if (timer <= 0f)
        {
            timer = timerMax;
            myRenderer.sortingOrder = (int)(sortingOrderBase - (transform.position.y * 100) - offset);
            if (accessory)
            {
                //StartCoroutine(WaitNow());
                Renderer parentRenderer = gameObject.transform.parent.gameObject.GetComponent<Renderer>();
                myRenderer.sortingOrder = parentRenderer.sortingOrder + 1;
                if (runOnce && (myRenderer.sortingOrder != 1))
                {
                    Destroy(this);//see how multiplayer affects this?
                }
            } else if (runOnce)
            {
                Destroy(this);//see how multiplayer affects this?
            }
        }
    }
}

//https://www.youtube.com/watch?v=CTf0WjhfBx8
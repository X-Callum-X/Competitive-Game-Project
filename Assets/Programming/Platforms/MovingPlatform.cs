using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingPlatform : MonoBehaviour
{
    public LineRenderer path;

    public Vector3 pointA;
    public Vector3 pointB;

    public float moveSpeed = 5;

    bool toggleMovement = false;

    float lerpValue = 0;

    private void Start()
    {
        path.positionCount = 2;

        path.startWidth = 0.5f;
        path.SetPosition(0, pointA);
        path.SetPosition(1, pointB);
    }

    void FixedUpdate()
    {
        if (toggleMovement)
        {
            lerpValue += moveSpeed * Time.deltaTime;

            if (lerpValue >= 1f)
            {
                lerpValue = 1f;
                toggleMovement = false;
            }
        }
        else
        {
            lerpValue -= Time.fixedDeltaTime * moveSpeed;

            if (lerpValue <= 0f)
            {
                lerpValue = 0f;
                toggleMovement = true;
            }
        }

        transform.position = Vector3.Lerp(pointA, pointB, lerpValue);
    }
}

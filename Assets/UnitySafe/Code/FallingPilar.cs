using UnityEngine;

public class FallingPilar : MonoBehaviour
{

    Vector3 StartPoint;
    Vector3 EndPoint;

    public Transform endPoint;
    public float Speed = 2f;
    public float FallAcceleration = 10f;
    public float RiseSpeed = 1f;
    public float WaitTime = 1f;

    bool Falling = false;
    bool Waiting = true;
    bool WaitingAtTop = true;

    float CurrentFallSpeed = 0f;
    float WaitTimer = 0f;

    void Start()
    {
        StartPoint = transform.position;
        EndPoint = endPoint.position;
    }

    void Update()
    {
        if (Waiting)
        {
            WaitTimer += Time.deltaTime;

            if (WaitTimer >= WaitTime)
            {
                Waiting = false;
                WaitTimer = 0f;

                if (WaitingAtTop)
                {
                    Falling = true;
                    CurrentFallSpeed = Speed;
                }
            }
        }
        else if (Falling)
        {
            CurrentFallSpeed += FallAcceleration * Time.deltaTime;

            transform.position = Vector3.MoveTowards(
                transform.position,
                StartPoint,
                CurrentFallSpeed * Time.deltaTime
            );

            if (transform.position == StartPoint)
            {
                Falling = false;
                Waiting = true;
                WaitingAtTop = false;
                WaitTimer = 0f;
            }
        }
        else
        {
            transform.position = Vector3.MoveTowards(
                transform.position,
                EndPoint,
                RiseSpeed * Time.deltaTime
            );

            if (transform.position == EndPoint)
            {
                Waiting = true;
                WaitingAtTop = true;
                WaitTimer = 0f;
            }
        }
    }

}

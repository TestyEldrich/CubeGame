using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveEvent : MonoBehaviour
{
    [SerializeField] public float minSpeed;  // minimum range of speed to move
    [SerializeField] public float maxSpeed;  // maximum range of speed to move
    [SerializeField] private float speed;     // speed is a constantly changing value from the random range of minSpeed and maxSpeed 

    [SerializeField] public Vector3 randomDirection;                // Random, constantly changing direction from a narrow range for natural motion
    [SerializeField] public bool isMoving = false;
    [SerializeField] public bool onMoveEvent = false;

    private void Update() {
        if (isMoving && onMoveEvent) {
            transform.position += randomDirection * speed * Time.deltaTime;
        }
    }

    public void UpdateDirection() {
        if (onMoveEvent) {
            randomDirection = new Vector3(Random.Range(-1f, 1f), 0, Random.Range(-1f, 1f)).normalized;
            speed = Random.Range(minSpeed, maxSpeed);
            isMoving = true;
        }
        else { isMoving = false; }
    }

    public void InverseDirection() {
        randomDirection = -randomDirection;
        speed = Random.Range(minSpeed, maxSpeed);
        isMoving = true;
    }

    public void Move() {
        if (!onMoveEvent) {
            onMoveEvent = true;
            UpdateDirection();
        }
        else {
            onMoveEvent = false;
            isMoving = false;
        }
    }

    public void StopMovement() {
        isMoving = false;
        onMoveEvent = false;
    }
}

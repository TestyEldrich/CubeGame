using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class MoveEventManager : MonoBehaviour, IPointerDownHandler, IPointerUpHandler {
    [Header("Events")]
    public GameEvent onMoveEventStart;
    [SerializeField]
    private float rotationLimit = 40;
    [SerializeField]
    private float rotationSpeed = 15;
    private bool rotate = false;


    void FixedUpdate() {
        float targetRotate = rotate ? rotationLimit : 0f;

        // Rotate the cube by converting the angles into a quaternion.
        Quaternion target = Quaternion.Euler(targetRotate, 0, 0);

        // Dampen towards the target rotation
        transform.rotation = Quaternion.Slerp(transform.rotation, target, Time.deltaTime * rotationSpeed);
    }

    public void OnPointerDown(PointerEventData pointerEventData) {
        rotate = true;
    }

    public void OnPointerUp(PointerEventData pointerEventData) {
        rotate = false;
        if (!transform.GetComponentInParent<EventStats>().isSpawning) {
            onMoveEventStart.Raise(this, null);
        }
    }
}

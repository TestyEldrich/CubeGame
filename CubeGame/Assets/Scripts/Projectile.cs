using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    [Header("Events")]
    public GameEvent onTargetHit;
    public int cubeNumber;

    private void OnCollisionEnter(Collision collision) {
        if(collision.gameObject.tag == "Cube") {
            if(collision.transform.GetChild(0).GetChild(0).GetComponent<TextMeshProUGUI>().text == "" + cubeNumber) {
                Debug.Log("collision " + cubeNumber);
                onTargetHit.Raise(this, cubeNumber);
                collision.gameObject.GetComponentInParent<MoveEvent>().StopMovement();
            }
        }
    }
}

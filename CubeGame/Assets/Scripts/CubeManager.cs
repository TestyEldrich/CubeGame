using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CubeManager : MonoBehaviour
{
    [SerializeField] public string[] collisionTags;

    void OnCollisionEnter(Collision col) {

        if (col.gameObject.tag == collisionTags[0]) {
            transform.parent.GetComponent<MoveEvent>().InverseDirection();
            // use the above code as a template for all the collisionTags
            // add here.. and on.. and on..
        }else if (col.gameObject.tag == collisionTags[1]) {
            transform.parent.GetComponent<MoveEvent>().UpdateDirection();
        }
    }
}

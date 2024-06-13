using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI : MonoBehaviour
{
    private void LateUpdate() {
        transform.position = new Vector3(transform.parent.position.x, transform.parent.position.y + 0.5f, transform.parent.position.z-1);
        transform.rotation = Quaternion.LookRotation(Camera.main.transform.forward);
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem.Android;
using TMPro;

public class CubeSpawn : MonoBehaviour, IPointerDownHandler, IPointerUpHandler {
    [SerializeField]
    private float rotationLimit = 40;
    [SerializeField]
    private float rotationSpeed = 15;
    [SerializeField]
    private GameObject spawn;
    [SerializeField]
    private GameObject cubePrefab;
    [SerializeField]
    private GameObject cubeList;


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
        spawn = GameObject.FindGameObjectWithTag("Respawn");
        cubeList = GameObject.FindGameObjectWithTag("CubeList");
        rotate = false;
        StartCoroutine(Spawn());
    }

    IEnumerator Spawn() {
        foreach (Transform child in cubeList.transform) {
            Destroy(child.gameObject);
        }
        Vector3 spawnPoint = spawn.transform.position;
        int spawnAmount = Random.Range(1, 7);
        int counter = 1;
        while(counter != spawnAmount+1) {
            GameObject cube = Instantiate(cubePrefab, spawnPoint, Random.rotation);
            cube.transform.parent = cubeList.transform;
            cube.GetComponent<Renderer>().material.color = Random.ColorHSV(0f, 1f);
            GameObject textObject = cube.transform.GetChild(0).GetChild(0).gameObject;
            textObject.GetComponent<TextMeshProUGUI>().text = "" + counter;
            counter++;
            yield return new WaitForSeconds(0.5f);
        }
        Debug.Log("Spawn");
    }
}
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
    private GameObject parentCubePrefab;
    [SerializeField]
    private GameObject cubeList;
    [SerializeField] public Dictionary<int, GameObject> cubes = new Dictionary<int, GameObject>();


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
        if (!transform.GetComponentInParent<EventStats>().isSpawning) {
            StartCoroutine(Spawn());
        }
    }

    IEnumerator Spawn() {
        cubes.Clear();
        transform.GetComponentInParent<EventStats>().isSpawning = true;
        foreach (Transform child in cubeList.transform) {
            Destroy(child.gameObject);
        }
        Vector3 spawnPoint = spawn.transform.position;
        int spawnAmount = Random.Range(1, 7);
        transform.GetComponentInParent<EventStats>().cubeCount = spawnAmount;
        int counter = 1;
        while(counter != spawnAmount+1) {
            GameObject cube = Instantiate(parentCubePrefab, new Vector3(spawnPoint.x + Random.Range(-3f,3f), spawnPoint.y, spawnPoint.z + Random.Range(-3f, 3f)), Quaternion.Euler(0,0,0));
            cube.transform.parent = cubeList.transform;
            cube.transform.GetChild(0).GetComponent<Renderer>().material.color = Random.ColorHSV(0f, 1f);
            cube.transform.GetChild(0).GetChild(0).GetChild(0).gameObject.GetComponent<TextMeshProUGUI>().text = "" + counter;
            cube.transform.GetChild(0).rotation = Random.rotation;
            cubes.Add(counter, cube);
            counter++;
            yield return new WaitForSeconds(0.5f);
        }
        Debug.Log("Spawn");
        transform.GetComponentInParent<EventStats>().isSpawned = true;
        transform.GetComponentInParent<EventStats>().isSpawning = false;
    }
}

using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Runtime.CompilerServices;
using TMPro;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.XR;
using static UnityEditor.Experimental.AssetDatabaseExperimental.AssetDatabaseCounters;

public class ShootEvent : MonoBehaviour, IPointerDownHandler, IPointerUpHandler {
    [SerializeField] private GameObject projectilePrefab;
    [SerializeField] private GameObject cylinder;
    [SerializeField] private Dictionary<int, GameObject> cubes = new Dictionary<int, GameObject>();
    [SerializeField] private bool isShooting = false;
    [SerializeField] public int force = 500;

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
        if (!transform.GetComponentInParent<EventStats>().isSpawning && transform.GetComponentInParent<EventStats>().isSpawned) {
            if (!isShooting) {
                isShooting = true;
                GameObject firstButton = GameObject.Find("FirstButton");
                cubes = copyDictionary(firstButton.GetComponent<CubeSpawn>().cubes);
                StartCoroutine(Shoot());
            }else if (isShooting) {
                isShooting=false;
            }
        }
    }

    IEnumerator Shoot() {
        int randomCubeNumber;
        GameObject randomCube;
        Vector3 shootDirection;
        while (cubes.Count != 0) {
            GameObject projectile = Instantiate(projectilePrefab, cylinder.transform);
            randomCubeNumber = cubes.ElementAt(Random.Range(0, cubes.Count)).Key;
            cylinder.transform.GetChild(0).GetChild(0).gameObject.GetComponent<TextMeshProUGUI>().text = "" + randomCubeNumber;
            randomCube = cubes[randomCubeNumber];
            shootDirection = (cylinder.transform.position - randomCube.transform.GetChild(0).position).normalized;
            projectile.GetComponent<Projectile>().cubeNumber = randomCubeNumber;
            projectile.GetComponent<Rigidbody>().AddForce(shootDirection*-force, ForceMode.Force);
            GameObject.Destroy(projectile, 4);
            yield return new WaitForSeconds(2f);
        }
        cylinder.transform.GetChild(0).GetChild(0).gameObject.GetComponent<TextMeshProUGUI>().text = "End";
        isShooting = false;
    }

    public void ExcludeCube(Component sender, object data) {
        Debug.Log("Hit!");
        cubes.Remove((int)data);
    }

    private Dictionary<int, GameObject> copyDictionary(Dictionary<int, GameObject> dictionaryRef) {
        Dictionary<int, GameObject> cubes = new Dictionary<int, GameObject>();
        foreach(KeyValuePair<int, GameObject> entry in dictionaryRef) {
            cubes.Add(entry.Key, entry.Value);
        }
        return cubes;
}
}

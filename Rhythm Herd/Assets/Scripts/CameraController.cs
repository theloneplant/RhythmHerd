using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour {
    [SerializeField] private GameObject target;
    [SerializeField] private Vector3 offset;
    [SerializeField] private float speedMultiplier = 1;

    // Use this for initialization
    void Start () {
        transform.position = target.transform.position + offset;
        transform.LookAt(target.transform);
    }
	
	// Update is called once per frame
	void Update () {
        Vector3 moveDelta = (target.transform.position + offset) - transform.position;
        transform.position += moveDelta * Time.deltaTime * speedMultiplier;
	}

	public void setTarget(GameObject newTarget) {
		target = newTarget;
	}
}

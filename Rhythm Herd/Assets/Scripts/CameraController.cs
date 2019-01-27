using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour {
    [SerializeField] private Vector3 offset;
    [SerializeField] private float speedMultiplier = 1;

    private Vector3 target;

    // Use this for initialization
    void Start () {
        target = new Vector3(HerdMember.Target.x, 0, HerdMember.Target.y);
        transform.position = target + offset;
        transform.LookAt(target);
    }
	
	// Update is called once per frame
	void Update () {
        target = new Vector3(HerdMember.Target.x, 0, HerdMember.Target.y);
        Vector3 moveDelta = (target + offset) - transform.position;
        transform.position += moveDelta * Time.deltaTime * speedMultiplier;
	}
}

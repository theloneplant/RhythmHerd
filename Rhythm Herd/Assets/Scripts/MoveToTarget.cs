using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveToTarget : MonoBehaviour {
    [SerializeField] private float magnitude = 400;
    [SerializeField] private float destroyDistance = 5;

    private Vector3 startPosition;
    private float nextBeat;
    private float startTime;

    // Start is called before the first frame update
    void Start() {
        startPosition = transform.position;
        nextBeat = GameManager.instance.getNextBeatTime(3);
        startTime = Time.time;
    }

    // Update is called once per frame
    void Update() {
        GameObject target = GameManager.instance.getBarTarget();
        Vector3 direction = target.transform.position - transform.position;
        if (direction.magnitude <= destroyDistance && destroyDistance > 0) {
            Destroy(gameObject);
        }
        transform.position = Vector3.Lerp(startPosition, target.transform.position, (Time.time - startTime) / (nextBeat - startTime));
    }
}

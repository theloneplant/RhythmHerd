using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveToTarget : MonoBehaviour {
    [SerializeField] private float magnitude;
    [SerializeField] private float destroyDistance;

    private Vector3 velocity;

    // Start is called before the first frame update
    void Start() {
    }

    // Update is called once per frame
    void Update() {
        GameObject target = GameManager.instance.getBarTarget();
        Vector3 direction = target.transform.position - transform.position;
        if (direction.magnitude <= destroyDistance && destroyDistance > 0) {
            Destroy(gameObject);
        }
        velocity = Vector3.Normalize(direction) * magnitude;
        transform.position += Time.deltaTime * velocity;
        Debug.Log(transform.position);
    }
}

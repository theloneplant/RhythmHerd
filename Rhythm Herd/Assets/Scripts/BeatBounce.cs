using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BeatBounce : MonoBehaviour {
    [SerializeField] private float power;
    [SerializeField] private float multiplier;

    private GameManager manager;
    private Vector3 localScale;

    // Start is called before the first frame update
    void Start() {
        manager = GameManager.instance;
        localScale = transform.localScale;
    }

    // Update is called once per frame
    void Update() {
        float t = manager.getBeatScore();
        float y = Mathf.Pow(t, power);
        transform.localScale = localScale + new Vector3(y, y, y) * multiplier;
    }
}

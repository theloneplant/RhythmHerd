using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BeatBounce : MonoBehaviour {
    public float power;
    public float multiplier = 0.5f;
    public Vector4 points = new Vector4(0, 0, 0, 0.5f);

    private GameManager manager;
    private Vector3 localScale;

    // Start is called before the first frame update
    void Start() {
        manager = GameManager.instance;
        localScale = transform.localScale;
    }

    // Update is called once per frame
    void Update() {
        // (1-t)^3P0 + 3(1-t)^2tP1 + 3(1-t)t^2P2 + t^3P3
        float t = manager.getBeatScore();
        //float x = Mathf.Pow((1-t),3) * points.x + 3 * Mathf.Pow((1-t),2) * t * points.y 
        //        + (1-t)*Mathf.Pow(t,2) * points.z + Mathf.Pow(t,3) * points.w;
        float y = Mathf.Pow(t, power);
        transform.localScale = localScale + new Vector3(y, y, y) * multiplier;
    }
}

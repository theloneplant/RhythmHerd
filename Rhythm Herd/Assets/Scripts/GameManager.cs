using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour {
	[SerializeField] private CameraController camera;
	[SerializeField] private Herd controller;
    [SerializeField] private GameObject barPrefab;
    [SerializeField] private Canvas canvas;
    [SerializeField] private GameObject barTarget;
    [SerializeField] private float offset;
	[SerializeField] private int bpm;

    public delegate void BeatDelegate();

    private List<BeatDelegate> callbacks;
	private float startTime;
    private float previousBeat;
    private float beatInterval;

    public static GameManager instance { get; private set; }

    private void Awake() {
        if (instance != null && instance != this) {
            Destroy(this.gameObject);
        }
        else {
            instance = this;
        }
    }

    // Use this for initialization
    void Start () {
        callbacks = new List<BeatDelegate>();
		controller = GetComponent<Herd> ();
        startTime = Time.time;
        beatInterval = 60.0f / bpm;
        previousBeat = startTime + offset - beatInterval;
	}
	
	// Update is called once per frame
	void Update () {
        // Determine if a beat hit, notify everyone listening for the beat
        if (Time.time - previousBeat >= beatInterval) {
            playBeat();
            float elapsed = Time.time - startTime + offset;
            previousBeat = elapsed - (elapsed % beatInterval);
        }
    }

    public void playBeat() {
        foreach(BeatDelegate callback in callbacks) {
            callback();
        }

        // Create UI bars

    }

    public void onBeat(BeatDelegate callback) {
        callbacks.Add(callback);
    }

    public float getBeatScore() {
		float elapsed = Time.time - startTime + offset;
		float beatPosition = elapsed % beatInterval;
		float center = beatInterval / 2.0f;
		return (Mathf.Abs(beatPosition - center) / beatInterval) * 2;
	}

    public GameObject getBarTarget() {
        return barTarget;
    }
}

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

    private event BeatDelegate callbacks;
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
		controller = GetComponent<Herd> ();
        startTime = Time.time;
        beatInterval = 60.0f / bpm;
        previousBeat = startTime + offset - beatInterval;
        onBeat(test);
	}

    // Update is called once per frame
    void Update () {
        // Determine if a beat hit, notify everyone listening for the beat
        if (Time.time - previousBeat >= beatInterval) {
            playBeat();
            Debug.Log(previousBeat + " - " + Time.time);
            previousBeat = getNextBeatTime() - beatInterval;
        }
    }

    public void playBeat() {
        // Create UI bars
        Vector3 barOffset = new Vector3(1000, 0, 0);
        Vector3 leftBarPosition = barTarget.transform.position - barOffset;
        Vector3 rightBarPosition = barTarget.transform.position + barOffset;
        GameObject leftBar = Instantiate(barPrefab, leftBarPosition, Quaternion.identity);
        GameObject rightBar = Instantiate(barPrefab, rightBarPosition, Quaternion.identity);
        leftBar.transform.SetParent(canvas.transform);
        rightBar.transform.SetParent(canvas.transform);

        callbacks();
    }

    private void test() {

    }

    public void onBeat(BeatDelegate callback) {
        callbacks += callback;
    }

    public float getNextBeatTime(float numberOfBeatsAhead = 1) {
        float elapsed = Time.time - startTime + offset;
        return elapsed + (beatInterval * numberOfBeatsAhead) - (elapsed % beatInterval);
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

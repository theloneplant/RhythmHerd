using UnityEngine;

public class GameManager : MonoBehaviour {
	[SerializeField] private CameraController camera;
	[SerializeField] private PlayerController controller;
	[SerializeField] private float offset;
	[SerializeField] private int bpm;

	private float startTime;

	// Use this for initialization
	void Start () {
		controller = GetComponent<PlayerController> ();
		startTime = Time.time;
	}
	
	// Update is called once per frame
	void Update () {
		float score = getBeatScore ();
		if (score > 0.95f) {
			Debug.Log("score hit! " + Time.time);
		}
	}

	public float getBeatScore() {
		float currentTime = Time.time;
		float bps = bpm / 60.0f;
		float elapsed = currentTime - startTime + offset;
		float beatPosition = elapsed % bps;
		float center = bps / 2.0f;
		return (Mathf.Abs(beatPosition - center) / bps) * 2;
	}
}

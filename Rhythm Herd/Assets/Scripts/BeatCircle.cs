using UnityEngine;
using UnityEngine.UI;

public class BeatCircle : MonoBehaviour
{
    private struct Constants
    {
        public static readonly int radius = Shader.PropertyToID("_Radius");
    }

    private GameManager manager;
    private Material circleMaterial;

    private void Start()
    {
        manager = GameManager.instance;
        circleMaterial = GetComponent<Image>().material;
    }

    private void Update()
    {
        float score = manager.getBeatScore();
        score *= score;
        circleMaterial.SetFloat(Constants.radius, 1f - score);
    }
}

using System.Collections;
using UnityEngine;

public class Jump : MonoBehaviour
{
    public float jumpDuration;
    public float height;

    private void Start()
    {
        GameManager.OnBeat += JumpOnBeat;
    }

    private void JumpOnBeat()
    {
        Debug.Log("Stuff");
        StartCoroutine(JumpOnBeatCoroutine());
    }

    private IEnumerator JumpOnBeatCoroutine()
    {
        for (float time = 0f; time < 1f; time += Time.deltaTime / jumpDuration)
        {
            Vector3 position = transform.position;
            float y = time * 2f - 1f;
            position.y = (1f - y * y) * height;
            transform.position = position;
            yield return null;
        }
    }

    private void SetToGround()
    {
        Vector3 position = transform.position;
        position.y = 0f;
        transform.position = position;
    }
}

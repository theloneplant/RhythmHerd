using UnityEngine;
using UnityEngine.SceneManagement;

public class Restart : MonoBehaviour
{
    public Herd herd;

    public void PlayAgain()
    {
        SceneManager.LoadScene(0);
    }
}

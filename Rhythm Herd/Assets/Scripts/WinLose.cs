using UnityEngine;

public class WinLose : MonoBehaviour
{
    public Transform homeLocation = null;
    public Herd herd = null;
    public float homeRadius = 1f;

    public delegate void WinAction();
    public static event WinAction OnWin;

    public delegate void LoseAction();
    public static event LoseAction OnLose;

    private bool hasFinished;

    private void Update()
    {
        if (!hasFinished)
        {
            var location = new Vector2 { x = homeLocation.position.x, y = homeLocation.position.z, };
            var distance = Vector2.Distance(location, HerdMember.Target);
            if (distance < homeRadius)
            {
                hasFinished = true;
                Debug.Log("Won the game.");
                OnWin?.Invoke();
            }
            else if (herd.JoinedCount() == 0)
            {
                hasFinished = true;
                Debug.Log("Lost the game.");
                OnLose?.Invoke();
            }
        }
    }
}

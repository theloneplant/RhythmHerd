using UnityEngine;

public class WinParticles : MonoBehaviour
{
    [SerializeField] private Herd herd;

    private void Start()
    {
        WinLose.OnWin += PlayEffect;
    }

    private void PlayEffect()
    {
        herd.CheerAll(); 
    }
}

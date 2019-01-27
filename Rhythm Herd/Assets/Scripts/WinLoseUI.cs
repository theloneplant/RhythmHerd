using UnityEngine;

public class WinLoseUI : MonoBehaviour
{
    public GameObject WinUi;
    public GameObject LoseUi;

    private void Start()
    {
        WinUi.SetActive(false);
        LoseUi.SetActive(false);
        WinLose.OnWin += () => WinUi.SetActive(true);
        WinLose.OnLose += () => LoseUi.SetActive(true);
    }
}

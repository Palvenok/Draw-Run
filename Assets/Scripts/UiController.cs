using UnityEngine;
using UnityEngine.UI;

public class UiController : MonoBehaviour
{
    [SerializeField] private Text score;
    [SerializeField] private GameObject drawTip;


    public void UpdateScore(int score)
    {
        this.score.text = "Enemies: " + score.ToString();
    }

    public void ChangeTipStatus(bool isShow)
    {
        drawTip.SetActive(isShow);
    }
}

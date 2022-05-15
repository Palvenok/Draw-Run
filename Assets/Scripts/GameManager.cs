using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager main;

    [SerializeField] private UiController uiController;
    [SerializeField] private GameObject playerPrefab;
    [SerializeField] private Draw draw;

    public Draw Draw => draw;

    private void Awake()
    {
        if(main != null)
        { 
            Destroy(gameObject);
            return;
        }
        main = this;
    }
}

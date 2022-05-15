using System.Collections;
using UnityEngine;

public class Level : MonoBehaviour
{
    private float _levelSpeed = 1;

    public float Speed { set { _levelSpeed = value; } }

    private bool isGameActive;
    public void StartLevel()
    {
        if (isGameActive) return;
        isGameActive = true;
        StartCoroutine(MoveLevel());
    }

    public void StopLevel()
    {
        StopAllCoroutines();
        Debug.Log("Stoped");
        isGameActive = false;
    }

    private IEnumerator MoveLevel()
    {
        while (isGameActive)
        {
            yield return new WaitForFixedUpdate();
            transform.Translate(new Vector3(0, 0, -.01f * _levelSpeed));
        }
    }
}

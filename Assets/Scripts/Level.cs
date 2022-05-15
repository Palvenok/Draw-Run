using System;
using System.Collections;
using UnityEngine;

public class Level : MonoBehaviour
{
    [SerializeField] private ParticleSystem[] fireworks;
    private float _levelSpeed = 1;

    public float Speed { set { _levelSpeed = value; } }

    private bool isGameActive;
    public void StartLevel()
    {
        if (isGameActive) return;
        isGameActive = true;
        StartCoroutine(MoveLevel());
    }

    public void StopLevel(bool isWin)
    {
        StopAllCoroutines();
        isGameActive = false;
        if (isWin) StartCoroutine(Win());
    }

    private IEnumerator Win()
    {
        while(true)
        {
            fireworks[UnityEngine.Random.Range(0, fireworks.Length)].Play();
            yield return new WaitForSeconds(.5f);
        }
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

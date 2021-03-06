using UnityEngine;
using Dreamteck.Splines;
using System;
using System.Collections;
using UnityEngine.Events;

public class CharController : MonoBehaviour
{
    public UnityEvent<GameObject> OnTrigger;
    public UnityEvent<CharController> Ondestroy;

    [SerializeField] private Animator anim;
    [SerializeField] private ParticleSystem poof;

    public void Move(Vector3 vector3)
    {
        if(GameManager.main.IsGameStarted) anim.SetBool("Running", true);
        StartCoroutine(MoveToPoint(vector3));
    }

    public void Win()
    {
        anim.SetBool("Running", false);
        anim.SetFloat("Dance", (int)UnityEngine.Random.Range(0, 3));
        anim.SetBool("Win", true);
    }

    private IEnumerator MoveToPoint(Vector3 point)
    {
        for (int i = 0; i < 100; i++)
        {
            yield return new WaitForFixedUpdate();
            transform.position = Vector3.Lerp(transform.position, point, Time.deltaTime);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        OnTrigger?.Invoke(other.gameObject);

        if(other.CompareTag("Trap"))
        {
            var p = Instantiate(poof, GameManager.main.CurrentLevel.transform);
            p.transform.position = transform.position;
            p.transform.localScale = Vector3.one * 1.15f;
            Destroy(gameObject);
        }
    }

    private void OnDestroy()
    {
        Ondestroy?.Invoke(this);
        StopAllCoroutines();
        OnTrigger?.RemoveAllListeners();
    }
}

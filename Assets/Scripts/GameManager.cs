using Dreamteck.Splines;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager main;

    [Header("Utils")]
    [SerializeField] private UiController uiController;
    [SerializeField] private SplineModify spline;
    [SerializeField] private GameObject playerPrefab;
    [SerializeField] private Draw draw;

    [Space, Header("Game settings")]
    [SerializeField] private LevelsConfig levelsConfig;
    [SerializeField] private int startEnemyCount = 10;
    [SerializeField] private float levelSpeed = 5;


    private Level _currentLevel;
    private Vector3[] _cachedPoints;
    private List<CharController> _chars = new List<CharController>();
    private bool _isGameStarted = false;
    private bool _isGameFinished = false;

    public Level CurrentLevel => _currentLevel;
    public Draw Draw => draw;
    public bool IsGameStarted => _isGameStarted;

    private void Awake()
    {
        if(main != null)
        { 
            Destroy(gameObject);
            return;
        }
        main = this;
    }

    private void Start()
    {
        spline.OnSplineUpdate.AddListener(CharsUpdate);
        uiController.ChangeTipStatus(!_isGameStarted);

        draw.OnLineUpdate.AddListener((Vector3[] v) => 
        {
            if (!_isGameStarted && !_isGameFinished) _isGameStarted = true;

            uiController.ChangeTipStatus(!_isGameStarted);
            StartGame(); 
        });


        _currentLevel = Instantiate(levelsConfig.GetRandomLevel());
        _currentLevel.Speed = levelSpeed;
        CharInst();

    }

    private void StartGame()
    {
        _currentLevel.StartLevel();
    }

    private void StopGame(bool isFinish)
    {
        _currentLevel?.StopLevel(isFinish);
        _isGameFinished = true;

        if(!isFinish)
        {
            StartCoroutine(RestartOnDelay(1));
            return;
        }

        StartCoroutine(RestartOnDelay(8));

        foreach (CharController c in _chars)
        {
            c.Win();
        }
    }

    private IEnumerator RestartOnDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    private void CharInst()
    {
        var points = new Vector3[startEnemyCount];
        foreach (var p in points)
        {
            var inst = Instantiate(playerPrefab, spline.transform);
            var c = inst.GetComponent<CharController>();
            c.OnTrigger.AddListener(OnCharTrigger);
            c.Ondestroy.AddListener(OnCharDestroy);
            _chars.Add(c);
        }
        CharsUpdate(points);
    }

    private void OnCharTrigger(GameObject obj)
    {
        switch (obj.tag)
        {
            case "Enemy":
                obj.transform.parent = spline.transform;
                obj.tag = "Player";
                var c = obj.GetComponent<CharController>();
                c.OnTrigger.AddListener(OnCharTrigger);
                c.Ondestroy.AddListener(OnCharDestroy);
                _chars.Add(c);
                CharsUpdate(_cachedPoints);
                break;
            case "Finish":
                StopGame(true);
                break;
            case "Trap":
                break;
        }
    }

    private void OnCharDestroy(CharController charController)
    {
        _chars.Remove(charController);
        uiController.UpdateScore(_chars.Count);
        if (_chars.Count == 0) StopGame(false);
    }

    private void CharsUpdate(Vector3[] points)
    {
        _cachedPoints = points;

        for (int i = 0; i < _chars.Count; i++)
        {
            _chars[i].StopAllCoroutines();
            var index = i;
            if (index > spline.Spline.pointCount - 1) index = spline.Spline.pointCount - 1;
            _chars[i].Move(spline.Spline.GetPointPosition(index));
        }

        uiController.UpdateScore(_chars.Count);
    }

}

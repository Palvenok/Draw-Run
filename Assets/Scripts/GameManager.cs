using Dreamteck.Splines;
using System;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager main;

    [Header("Utils")]
    [SerializeField] private UiController uiController;
    [SerializeField] private SplineModify spline;
    [SerializeField] private GameObject playerPrefab;
    [SerializeField] private Draw draw;
    [SerializeField] private Animator anim;

    [Space, Header("Game settings")]
    [SerializeField] private LevelsConfig levelsConfig;
    [SerializeField] private int startEnemyCount = 10;
    [SerializeField] private float levelSpeed = 5;


    private Level _currentLevel;
    private Vector3[] _cachedPoints;
    private List<CharController> _chars = new List<CharController>();
    private bool _isGameStarted = false;

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
            if (!_isGameStarted) _isGameStarted = true;

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

    private void StopGame()
    {
        _currentLevel?.StopLevel();

        foreach(CharController c in _chars)
        {
            c.Win();
        }

    }

    private void CharInst()
    {
        var points = new Vector3[startEnemyCount];
        foreach (var p in points)
        {
            var inst = Instantiate(playerPrefab, spline.transform);
            var c = inst.GetComponent<CharController>();
            c.OnTrigger.AddListener(OnCharTrigger);
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
                var c = obj.GetComponent<CharController>();
                c.OnTrigger.AddListener(OnCharTrigger);
                _chars.Add(c);
                CharsUpdate(_cachedPoints);
                break;
            case "Finish":
                StopGame();
                break;
        }
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
    }

}

using UnityEngine;

[CreateAssetMenu(fileName = "LevelsConfig", menuName = "Configs/LevelsConfig", order = 0)]
public class LevelsConfig : ScriptableObject
{
    [SerializeField] private Level[] _levels;

    public Level GetRandomLevel()
    {
        return _levels[Random.Range(0, _levels.Length)];
    }
}

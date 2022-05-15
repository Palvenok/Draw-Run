using Dreamteck.Splines;
using UnityEngine;
using UnityEngine.Events;

public class SplineModify : MonoBehaviour
{
    public UnityEvent<Vector3[]> OnSplineUpdate;

    [SerializeField] private SplineComputer spline;
    [SerializeField] private Vector2 offset = Vector2.one;

    public SplineComputer Spline => spline;

    private void Start()
    {
        if (spline == null) spline = GetComponent<SplineComputer>();
        GameManager.main.Draw.OnLineUpdate.AddListener(UpdateSpline);
    }

    private void UpdateSpline(Vector3[] points)
    {
        var sPoints = new SplinePoint[points.Length];
        Vector3[] positions = new Vector3[points.Length];

        for (int i = 0; i < points.Length; i++)
        {
            var position = CalcPosition(points[i]);
            sPoints[i] = new SplinePoint(position);
            positions[i] = position;
        }

        spline.SetPoints(sPoints);
        spline.Rebuild(true);

        OnSplineUpdate.Invoke(positions);
    }

    private Vector3 CalcPosition(Vector3 point)
    {
        Vector3 position = Vector3.zero;

        position.z = (point.y - GameManager.main.Draw.transform.position.y) * offset.y;
        position.x = point.x * offset.x;
        position.y = .15f;

        return position;
    }

    private void OnDestroy()
    {
        OnSplineUpdate.RemoveAllListeners();
    }
}

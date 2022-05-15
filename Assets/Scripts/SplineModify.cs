using Dreamteck.Splines;
using UnityEngine;

public class SplineModify : MonoBehaviour
{
    [SerializeField] private Spline spline;

    private void Start()
    {
        if (spline == null) spline = GetComponent<Spline>();
        GameManager.main.Draw.OnLineUpdate.AddListener(UpdateSpline);
    }

    private void UpdateSpline(Vector3[] points)
    {
        spline.points = new SplinePoint[points.Length];

        for (int i = 0; i < points.Length; i++)
        {
            spline.points[i].SetPosition(CalcPosition(points[i]));
            spline.points[i].color = Color.red;
            spline.points[i].size = 10f;
        }
    }

    private Vector3 CalcPosition(Vector3 point)
    {
        Vector3 position = Vector3.zero;

        position.z = Mathf.Pow(point.y * point.y + point.z * point.z, .5f) 
                       + Camera.main.gameObject.transform.position.z;
        position.x = point.x;
        position.y = .15f;

        return position;
    }
}

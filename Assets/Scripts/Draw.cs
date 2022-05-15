using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Draw : MonoBehaviour
{
    public UnityEvent<Vector3[]> OnLineUpdate; 

    [SerializeField] private LineRenderer linePrefab;

    private Camera cam;
    private LineRenderer currentTrail;
    private List<Vector3> points = new List<Vector3>();

    private void Start()
    {
        cam = Camera.main;
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            CreateNewLine();
        }

        if (Input.GetMouseButton(0))
        {
            AddPoint();
        }

        if (Input.GetMouseButtonUp(0))
        {
            if (transform.childCount != 0)
            {
                foreach (Transform R in transform)
                {
                    Destroy(R.gameObject);
                }
            }
        }
    }

    private void CreateNewLine()
    {
        currentTrail = Instantiate(linePrefab);
        currentTrail.transform.SetParent(transform, true);
        points.Clear();
    }

    private void UpdateLinePoints()
    {
        if (currentTrail != null && points.Count > 1)
        {
            currentTrail.positionCount = points.Count;
            currentTrail.SetPositions(points.ToArray());
            OnLineUpdate.Invoke(points.ToArray());
        }
    }

    private void AddPoint()
    {        
        var Ray = cam.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        if (Physics.Raycast(Ray, out hit))
        {
            if (hit.collider.CompareTag("DrawPanel"))
            {
                // points.Add(new Vector3(hit.point.x, 0f, hit.point.z));
                points.Add(hit.point);
                UpdateLinePoints();
                return;
            }
            else
                return;
        }
        
    }

    private void OnDestroy()
    {
        OnLineUpdate.RemoveAllListeners();
    }
}

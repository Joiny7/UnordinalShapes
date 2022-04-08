using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class MainController : MonoBehaviour
{
    [SerializeField]
    private GameObject PointPrefab;

    [SerializeField]
    private GameObject AboutText;

    [SerializeField]
    private Text PositionalData;

    [SerializeField]
    private LineRenderer LineRenderer;

    [SerializeField]
    private LineRenderer CircleRenderer;

    private List<Point> CurrentPoints = new List<Point>();
    //private GameObject PhantomPoint = null;
    private float CircleArea = 0;
    private float ParallelogramArea = 0;

    void Update()
    {
        CheckForClicks();
    }

    private void Start()
    {
        Setup();
    }

    private void Setup()
    {
        LineRenderer.startWidth = 0.05f;
        LineRenderer.endWidth = 0.05f;
        LineRenderer.startColor = Color.blue;
        LineRenderer.endColor = Color.blue;

        CircleRenderer.startWidth = 0.1f;
        CircleRenderer.endWidth = 0.1f;
        CircleRenderer.startColor = Color.yellow;
        CircleRenderer.endColor = Color.yellow;
    }

    private bool PointHere2(Vector3 pos)
    {
        Vector2 v = new Vector2(pos.x, pos.y);
        RaycastHit2D hit = Physics2D.Raycast(v, Vector2.zero);

        if (hit.collider == null)
            return false;
        else 
            return true;
    }

    private void CheckForClicks()
    {
        Vector3 mousePosition = Input.mousePosition;
        Vector3 objectPos = Camera.main.ScreenToWorldPoint(mousePosition);
        objectPos.z = 0f;

        if (Input.GetButtonDown("Fire1") && !PointHere2(objectPos) && CurrentPoints.Count < 3)
        {
            var p = Instantiate(PointPrefab, objectPos, Quaternion.identity);
            CurrentPoints.Add(p.GetComponent<Point>());

            if (CurrentPoints.Count == 3)
            {
                DrawShapes();
            }

            PrintPositionalData();
        }
    }

    public void PrintPositionalData()
    {
        string s = "";

        for (int i = 0; i < CurrentPoints.Count; i++)
        {
            s += "Point " + (i + 1) + " position: " + CurrentPoints[i].transform.position.ToString() + "\n";
        }

        if (CurrentPoints.Count == 3)
        {
            s += "Parallelogram area: " + ParallelogramArea + "\n";
            s += "Circle area: " + CircleArea + "\n";
        }

        PositionalData.text = s;
    }

    private void CalculateParralelogramArea()
    {
        ParallelogramArea = 1f;
    }

    private void CalculateCircleArea()
    {
        CircleArea = 1f;
    }

    private void DrawShapes()
    {
        DrawParallelogram();
        DrawCircle();
        CalculateParralelogramArea();
        CalculateCircleArea();
    }

    public void ReDrawShapes()
    {
        if(CurrentPoints.Count == 3)
        {
            DrawParallelogram();
            DrawCircle();
            CalculateParralelogramArea();
            CalculateCircleArea();
        }
    }

    private void DrawParallelogram()
    {
        //old version
        LineRenderer.positionCount = 4;
        Vector3[] positions = new Vector3[4];

        for (int i = 0; i < CurrentPoints.Count; i++)
        {
            positions[i] = CurrentPoints[i].transform.position;
        }

        positions[3] = GetPhantomPoint();
        LineRenderer.SetPositions(positions);

    }

    private Vector3 GetPhantomPoint()
    {
        //minor fancy math magic
        float x = (CurrentPoints[0].transform.position.x - CurrentPoints[1].transform.position.x + CurrentPoints[2].transform.position.x);
        float y = (CurrentPoints[0].transform.position.y - CurrentPoints[1].transform.position.y + CurrentPoints[2].transform.position.y);
        Vector3 v = new Vector3(x, y, 0);
        return v;
    }

    private void DrawCircle()
    {

    }

    public void ResetShapes()
    {
        var tempList = CurrentPoints.ToArray();

        for (int i = 0; i < tempList.Length; i++)
        {
            Destroy(tempList[i].gameObject);
        }

        CurrentPoints.Clear();
        LineRenderer.positionCount = 0;
        PrintPositionalData();
    }

    public void ShowAbout()
    {
        if (AboutText.activeInHierarchy)
            AboutText.SetActive(false);
        else
            AboutText.SetActive(true);
    }
}
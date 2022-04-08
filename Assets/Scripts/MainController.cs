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
    private float Area = 0;

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
        CircleRenderer.startColor = Color.yellow;
        CircleRenderer.endColor = Color.yellow;
        CircleRenderer.startWidth = 0.05f;
        CircleRenderer.endWidth = 0.05f;
    }

    //To help check so that you dont spawn point when trying to drag a point
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
            s += "Parallelogram area: " + Area + "\n";
            s += "Circle area: " + Area + "\n";
        }

        PositionalData.text = s;
    }

    //Calculates area from the parallelogram, which we then use for creating the circle
    private void CalculateArea()
    {
        var f = CalculateAngleForParallelogram();
        var dist1 = Vector3.Distance(CurrentPoints[0].transform.position, CurrentPoints[1].transform.position);
        var dist2 = Vector3.Distance(CurrentPoints[1].transform.position, CurrentPoints[2].transform.position);
        var sin = Mathf.Sin(f * Mathf.Deg2Rad);
        Area = (dist1 * dist2 * sin);
    }

    private float CalculateAngleForParallelogram()
    {
        float angle = Vector3.Angle(CurrentPoints[1].transform.position - CurrentPoints[0].transform.position, CurrentPoints[1].transform.position - CurrentPoints[2].transform.position);
        return angle;
    }

    private void DrawShapes()
    {
        DrawParallelogram();
        CalculateArea();
        DrawCircle();
    }

    //Different from DrawShapes only in that it's used for updating when moving points
    public void ReDrawShapes()
    {
        if(CurrentPoints.Count == 3)
        {
            DrawParallelogram();
            DrawCircle();
            CalculateArea();
        }

        PrintPositionalData();
    }

    private void DrawParallelogram()
    {
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
        float x = (CurrentPoints[0].transform.position.x - CurrentPoints[1].transform.position.x + CurrentPoints[2].transform.position.x);
        float y = (CurrentPoints[0].transform.position.y - CurrentPoints[1].transform.position.y + CurrentPoints[2].transform.position.y);
        Vector3 v = new Vector3(x, y, 0);
        return v;
    }

    private void DrawCircle()
    {
        //vertexNumber is hardcoded only for simplicity
        int vertexNumber = 60;
        Vector3 center = FindCircleCenter();
        float radius = Mathf.Sqrt((Area / Mathf.PI));
        float angle = 2 * Mathf.PI / vertexNumber;
        CircleRenderer.positionCount = vertexNumber;

        for (int i = 0; i < vertexNumber; i++)
        {
            Matrix4x4 rotationMatrix = new Matrix4x4(new Vector4(Mathf.Cos(angle * i), Mathf.Sin(angle * i), 0, 0),
                                                     new Vector4(-1 * Mathf.Sin(angle * i), Mathf.Cos(angle * i), 0, 0),
                                       new Vector4(0, 0, 1, 0),
                                       new Vector4(0, 0, 0, 1));
            Vector3 initialRelativePosition = new Vector3(0, radius, 0);
            CircleRenderer.SetPosition(i, center + rotationMatrix.MultiplyPoint(initialRelativePosition));
        }
    }

    private Vector3 FindCircleCenter()
    {
        var x = (CurrentPoints[0].transform.position.x + CurrentPoints[2].transform.position.x) / 2;
        var y = (CurrentPoints[1].transform.position.y + GetPhantomPoint().y) / 2;
        Vector3 v = new Vector3(x, y);
        return v;
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
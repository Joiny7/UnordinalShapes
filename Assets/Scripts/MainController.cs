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
        LineRenderer.startWidth = 0.3f;
        LineRenderer.endWidth = 0.3f;
        LineRenderer.startColor = Color.blue;
        LineRenderer.endColor = Color.blue;

        CircleRenderer.startWidth = 0.3f;
        CircleRenderer.endWidth = 0.3f;
        CircleRenderer.startColor = Color.yellow;
        CircleRenderer.endColor = Color.yellow;
    }

    private void CheckForClicks()
    {
        if (Input.GetButtonDown("Fire1") && CurrentPoints.Count < 3)
        {
            Vector3 mousePosition = Input.mousePosition;
            Vector3 objectPos = Camera.main.ScreenToWorldPoint(mousePosition);
            objectPos.z = 0f;
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
            s += "Parallelogram area: " + CalculateParralelogramArea() + "\n";
            s += "Circle area: " + CalculateCircleArea() + "\n";
        }

        PositionalData.text = s;
    }

    private float CalculateParralelogramArea()
    {
        return 1f;
    }

    private float CalculateCircleArea()
    {
        return 1f;
    }

    public void DrawShapes()
    {
        LineRenderer.positionCount = 3;
        Vector3[] positions = new Vector3[3];

        for (int i = 0; i < CurrentPoints.Count; i++)
        {
            positions[i] = CurrentPoints[i].transform.position;
        }

        LineRenderer.SetPositions(positions);
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
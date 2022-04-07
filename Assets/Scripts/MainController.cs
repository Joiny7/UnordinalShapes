using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainController : MonoBehaviour
{
    [SerializeField]
    private GameObject PointPrefab;

    private List<Point> CurrentPoints = new List<Point>();

    void Update()
    {
        CheckForClicks();
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
        }
    }

    public void DrawShapes()
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

        //Add code to remove shapes
    }

    public void ShowAbout()
    {

    }
}
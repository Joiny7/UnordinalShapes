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
        }
    }
}
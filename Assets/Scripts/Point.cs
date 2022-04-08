using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Point : MonoBehaviour
{
    private Vector2 PreviousPosition;
    private MainController Main;

    private void Start()
    {
        PreviousPosition = transform.position;
        Main = FindObjectOfType<MainController>();
    }

    private void OnMouseDrag()
    {
        Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        transform.position = mousePos;
        Main.ReDrawShapes();
    }
}
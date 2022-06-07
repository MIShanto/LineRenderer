    using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(LineRenderer))]
public class LineManager : MonoBehaviour
{
    public List<Transform> nodes;
    LineRenderer lr;
    Color lrOriginalColor;
    private void Start()
    {
        lr = GetComponent<LineRenderer>();

        lr.positionCount = nodes.Count;

        lrOriginalColor = lr.material.color;
    }

    private void Update()
    {
        lr.SetPositions(nodes.ConvertAll(n => n.position).ToArray());
    }

    public Vector3[] Getpositions()
    {
        Vector3[] positions = new Vector3[lr.positionCount];

        lr.GetPositions(positions);

        return positions;
    }

    public float GetWidth()
    {
        return lr.startWidth;
    }

    private void OnMouseDown()
    {
        Debug.Log(gameObject.name);
    }

    private void OnMouseOver()
    {
        lr.material.color = Color.black;
    }

    private void OnMouseExit()
    {
        lr.material.color = lrOriginalColor;
    }

}


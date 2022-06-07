using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(LineManager), typeof(PolygonCollider2D))]
public class LineCollisionManager : MonoBehaviour
{
    LineManager lm;

    PolygonCollider2D polygonCollider2D;
    // Start is called before the first frame update

    List<Vector2> colliderPoints = new List<Vector2>(); // points to draw collider
    void Awake()
    {
        lm = GetComponent<LineManager>();
        polygonCollider2D = GetComponent<PolygonCollider2D>();
    }

    // Update is called once per frame
   

    private void LateUpdate()
    {
        Vector3[] positions = lm.Getpositions();

        if (positions.Length >= 2)
        {
            //Get the number of line between two points
            int numberOfLines = positions.Length - 1;

            polygonCollider2D.pathCount = numberOfLines;

            //get collider points between two consecutive points
            for (int i = 0; i < numberOfLines; i++)
            {
                List<Vector2> currentPositions = new List<Vector2>
                {
                    positions[i],
                    positions[i+1],
                };

                List<Vector2> currentColiderPoints = CalculateColliderPoints(currentPositions);
                polygonCollider2D.SetPath(i, currentColiderPoints.ConvertAll(p => (Vector2)transform.InverseTransformPoint(p)));
            }

        }
        else
            polygonCollider2D.pathCount = 0; 
        
    }

/*    private void OnDrawGizmos()
    {
        Gizmos.color = Color.black;
        if (colliderPoints != null) colliderPoints.ForEach(p => Gizmos.DrawSphere(p, 0.1f));
    }*/
    private List<Vector2> CalculateColliderPoints(List<Vector2> positions)
    {
        //get the width of the line
        float width = lm.GetWidth();

        //m = (y2-y1)/(x2-x1)
        float m = (positions[1].y - positions[0].y) / (positions[1].x - positions[0].x);
        float deltaX = (width / 2f) * (m / Mathf.Pow(m * m + 1, 0.5f));
        float deltaY = (width / 2f) * (1 / Mathf.Pow(1 + m * m, 0.5f));

        //calculate offset from each point to collision vertex
        Vector2[] offsets = new Vector2[2];
        offsets[0] = new Vector2(-deltaX, deltaY);
        offsets[1] = new Vector2(deltaX, -deltaY);


        //generate collider vertices
        List<Vector2> colliderPositions = new List<Vector2>
        {
            positions[0] + offsets[0],
            positions[1] + offsets[0],
            positions[1] + offsets[1],
            positions[0] + offsets[1]
        };

        return colliderPositions;
    }
}

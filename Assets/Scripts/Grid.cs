﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//using UnityEditor;

public class Grid : MonoBehaviour
{
    public Collider Collider;
    public GridElements[,] GridContent;
    public int Rows;
    public int Cols;
    public SimulationElement[] Elements; 

    private void Awake() {
        Cols = (int) Collider.bounds.size.z;
        Rows = (int) Collider.bounds.size.x;

        GridContent = new GridElements[Rows, Cols];

        for(var x = 0; x < Cols; x++) {
            for(var y = 0; y < Rows; y++) {
                GridContent[x, y] = GridElements.EMPTY;
            }
        }

        foreach(var element in Elements) {
            AddElement(element.Collider, element.Type);
        }
    }

    public void AddElement(Collider col, GridElements type) {
        var xStart = col.transform.position.x - col.bounds.extents.x;
        var xEnd = col.transform.position.x + col.bounds.extents.x;

        var zStart = col.transform.position.z - col.bounds.extents.z;
        var zEnd = col.transform.position.z + col.bounds.extents.z;

        var startCell = GetCellCoordinate(new Vector3(xStart, 0, zStart));
        var endCell = GetCellCoordinate(new Vector3(xEnd, 0, zEnd));

        for(var x = startCell.x; x <= endCell.x; x++) {
            for(var y = startCell.y; y <= endCell.y; y++) {
                GridContent[x, y] = type;
            }
        }
    }

    public Vector2Int GetCellCoordinate(Vector3 position) {
        var xStart = transform.position.x - Collider.bounds.extents.x;
        var zStart = transform.position.z - Collider.bounds.extents.z;

        var x = (int) (position.x - xStart);
        var z = (int) (position.z - zStart);
        
        return new Vector2Int(x, z);
    }

    public Vector2 GetCoordinateFromCell(Vector2Int cell) {
        var xStart = transform.position.x - Collider.bounds.extents.x;
        var zStart = transform.position.z - Collider.bounds.extents.z;

        var xInterval = Collider.bounds.size.x / Rows;
        var zInterval = Collider.bounds.size.z / Cols;

        return new Vector2(xStart + cell.x * xInterval, zStart + cell.y * zInterval);
    }

    private void OnDrawGizmos() {
        var Rows = (int) Collider.bounds.size.x;
        var Cols = (int) Collider.bounds.size.z;

        Gizmos.color = Color.red;

        var xStart = transform.position.x - Collider.bounds.extents.x;
        var xEnd = transform.position.x + Collider.bounds.extents.x;

        var zStart = transform.position.z - Collider.bounds.extents.z;
        var zEnd = transform.position.z + Collider.bounds.extents.z;

        var xInterval = Collider.bounds.size.x / Rows;
        var zInterval = Collider.bounds.size.z / Cols;

        var y = transform.position.y + 0.01f;

        for(int i = 0; i < Rows + 1; i++) {
            Gizmos.DrawLine(new Vector3(xStart + i * xInterval, y, zStart), new Vector3(xStart + i * xInterval, y, zEnd));
        }

        for(int i = 0; i < Cols + 1; i++) {
            Gizmos.DrawLine(new Vector3(xStart, y, zStart + i * zInterval), new Vector3(xEnd, y, zStart + i * zInterval));
        }
    }
}
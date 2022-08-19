using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

/// <summary>
/// using tile grid to find shortest way from A to B. 
/// </summary>
public class RoadSearcher : MonoBehaviour
{
    [SerializeField] private Grid _roadGrid;
    [SerializeField] private Tilemap _roadMap;
    [SerializeField] private RuleTile _roadRule;
    private static int MAX_STEPS_AMOUNT = 100;
    private static Vector3Int UP = new Vector3Int(0, 1, 0);
    private static Vector3Int RIGHT = new Vector3Int(1, 0, 0);
    private static Vector3Int DOWN = new Vector3Int(0, -1, 0);
    private static Vector3Int LEFT = new Vector3Int(-1, 0, 0);
    private static Vector3Int[] DIRECTIONS = { UP, RIGHT, DOWN, LEFT };
    private static Vector3 TILE_OFFSET;
    private List<Node2D> _nodeHeap = new List<Node2D>();
    private List<Node2D> _nodeEdge = new List<Node2D>();

    private void Start()
    {
        TILE_OFFSET = new Vector3(.5f, .5f, 0);
        PregenerateAllNodes();
    }

    /// <summary>
    /// Find and generate shortest road from A to B using only tilemap and ruletile. WaterFlow algorythm
    /// </summary>
    /// <param name="to"></param>
    /// <param name="from"></param>
    /// <returns></returns>
    public Road FindRoadFromTo(Vector3 from, Vector3 to)
    {
        Vector3Int cellFrom = _roadMap.WorldToCell(from);
        Vector3Int cellTo = _roadMap.WorldToCell(to);

        Node2D fromNode = _nodeHeap.Find(node => node.CellPos.x == cellFrom.x && node.CellPos.y == cellFrom.y);
        if (fromNode == null)
        {
            Debug.LogWarning("Start position is not visible");
            Debug.Break();
        }

        Node2D toNode = _nodeHeap.Find(node => node.CellPos.x == cellTo.x && node.CellPos.y == cellTo.y);
        if (toNode == null)
        {
            Debug.LogWarning("Destination is not visible");
            Debug.Break();
        }
        toNode.Status = Node2D.Node2DStatus.EDGE;

        int i = 0;
        while (fromNode.Status != Node2D.Node2DStatus.EDGE && i++ < MAX_STEPS_AMOUNT)
        {
            _nodeEdge = _nodeHeap.FindAll(node => node.Status == Node2D.Node2DStatus.EDGE);
            WaveDoStep(_nodeEdge);
        }

        return GenerateRoadFromNodes(fromNode);
    }

    private void WaveDoStep(List<Node2D> waveEdge)
    {
        foreach (Node2D node in waveEdge)
        {
            NodeDoStep(node);
        }
    }

    private void NodeDoStep(Node2D growingNode)
    {
        growingNode.Status = Node2D.Node2DStatus.VISITED;
        foreach (Vector3Int dir in DIRECTIONS)
        {
            Node2D node = _nodeHeap.Find(node => node.CellPos == growingNode.CellPos + dir);
            if (node != null && node.Status == Node2D.Node2DStatus.NOT_VISITED)
            {
                node.Status = Node2D.Node2DStatus.EDGE;
                node.PrevNode = growingNode;
            }
        }
    }

    private void PregenerateAllNodes()
    {
        BoundsInt bounds = _roadMap.cellBounds;
        for (int y = bounds.yMin; y < bounds.yMax; y++)
        {
            for (int x = bounds.xMin; x < bounds.xMax; x++)
            {
                Vector3Int cellPos = new Vector3Int(x, y, bounds.zMin);
                if (_roadMap.GetTile<RuleTile>(cellPos) != null && _roadMap.GetTile<RuleTile>(cellPos) == _roadRule)
                {
                    Node2D node = new Node2D();

                    node.CellPos = cellPos;
                    node.WorldPos = _roadMap.CellToWorld(node.CellPos) + TILE_OFFSET;
                    node.Status = Node2D.Node2DStatus.NOT_VISITED;
                    node.PrevNode = null;

                    _nodeHeap.Add(node);
                }
            }
        }
    }

    private Road GenerateRoadFromNodes(Node2D fromNode)
    {
        Road road = new Road();
        Node2D tmpNode = fromNode;

        while (tmpNode != null)
        {
            road.WaypointsCell.Enqueue(tmpNode.CellPos);
            road.WaypointsWorld.Enqueue(tmpNode.WorldPos);
            tmpNode = tmpNode.PrevNode;
        }

        return road;
    }
}

public class Node2D
{
    public Vector3Int CellPos;
    public Vector3 WorldPos;
    public Node2D PrevNode;
    public Node2DStatus Status;

    public enum Node2DStatus
    {
        NOT_VISITED,
        EDGE,
        VISITED
    }
}

/// <summary>
/// Better storage for local and world positions of waypoints
/// </summary>
public class Road
{
    public Queue<Vector3Int> WaypointsCell = new Queue<Vector3Int>();
    public Queue<Vector3> WaypointsWorld = new Queue<Vector3>();
    public Vector3Int FirstCell { get => WaypointsCell.Peek(); }
    public Vector3 FirstWorld { get => WaypointsWorld.Peek(); }
    public Vector3Int LastCell { get => Last(WaypointsCell); }
    public Vector3 LastWorld { get => Last(WaypointsCell); }

    private T Last<T>(Queue<T> queue) where T : IEquatable<T>, IFormattable
    {
        T last;
        Queue<T> tmp = new Queue<T>(queue);
        while (tmp.TryDequeue(out last)); //outlast hehe
        return last;
    }

    public Road() {}

    public Road(Road road)
    {
        WaypointsCell = new Queue<Vector3Int>(road.WaypointsCell);
        WaypointsWorld = new Queue<Vector3>(road.WaypointsWorld);
    }
}
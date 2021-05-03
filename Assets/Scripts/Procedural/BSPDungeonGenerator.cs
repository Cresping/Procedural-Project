using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BSPDungeonGenerator : AbstractDoungeonGenerator
{
    private const int MAXIMUM_ATTEMPTS_RANDOM = 10;
    [SerializeField] private PlayerVariableSO playerVariableSO;
    [SerializeField] private List<RoomVariableSO> roomVariableSOs;
    [SerializeField] private Transform enemiesParent;
    [SerializeField] private Pathfind.GridPathfind gridPathfind;

    [SerializeField] private int minRoomWidth = 4, minRoomHeight = 4;
    [SerializeField] private int dungeonWidth = 20, dungeonHeight = 20;
    [SerializeField] [Range(0, 10)] private int offset = 1;

    HashSet<Vector2Int> _floor;
    HashSet<Vector2Int> _corridors;
    Vector2Int _start;
    Vector2Int _end;

    protected override void RunProceduralGeneration()
    {
        int childs = enemiesParent.childCount;
        for (int i = childs - 1; i >= 0; i--)
        {
            GameObject.DestroyImmediate(enemiesParent.GetChild(i).gameObject);
        }
        CreateRooms();
    }
    private void CreateRooms()
    {
        var roomList = ProceduralGenerationAlgorithms.BinarySpacePartitioning
            (new BoundsInt((Vector3Int)startPosition, new Vector3Int(dungeonWidth, dungeonHeight, 0)), minRoomWidth, minRoomHeight);
        gridPathfind.CreateGrid(dungeonWidth, dungeonHeight);
        _floor = new HashSet<Vector2Int>();
        _corridors = new HashSet<Vector2Int>();
        foreach (var room in roomList)
        {
            CreateSimpleRoom(room);
        }
        List<Vector2Int> roomCenter = new List<Vector2Int>();
        foreach (var room in roomList)
        {
            roomCenter.Add((Vector2Int)Vector3Int.RoundToInt(room.center));
        }
        CreateStartAndEnd(roomCenter);
        CreateSpecialRoom(roomList);
        playerVariableSO.PlayerStartPosition = _start;
        ConnectRooms(roomCenter);
        _floor.UnionWith(_corridors);
        tileMapGenerator.PaintFloorTiles(_floor);
        tileMapGenerator.PaintStartTile(_start);
        tileMapGenerator.PaintEndTile(_end);
        WallGenerator.CreateSimpleWalls(_floor, tileMapGenerator);
    }
    private void CreateStartAndEnd(List<Vector2Int> roomCenter)
    {
        float maxDistance = float.MinValue;
        _start = new Vector2Int();
        _end = new Vector2Int();
        foreach (var start in roomCenter)
        {
            float distance;
            Vector2Int end;
            end = FindFurtherPointTo(start, roomCenter, out distance);
            if (distance > maxDistance)
            {
                _start = start;
                _end = end;
                maxDistance = distance;
            }
        }
    }
    private void ConnectRooms(List<Vector2Int> roomCenter)
    {
        var currentRoomCenter = roomCenter[UnityEngine.Random.Range(0, roomCenter.Count)];
        roomCenter.Remove(currentRoomCenter);
        while (roomCenter.Count > 0)
        {
            Vector2Int closet = FindClosestPointTo(currentRoomCenter, roomCenter);
            roomCenter.Remove(closet);
            HashSet<Vector2Int> newCorridor = CreateCorridor(currentRoomCenter, closet);
            currentRoomCenter = closet;
            _corridors.UnionWith(newCorridor);
        }
    }

    private HashSet<Vector2Int> CreateCorridor(Vector2Int currentRoomCenter, Vector2Int destination)
    {
        HashSet<Vector2Int> corridor = new HashSet<Vector2Int>();
        var position = currentRoomCenter;
        corridor.Add(position);
        while (position.y != destination.y)
        {
            if (destination.y > position.y)
            {
                position += Vector2Int.up;
            }
            else if (destination.y < position.y)
            {
                position += Vector2Int.down;
            }
            corridor.Add(position);
            gridPathfind.ChangeNode(position.x, position.y, true);
        }
        while (position.x != destination.x)
        {
            if (destination.x > position.x)
            {
                position += Vector2Int.right;
            }
            else if (destination.x < position.x)
            {
                position += Vector2Int.left;
            }
            corridor.Add(position);
            gridPathfind.ChangeNode(position.x, position.y, true);
        }
        return corridor;
    }

    private Vector2Int FindClosestPointTo(Vector2Int currentRoomCenter, List<Vector2Int> roomCenter)
    {
        Vector2Int closet = Vector2Int.zero;
        float distance = float.MaxValue;
        foreach (var position in roomCenter)
        {
            float currentDistance = Vector2.Distance(position, currentRoomCenter);
            if (currentDistance < distance)
            {
                distance = currentDistance;
                closet = position;
            }
        }
        return closet;
    }

    private Vector2Int FindFurtherPointTo(Vector2Int currentRoomCenter, List<Vector2Int> roomCenter, out float distance)
    {
        Vector2Int far = Vector2Int.zero;
        distance = float.MinValue;
        foreach (var position in roomCenter)
        {
            float currentDistance = Vector2.Distance(position, currentRoomCenter);
            if (currentDistance > distance)
            {
                distance = currentDistance;
                far = position;
            }
        }
        return far;
    }

    private void CreateSpecialRoom(List<BoundsInt> roomList)
    {
        List<BoundsInt> roomListAux = new List<BoundsInt>();
        roomListAux = roomList;
        if (roomVariableSOs.Count <= 0) return;
        foreach (var roomVariable in roomVariableSOs)
        {
            foreach (BoundsInt room in roomListAux)
            {
                if ((Vector2Int)Vector3Int.RoundToInt(room.center) != _start && (Vector2Int)Vector3Int.RoundToInt(room.center) != _end)
                {
                    Dictionary<Vector2Int, Vector2Int> currentPositions = new Dictionary<Vector2Int, Vector2Int>();
                    List<Vector2Int> enemiesPosition = new List<Vector2Int>();
                    for (int i = roomVariable.NumberOfEnemies() - 1; i >= 0; i--)
                    {
                        int cont = 0;
                        while (cont < MAXIMUM_ATTEMPTS_RANDOM)
                        {
                            Vector2Int aux;
                            aux = new Vector2Int(UnityEngine.Random.Range(room.xMin + 1, room.xMax - 1), UnityEngine.Random.Range(room.yMin + 1, room.yMax - 1));
                            if (!currentPositions.ContainsKey(aux))
                            {
                                currentPositions.Add(aux, aux);
                                enemiesPosition.Add(aux);
                                break;
                            }
                            cont++;
                        }
                    }
                    roomVariable.InstantiateAllEnemies(enemiesParent);
                    roomVariable.EnableAllEnemies(enemiesPosition);
                    roomListAux.Remove(room);
                    break;
                }
            }
        }
    }
    private void CreateSimpleRoom(BoundsInt room)
    {
        for (int col = offset; col < room.size.x - offset; col++)
        {
            for (int row = offset; row < room.size.y - offset; row++)
            {
                Vector2Int position = (Vector2Int)room.min + new Vector2Int(col, row);
                _floor.Add(position);
                gridPathfind.ChangeNode(position.x, position.y, true);
            }
        }
    }
}

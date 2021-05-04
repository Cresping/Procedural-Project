using HeroesGames.ProjectProcedural.SO;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HeroesGames.ProjectProcedural.Procedural
{
    /// <summary>
    /// Clase encargada de generar una mazmorra con el algoritmo BSP
    /// </summary>
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

        /// <summary>
        /// Módulo encargada de crear la mazmorra
        /// </summary>
        protected override void RunProceduralGeneration()
        {
            int childs = enemiesParent.childCount;
            for (int i = childs - 1; i >= 0; i--)
            {
                GameObject.DestroyImmediate(enemiesParent.GetChild(i).gameObject);
            }
            CreateRooms();
        }

        /// <summary>
        /// Módulo encargado de crear las habitaciones de la mazmorra
        /// </summary>
        private void CreateRooms()
        {
            var roomList = ProceduralGenerationAlgorithms.BinarySpacePartitioning
                (new BoundsInt((Vector3Int)startPosition, new Vector3Int(dungeonWidth, dungeonHeight, 0)), minRoomWidth, minRoomHeight);
            //Crea el Grid del Pathfind
            gridPathfind.CreateGrid(dungeonWidth, dungeonHeight);
            _floor = new HashSet<Vector2Int>();
            _corridors = new HashSet<Vector2Int>();
            //En primer lugar crea las habitaciones
            foreach (var room in roomList)
            {
                CreateSimpleRoom(room);
            }
            List<Vector2Int> roomCenter = new List<Vector2Int>();
            foreach (var room in roomList)
            {
                roomCenter.Add((Vector2Int)Vector3Int.RoundToInt(room.center));
            }
            //Luego crea las habitaciones especiales
            CreateStartAndEnd(roomCenter);
            CreateSpecialRoom(roomList);
            //Conecta todas las habitaciones
            ConnectRooms(roomCenter);
            _floor.UnionWith(_corridors);
            playerVariableSO.PlayerStartPosition = _start;
            //Pinta la mazmorra en el mapa
            tileMapGenerator.PaintFloorTiles(_floor);
            tileMapGenerator.PaintStartTile(_start);
            tileMapGenerator.PaintEndTile(_end);
            WallGenerator.CreateSimpleWalls(_floor, tileMapGenerator);
        }

        /// <summary>
        /// Crea las habitaciones de comienzo y final de la mazmorra dadas las posiciones centrales de todas las habitaciones
        /// </summary>
        /// <param name="roomCenter">Posiciones centrales de todas las habitaciones</param>
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

        /// <summary>
        /// Conecta todas las habitaciones con pasillos
        /// </summary>
        /// <param name="roomCenter">Posiciones centrales de todas las habitaciones</param>
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

        /// <summary>
        /// Conecta dos habitaciones con un pasillo
        /// </summary>
        /// <param name="currentRoomCenter"> Centro de la habitación desde donde se creará el pasillo</param>
        /// <param name="destination">Fin del pasillo</param>
        /// <returns>Devuelve las posiciones del pasillo</returns>
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

        /// <summary>
        /// Devuelve la habitación más cercana a determinado punto
        /// </summary>
        /// <param name="currentRoomCenter">Punto</param>
        /// <param name="roomCenter">Lista  de posiciones de centrales de todas las habitaciones</param>
        /// <returns>Habitación más cercana</returns>
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

        /// <summary>
        /// Devuelve la habitación más lejana a determinado punto
        /// </summary>
        /// <param name="currentRoomCenter">Punto</param>
        /// <param name="roomCenter">Lista  de posiciones de centrales de todas las habitaciones</param>
        /// <returns>Habitación más lejana</returns>
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

        /// <summary>
        /// Módulo encargado de crear el resto de habitaciones especiales
        /// </summary>
        /// <param name="roomList">Lista de habitaciones</param>
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

        /// <summary>
        /// Modulo encargado de crear una habitación simple
        /// </summary>
        /// <param name="room">Habitación</param>
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

}

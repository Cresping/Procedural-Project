using HeroesGames.ProjectProcedural.Player;
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
        //[SerializeField] private List<RoomVariableSO> roomVariableSOs;
        [SerializeField] private BSPDungeonVariableSO bSPDungeonVariableSO;
        [SerializeField] private GameObject mazeEntrancePrefab;
        [SerializeField] private Transform enemiesParent;
        [SerializeField] private Transform chestsParent;
        [SerializeField] private Transform mazeParent;
        [SerializeField] private Pathfind.GridPathfind gridPathfind;

        //[SerializeField] private int minRoomWidth = 4, minRoomHeight = 4;
        //[SerializeField] private int dungeonWidth = 20, dungeonHeight = 20;
        //[SerializeField] [Range(0, 10)] private int offset = 1;

        HashSet<Vector2Int> _floor;
        HashSet<Vector2Int> _corridors;
        Vector2Int _start;
        Vector2Int _end;

        /// <summary>
        /// M�dulo encargada de crear la mazmorra
        /// </summary>
        protected override void RunProceduralGeneration()
        {
            bSPDungeonVariableSO.CalculateDifficulty();
            int childsEnemies = enemiesParent.childCount;
            int childsChests = chestsParent.childCount;
            int childsMaze = mazeParent.childCount;
            for (int i = childsEnemies - 1; i >= 0; i--)
            {
                GameObject.DestroyImmediate(enemiesParent.GetChild(i).gameObject);
            }
            for (int i = childsChests - 1; i >= 0; i--)
            {
                GameObject.DestroyImmediate(chestsParent.GetChild(i).gameObject);
            }
            for (int i = childsMaze - 1; i >= 0; i--)
            {
                GameObject.DestroyImmediate(mazeParent.GetChild(i).gameObject);
            }
            CreateRooms();
            FindObjectOfType<PlayerController>().gameObject.transform.position = (Vector3Int)_start;
        }

        /// <summary>
        /// M�dulo encargado de crear las habitaciones de la mazmorra
        /// </summary>
        private void CreateRooms()
        {
            var roomList = ProceduralGenerationAlgorithms.BinarySpacePartitioning
                (new BoundsInt((Vector3Int)startPosition, new Vector3Int(bSPDungeonVariableSO.DungeonWidth, bSPDungeonVariableSO.DungeonHeight, 0)), bSPDungeonVariableSO.MinRoomWidth, bSPDungeonVariableSO.MinRoomHeight);
            //Crea el Grid del Pathfind
            gridPathfind.CreateGrid(bSPDungeonVariableSO.DungeonWidth, bSPDungeonVariableSO.DungeonHeight);
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
            //Conecta todas las habitaciones
            ConnectRooms(roomCenter);
            _floor.UnionWith(_corridors);
            playerVariableSO.InstancePlayer(_start);
            //Pinta la mazmorra en el mapa
            tileMapGenerator.PaintFloorTiles(_floor);
            tileMapGenerator.PaintStartTile(_start);
            tileMapGenerator.PaintEndTile(_end);
            WallGenerator.CreateSimpleWalls(_floor, tileMapGenerator);
            CreateSpecialRoom(roomList);
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
        /// <param name="currentRoomCenter"> Centro de la habitaci�n desde donde se crear� el pasillo</param>
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
        /// Devuelve la habitaci�n m�s cercana a determinado punto
        /// </summary>
        /// <param name="currentRoomCenter">Punto</param>
        /// <param name="roomCenter">Lista  de posiciones de centrales de todas las habitaciones</param>
        /// <returns>Habitaci�n m�s cercana</returns>
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
        /// Devuelve la habitaci�n m�s lejana a determinado punto
        /// </summary>
        /// <param name="currentRoomCenter">Punto</param>
        /// <param name="roomCenter">Lista  de posiciones de centrales de todas las habitaciones</param>
        /// <returns>Habitaci�n m�s lejana</returns>
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
        /// M�dulo encargado de crear el resto de habitaciones especiales
        /// </summary>
        /// <param name="roomList">Lista de habitaciones</param>
        private void CreateSpecialRoom(List<BoundsInt> roomList)
        {
            List<BoundsInt> roomListAux = new List<BoundsInt>();
            roomListAux = roomList;
            if (bSPDungeonVariableSO.SelectedRooms.Count <= 0) return;

            foreach (var roomVariable in bSPDungeonVariableSO.SelectedRooms)
            {
                if (roomListAux.Count <= 2) return;
                foreach (BoundsInt room in roomListAux)
                {
                    if ((Vector2Int)Vector3Int.RoundToInt(room.center) != _start && (Vector2Int)Vector3Int.RoundToInt(room.center) != _end)
                    {
                        roomVariable.PrepareRoom(enemiesParent, chestsParent, mazeParent, room, gridPathfind, mazeEntrancePrefab);
                        roomListAux.Remove(room);
                        break;
                    }
                }
            }
        }

        private BoundsInt Deletethis(RoomVariableSO roomVariable, BoundsInt room)
        {
            Dictionary<Vector2Int, Vector2Int> currentEnemiesPositions = new Dictionary<Vector2Int, Vector2Int>();
            List<Vector2Int> enemiesPosition = new List<Vector2Int>();
            for (int i = roomVariable.NumberOfEnemies() - 1; i >= 0; i--)
            {
                int cont = 0;
                while (cont < MAXIMUM_ATTEMPTS_RANDOM)
                {
                    Vector2Int aux;
                    aux = new Vector2Int(UnityEngine.Random.Range(room.xMin + 1, room.xMax - 1), UnityEngine.Random.Range(room.yMin + 1, room.yMax - 1));
                    if (!currentEnemiesPositions.ContainsKey(aux) && gridPathfind.IsWalkeable(aux.x, aux.y))
                    {
                        currentEnemiesPositions.Add(aux, aux);
                        enemiesPosition.Add(aux);
                        break;
                    }
                    cont++;
                }
            }
            roomVariable.InstantiateAllEnemies(enemiesParent);
            roomVariable.EnableAllEnemies(enemiesPosition);
            return room;
        }

        /// <summary>
        /// Modulo encargado de crear una habitaci�n simple
        /// </summary>
        /// <param name="room">Habitaci�n</param>
        private void CreateSimpleRoom(BoundsInt room)
        {
            for (int col = bSPDungeonVariableSO.Offset; col < room.size.x - bSPDungeonVariableSO.Offset; col++)
            {
                for (int row = bSPDungeonVariableSO.Offset; row < room.size.y - bSPDungeonVariableSO.Offset; row++)
                {
                    Vector2Int position = (Vector2Int)room.min + new Vector2Int(col, row);
                    _floor.Add(position);
                    gridPathfind.ChangeNode(position.x, position.y, true);
                }
            }
        }
    }

}

using HeroesGames.ProjectProcedural.Pathfind;
using HeroesGames.ProjectProcedural.Utils;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HeroesGames.ProjectProcedural.SO
{
    /// <summary>
    ///  Clase encargada de controlar los 'ScriptableObject' de las habitaciones del mapa
    /// </summary>
    [CreateAssetMenu(fileName = "NewRoomVariable", menuName = "Scriptables/Room/RoomVariable")]
    public class RoomVariableSO : ScriptableObject, ISerializationCallbackReceiver
    {
        private const int MAXIMUM_ATTEMPTS_RANDOM = 10;
        [SerializeField] EnumTypes.RoomType roomType;
        [SerializeField] List<EnemyVariableSO> enemyTypeList;
        [SerializeField] List<ChestVariableSO> chestTypeList;
        [SerializeField] private bool hasEntranceMaze = false;

        private List<GameObject> enemyList;
        private List<GameObject> chestList;
        public EnumTypes.RoomType RoomType { get => roomType; private set => roomType = value; }


        public void PrepareRoom(Transform parentEnemies, Transform parentChests, Transform parentMaze, BoundsInt room, GridPathfind gridPathfing, GameObject entranceMazePrefab)
        {
            int cont = 0;
            Dictionary<Vector2Int, Vector2Int> currentOccupiedPositions = new Dictionary<Vector2Int, Vector2Int>();
            List<Vector2Int> chestsPosition = new List<Vector2Int>();
            List<Vector2Int> enemiesPosition = new List<Vector2Int>();
            if (hasEntranceMaze && entranceMazePrefab)
            {
                cont = 0;
                while (cont < MAXIMUM_ATTEMPTS_RANDOM)
                {
                    Vector2Int aux;
                    aux = new Vector2Int(UnityEngine.Random.Range(room.xMin + 2, room.xMax - 3), UnityEngine.Random.Range(room.yMin + 2, room.yMax - 3));
                    if (!currentOccupiedPositions.ContainsKey(aux) && gridPathfing.IsWalkeable(aux.x, aux.y))
                    {
                        GameObject maze = Instantiate(entranceMazePrefab, parentMaze);
                        currentOccupiedPositions.Add(aux, aux);
                        gridPathfing.ChangeNode(aux.x, aux.y, true);
                        maze.transform.position = (Vector3Int) aux;
                        break;
                    }
                }
            }
            for (int i = chestTypeList.Count - 1; i >= 0; i--)
            {
                cont = 0;
                while (cont < MAXIMUM_ATTEMPTS_RANDOM)
                {
                    Vector2Int aux;
                    aux = new Vector2Int(UnityEngine.Random.Range(room.xMin + 2, room.xMax - 3), UnityEngine.Random.Range(room.yMin + 2, room.yMax - 3));
                    if (!currentOccupiedPositions.ContainsKey(aux) && gridPathfing.IsWalkeable(aux.x, aux.y))
                    {
                        currentOccupiedPositions.Add(aux, aux);
                        chestsPosition.Add(aux);
                        gridPathfing.ChangeNode(aux.x, aux.y, false);
                        break;
                    }
                    cont++;
                }
            }
            for (int i = enemyTypeList.Count - 1; i >= 0; i--)
            {
                cont = 0;
                while (cont < MAXIMUM_ATTEMPTS_RANDOM)
                {
                    Vector2Int aux;
                    aux = new Vector2Int(UnityEngine.Random.Range(room.xMin + 1, room.xMax - 2), UnityEngine.Random.Range(room.yMin + 1, room.yMax - 2));
                    if (!currentOccupiedPositions.ContainsKey(aux) && gridPathfing.IsWalkeable(aux.x, aux.y))
                    {
                        currentOccupiedPositions.Add(aux, aux);
                        enemiesPosition.Add(aux);
                        gridPathfing.ChangeNode(aux.x, aux.y, false);
                        break;
                    }
                    cont++;
                }
            }
            InstantiateAllEnemies(parentEnemies);
            InstantiateAllChests(parentChests);
            EnableAllEnemies(enemiesPosition);
            EnableAllChests(chestsPosition);
        }
        public void InstantiateAllChests(Transform parent)
        {
            chestList = new List<GameObject>();
            int index = 0;
            foreach (var chest in chestTypeList)
            {
                chestList.Add(Instantiate(chest.ChestPrefab, parent));
                chestList[index].SetActive(false);
                index++;
            }
        }
        public void EnableAllChests(List<Vector2Int> positions)
        {
            int index = 0;
            try
            {
                foreach (var chest in chestList)
                {
                    chest.transform.position = (Vector3Int)positions[index];
                    chest.SetActive(true);
                    index++;
                }
            }
            catch (ArgumentOutOfRangeException)
            {
                Debug.LogError("Fallo al colocar los cofres, el indice " + index + " se ha salido del total de posiciones " + positions.Count);
            }

        }
        /// <summary>
        /// M�dulo encargadod e instanciar todos los enemigos de la habitaci�n en un padre concreto
        /// </summary>
        /// <param name="parent"> Donde se van a instanciar los enemigos en la escena</param>
        public void InstantiateAllEnemies(Transform parent)
        {
            enemyList = new List<GameObject>();
            int index = 0;
            foreach (var enemy in enemyTypeList)
            {
                enemyList.Add(Instantiate(enemy.EnemyPrefab, parent));
                enemyList[index].SetActive(false);
                index++;
            }
        }
        /// <summary>
        /// Activa todos los enemigos de la habitaci�n en las posiciones dadas
        /// </summary>
        /// <param name="positions">Posiciones donde se activar�n los enemigos</param>
        public void EnableAllEnemies(List<Vector2Int> positions)
        {
            int index = 0;
            try
            {
                foreach (var enemy in enemyList)
                {
                    enemy.transform.position = (Vector3Int)positions[index];
                    enemy.SetActive(true);
                    index++;
                }
            }
            catch (ArgumentOutOfRangeException)
            {
                Debug.LogError("Fallo al colocar los enemigos, el indice " + index + " se ha salido del total de posiciones " + positions.Count);
            }
        }
        /// <summary>
        /// Desactiva todos los enemigos de la habitaci�n
        /// </summary>
        public void DisableAllEnemies()
        {
            foreach (var enemy in enemyList)
            {
                enemy.SetActive(false);
            }
        }
        /// <summary>
        /// Devuelve la cantidad total de enemigos que hay en la habitaci�n
        /// </summary>
        /// <returns> Cantidad de total de enemigos en la habitaci�on</returns>
        public int NumberOfEnemies()
        {
            return enemyTypeList.Count;
        }
        public void OnAfterDeserialize() { }

        public void OnBeforeSerialize() { }
    }
}


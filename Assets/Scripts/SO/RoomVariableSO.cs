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
        [SerializeField] EnumTypes.RoomType roomType;
        [SerializeField] List<EnemyVariableSO> enemyTypeList;

        private List<GameObject> enemyList;
        public EnumTypes.RoomType RoomType { get => roomType; private set => roomType = value; }

        /// <summary>
        /// Módulo encargadod e instanciar todos los enemigos de la habitación en un padre concreto
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
        /// Activa todos los enemigos de la habitación en las posiciones dadas
        /// </summary>
        /// <param name="positions">Posiciones donde se activarán los enemigos</param>
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
            catch (Exception) { }
        }
        /// <summary>
        /// Desactiva todos los enemigos de la habitación
        /// </summary>
        public void DisableAllEnemies()
        {
            foreach (var enemy in enemyList)
            {
                enemy.SetActive(false);
            }
        }
        /// <summary>
        /// Devuelve la cantidad total de enemigos que hay en la habitación
        /// </summary>
        /// <returns> Cantidad de total de enemigos en la habitaci´on</returns>
        public int NumberOfEnemies()
        {
            return enemyTypeList.Count;
        }
        public void OnAfterDeserialize() { }

        public void OnBeforeSerialize() { }
    }
}
 

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HeroesGames.ProjectProcedural.Procedural
{
    /// <summary>
    /// Clase abstracta encargada de agrupar los metodos comunes de generación de mazmorras
    /// </summary>
    public abstract class AbstractDoungeonGenerator : MonoBehaviour
    {
        [SerializeField] protected SimpleTileMapGenerator tileMapGenerator = null;
        [SerializeField] protected Vector2Int startPosition = Vector2Int.zero;

        private void Awake()
        {
            GenerateDungeon();
        }
        /// <summary>
        /// Genera la mazmorra
        /// </summary>
        public void GenerateDungeon()
        {
            tileMapGenerator.ClearAllTiles();
            RunProceduralGeneration();
        }
        /// <summary>
        /// Ejecuta el algoritmo procedural implementado
        /// </summary>
        protected abstract void RunProceduralGeneration();
    }
}


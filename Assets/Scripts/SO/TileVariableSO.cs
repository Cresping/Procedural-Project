using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace HeroesGames.ProjectProcedural.SO
{
    /// <summary>
    ///  Clase encargada de controlar los 'ScriptableObject' de las 'tiles' del mapa
    /// </summary>
    [CreateAssetMenu(fileName = "NewTileVariable", menuName = "Scriptables/Tiles/TileVariable")]
    public class TileVariableSO : ScriptableObject
    {
        [SerializeField] private TileBase mainTile;

        [SerializeField] private List<TileBase> commonTiles;

        [SerializeField] private List<TileBase> rareTiles;

        [SerializeField] private float probabilityRareTile = 0.2f;

        /// <summary>
        ///    Selecciona una 'tile' aleatoria del conjunto de 'tiles'
        /// </summary>
        public TileBase PickRandomTile()
        {
            if (commonTiles.Count <= 0)
            {
                return mainTile;
            }
            if (rareTiles.Count > 0)
            {
                float random = Random.Range(0.00f, 1.00f);
                if (random <= probabilityRareTile)
                {
                    return rareTiles[(Random.Range(0, rareTiles.Count))];
                }
            }
            return commonTiles[Random.Range(0, commonTiles.Count)];
        }
    }
}


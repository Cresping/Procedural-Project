using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using HeroesGames.ProjectProcedural.SO;
namespace HeroesGames.ProjectProcedural.Procedural
{
    /// <summary>
    /// Clase encargada de pintar los 'tiles' en el mapa del juego
    /// </summary>
    public class SimpleTileMapGenerator : MonoBehaviour
    {
        [SerializeField] private Tilemap floorTileMap;
        [SerializeField] private Tilemap wallTileMap;
        [SerializeField] private Tilemap startTileMap;
        [SerializeField] private Tilemap endTileMap;
        [SerializeField] private TileVariableSO floorTiles;
        [SerializeField] private TileVariableSO wallTiles;
        [SerializeField] private TileVariableSO startTiles;
        [SerializeField] private TileVariableSO endTiles;

        /// <summary>
        /// M�dulo encargado de pintar los 'tiles' dados en unas posiciones determinadas
        /// </summary>
        /// <param name="positions">Posiciones donde se pintar�n los 'tiles'</param>
        /// <param name="tilemap">Mapa donde se pintar�n los 'tiles'</param>
        /// <param name="tile">'Tile' seleccionado</param>
        private void PaintTiles(IEnumerable<Vector2Int> positions, Tilemap tilemap, TileBase tile)
        {
            foreach (var position in positions)
            {
                PaintSingleTile(tilemap, tile, position);
            }
        }

        /// <summary>
        /// M�dulo encargado de pintar un solo 'tile' en una posici�n determinada
        /// </summary>
        /// <param name="tilemap">Mapa donde se pintar� el 'tile'</param>
        /// <param name="tile">'Tile' seleccionado</param>
        /// <param name="position">Posici�n donde se pintar�</param>
        public void PaintSingleTile(Tilemap tilemap, TileBase tile, Vector2Int position)
        {
            var tilePosition = tilemap.WorldToCell((Vector3Int)position);
            tilemap.SetTile(tilePosition, tile);
        }

        /// <summary>
        /// M�dulo encargado de pintar los 'tiles' de los muros dadas unas posiciones
        /// </summary>
        /// <param name="wallPositions">Posiciones de los muros</param>
        public void PaintWallTiles(HashSet<Vector2Int> wallPositions)
        {
            foreach (var position in wallPositions)
            {
                PaintSingleTile(wallTileMap, wallTiles.PickRandomTile(), position);
            }
        }
        
        public void PaintWallTile(Vector2Int position)
        {
            PaintSingleTile(wallTileMap, wallTiles.PickRandomTile(), position);
        }

        /// <summary>
        /// M�dulo encargado de pintar los 'tiles' del suelo dadas unas posiciones
        /// </summary>
        /// <param name="floorPositions">Posiciones del suelo</param>
        public void PaintFloorTiles(HashSet<Vector2Int> floorPositions)
        {
            foreach (var position in floorPositions)
            {
                PaintSingleTile(floorTileMap, floorTiles.PickRandomTile(), position);
            }
        }
        
        public void PaintFloorTile(Vector2Int position)
        {
            PaintSingleTile(floorTileMap, floorTiles.PickRandomTile(), position);
        }

        /// <summary>
        /// M�dulo encargado de pintar el 'tile' del comienzo de la mazmorra
        /// </summary>
        /// <param name="position">Posicion del 'tile'</param>
        public void PaintStartTile(Vector2Int position)
        {
            PaintSingleTile(startTileMap, startTiles.PickRandomTile(), position);
        }
        /// <summary>
        /// M�dulo encargado de pintar el 'tile' del fin de la mazmorra
        /// </summary>
        /// <param name="position">Posicion del 'tile'</param>
        public void PaintEndTile(Vector2Int position)
        {
            PaintSingleTile(endTileMap, endTiles.PickRandomTile(), position);
        }

        /// <summary>
        /// Limpia todos los mapas
        /// </summary>
        public void ClearAllTiles()
        {
            floorTileMap.ClearAllTiles();
            wallTileMap.ClearAllTiles();
            startTileMap.ClearAllTiles();
            endTileMap.ClearAllTiles();
        }
    }
}
  

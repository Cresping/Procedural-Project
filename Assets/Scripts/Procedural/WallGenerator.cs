using HeroesGames.ProjectProcedural.Utils;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HeroesGames.ProjectProcedural.Procedural
{
    /// <summary>
    /// Clase encargada de colocar los muros en el mapa de la mazmorra
    /// </summary>
    public static class WallGenerator
    {
        /// <summary>
        /// Módulo encargado de crear los muros dadas las posiciones del suelo y un mapa
        /// </summary>
        /// <param name="floorPositions">Posiciones del suelo</param>
        /// <param name="tilemapVisualizer">Mapa donde se pintarán los muros</param>
        public static void CreateSimpleWalls(HashSet<Vector2Int> floorPositions, SimpleTileMapGenerator tilemapVisualizer)
        {
            var basicWallPositions = FindWallsInDirection(floorPositions, Direction2D.cardinalDirectionList);
            var cornerWallPositions = FindWallsInDirection(floorPositions, Direction2D.diagonalDirectionList);
            tilemapVisualizer.PaintWallTiles(basicWallPositions);
            tilemapVisualizer.PaintWallTiles(cornerWallPositions);
        }
        /// <summary>
        /// Módulo encargado de devolver las posiciones de los muros en el mapa
        /// </summary>
        /// <param name="floorPositions">Posiciones del suelo</param>
        /// <param name="directionList">Direcciones donde se pueden encontrar dichos muros</param>
        /// <returns></returns>
        private static HashSet<Vector2Int> FindWallsInDirection(HashSet<Vector2Int> floorPositions, List<Vector2Int> directionList)
        {
            HashSet<Vector2Int> wallPositions = new HashSet<Vector2Int>();
            foreach (var position in floorPositions)
            {
                foreach (var direction in directionList)
                {
                    var neighbourPosition = position + direction;
                    if (floorPositions.Contains(neighbourPosition) == false)
                    {
                        wallPositions.Add(neighbourPosition);
                    }
                }
            }
            return wallPositions;
        }
    }
}

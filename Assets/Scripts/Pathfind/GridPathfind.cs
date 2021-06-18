using UnityEngine;
using System.Collections.Generic;
using System;

namespace HeroesGames.ProjectProcedural.Pathfind
{
    /// <summary>
    /// Clase encargada de almacenar los datos de todos los nodos del Grid
    /// </summary>
    public class GridPathfind : MonoBehaviour
    {
        private Node[,] _nodes;
        private int _gridSizeX, _gridSizeY;

        /// <summary>
        /// Crea un grid de determinada altura y anchura
        /// </summary>
        /// <param name="width">Altura</param>
        /// <param name="height">Anchura</param>
        public void CreateGrid(int width, int height)
        {
            _gridSizeX = width;
            _gridSizeY = height;
            _nodes = new Node[width, height];
            for (int x = 0; x < width; x++)
            {
                for (int y = 0; y < height; y++)
                {
                    _nodes[x, y] = new Node(0, x, y);
                    _nodes[x, y].Walkable = false;
                }
            }
        }
        /// <summary>
        /// Cambia si un nodo se puede caminar o no
        /// </summary>
        /// <param name="posx">Posici�n del nodo en X</param>
        /// <param name="posy">Posici�n del nodo en Y</param>
        /// <param name="walkeable">Si es caminable o no</param>
        /// <returns>True si existe, false si no</returns>
        public bool ChangeNode(int posx, int posy, bool walkeable)
        {
            try
            {
                _nodes[posx, posy].Walkable = walkeable;
                return true;
            }
            catch (ArgumentOutOfRangeException)
            {
                Debug.LogError("El nodo " + posx + " " + posy + " no existe");
                return false;
            }
        }

        /// <summary>
        /// Devuelve el nodo de la posici�n dada
        /// </summary>
        /// <param name="posx">Posici�n del nodo en X</param>
        /// <param name="posy">Posici�n del nodo en Y</param>
        /// <returns>Nodo</returns>
        public Node GetNode(int posx, int posy)
        {
            return _nodes[posx, posy];
        }

        /// <summary>
        /// Devuelve si el nodo de la posición dada es caminable
        /// </summary>
        /// <param name="posx">Posici�n del nodo en X</param>
        /// <param name="posy">Posici�n del nodo en Y</param>
        /// <returns>True si se puede caminar, false si no</returns>
        public bool IsWalkeable(int posx, int posy)
        {
            return GetNode(posx, posy).Walkable;
        }

        /// <summary>
        /// Devuelve la lista de vecinos aptos respecto a un nodo
        /// </summary>
        /// <param name="node">Nodo desde donde se calcular�n los vecinos</param>
        /// <param name="canMoveDiagonal">Si se puede mover en diagonal o no</param>
        /// <returns></returns>
        public List<Node> GetNeighbours(Node node, bool canMoveDiagonal)
        {
            List<Node> neighbours = new List<Node>();
            for (int x = -1; x <= 1; x++)
            {
                for (int y = -1; y <= 1; y++)
                {
                    try
                    {

                        if (x == 0 && y == 0)
                        {
                            continue;
                        }
                        else if (x == -1 && y == -1)
                        {
                            if (!_nodes[node.GridX - 1, node.GridY].Walkable || !_nodes[node.GridX, node.GridY - 1].Walkable || !canMoveDiagonal)
                            {
                                continue;
                            }
                        }
                        else if (x == 1 && y == 1)
                        {
                            if (!_nodes[node.GridX + 1, node.GridY].Walkable || !_nodes[node.GridX, node.GridY + 1].Walkable || !canMoveDiagonal)
                            {
                                continue;
                            }
                        }
                        else if (x == 1 && y == -1)
                        {
                            if (!_nodes[node.GridX + 1, node.GridY].Walkable || !_nodes[node.GridX, node.GridY - 1].Walkable || !canMoveDiagonal)
                            {
                                continue;
                            }
                        }
                        else if (x == -1 && y == 1)
                        {
                            if (!_nodes[node.GridX - 1, node.GridY].Walkable || !_nodes[node.GridX, node.GridY + 1].Walkable || !canMoveDiagonal)
                            {
                                continue;
                            }
                        }
                        int checkX = node.GridX + x;
                        int checkY = node.GridY + y;
                        neighbours.Add(_nodes[checkX, checkY]);
                    }
                    catch (ArgumentOutOfRangeException)
                    {
                        continue;
                    }
                    catch (IndexOutOfRangeException)
                    {
                        continue;
                    }
                }
            }
            return neighbours;
        }

    }
}

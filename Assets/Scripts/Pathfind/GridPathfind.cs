using UnityEngine;
using System.Collections.Generic;
using System;

namespace Pathfind
{
    public class GridPathfind : MonoBehaviour
    {
        private Node[,] _nodes;
        private int _gridSizeX, _gridSizeY;

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


        public bool AddNode(int posx, int posy, bool walkeable)
        {
            if (posx < _gridSizeX && posy < _gridSizeY && posx >= 0 && posy >= 0)
            {
                _nodes[posx, posy].Walkable = walkeable;

                return true;
            }

            return false;
        }
        public bool ChangeNode(int posx, int posy, bool walkeable)
        {
            try
            {
                _nodes[posx, posy].Walkable = walkeable;
                return true;
            }
            catch (Exception)
            {
                Debug.Log("El nodo "+posx+" "+posy+" no existe");
                return false;
            }
        }

        public Node GetNode(int posx, int posy)
        {
            return _nodes[posx, posy];
        }

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

                }
            }

            return neighbours;
        }

    }
}

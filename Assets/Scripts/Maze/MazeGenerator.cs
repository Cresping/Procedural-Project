using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

namespace HeroesGames.ProjectProcedural.Procedural
{
    public class MazeGenerator : AbstractDoungeonGenerator
    {
        #region VARIABLES
    
        [SerializeField]
        private int
            width,
            height;

        [SerializeField]
        private GameObject
            wall,
            border,
            floor,
            exit;

        private List<Vector2Int> CurrentFrontiers = new List<Vector2Int>();
    
        private bool _endPlaced;
        private bool[,] _maze; // false = is corridor of the maze, true = is NOT corridor of the maze
    
        #endregion

        #region INIT MAZE

        private void Awake()
        {
            _maze = new bool[width, height];
            _endPlaced = false;
        }

        protected override void RunProceduralGeneration()
        {
            InitMaze();
            PrimsAlgorithm();
            DrawTilesInMaze();
            DrawBorders();
        }

        /*
         private void Start()
        {
            InitMaze();
            PrimsAlgorithm();
            DrawTilesInMaze();
            DrawBorders();
        }
        */
        
        // Initialize the maze array.
        private void InitMaze()
        {
            for (var column = 0; column < width; column++)
            for (var row = 0; row < height; row++)
                _maze[column, row] = true;
        }
        
        #endregion
        
        #region PRIM'S ALGORITHM

        // Use randomised Prims algorithm to generate corridors in the maze
        private void PrimsAlgorithm()
        {
            // 1. Choose random initial node, add to corridors and identify neighbours
            var randomX = Mathf.Clamp(Random.Range(0, width) * 2,0,width -1);
            var randomY = Mathf.Clamp(Random.Range(0, height) * 2,0, height -1);

            _maze[randomX, randomY] = false;
            AddNeighboursToFrontier(randomX, randomY);

            // 2. Repeat until there aren't frontiers in CurrentFrontiers list
            while (CurrentFrontiers.Count != 0)
            {
                // Select a random frontier from hashset
                var randomIndex = Random.Range(0, CurrentFrontiers.Count - 1);
                var chosenFrontier = CurrentFrontiers[randomIndex];

                // Choose a random no corridor neighbour and create floor to it
                var chosenNeighbour = ChooseRandomNeighbour(chosenFrontier.x, chosenFrontier.y);
            
                _maze[chosenFrontier.x, chosenFrontier.y] = false;
                _maze[chosenNeighbour.x, chosenNeighbour.y] = false;
                LinkFloorNodes(chosenFrontier.x, chosenFrontier.y, chosenNeighbour.x, chosenNeighbour.y);

                // Add new frontier tiles to list
                AddNeighboursToFrontier(chosenFrontier.x, chosenFrontier.y);

                // Remove the chosen frontier from our hashset
                CurrentFrontiers.Remove(chosenFrontier);
            }
        }
    
        // Add available neighbours to our frontier hashset
        private void AddNeighboursToFrontier(int column, int row)
        {
            if (column - 2 >= 0 && _maze[column - 2, row])
                CurrentFrontiers.Add(new Vector2Int(column - 2, row));

            if (column + 2 < width && _maze[column + 2, row])
                CurrentFrontiers.Add(new Vector2Int(column + 2, row));

            if (row - 2 >= 0 && _maze[column, row - 2]) 
                CurrentFrontiers.Add(new Vector2Int(column, row - 2));

            if (row + 2 < height && _maze[column, row + 2])
                CurrentFrontiers.Add(new Vector2Int(column, row + 2));
        }
    
        // Choose and return a random no corridor neighbour
        private Vector2Int ChooseRandomNeighbour(int column, int row)
        {
            var neighbours = new List<Vector2Int>();

            if (column - 2 >= 0 && !_maze[column - 2, row])
                neighbours.Add(new Vector2Int(column - 2, row));

            if (column + 2 < width && !_maze[column + 2, row])
                neighbours.Add(new Vector2Int(column + 2, row));

            if (row - 2 >= 0 && !_maze[column, row - 2])
                neighbours.Add(new Vector2Int(column, row - 2));

            if (row + 2 < height && !_maze[column, row + 2])
                neighbours.Add(new Vector2Int(column, row + 2));

            var randomNeighbour = Random.Range(0, neighbours.Count - 1);

            return neighbours[randomNeighbour];
        }

        // Remove the wall tile between two chosen in maze floors to complete the corridor
        private void LinkFloorNodes(int startColumn, int startRow, int endColumn, int endRow)
        {
            if (startColumn == endColumn)
            {
                var minRow = Mathf.Min(startRow, endRow);
                _maze[startColumn, minRow + 1] = false;
            }
            else if (startRow == endRow)
            {
                var minColumn = Mathf.Min(startColumn, endColumn);
                _maze[minColumn + 1, startRow] = false;
            }
        }

        #endregion

        #region DRAW THE MAZE

        // Draw Tiles in maze depends on _maze values
        private void DrawTilesInMaze()
        {
            for (var row = 0; row < width; row++)
            for (var column = 0; column < height; column++)
            {
                if (_maze[row, column])
                    Instantiate(wall, new Vector3(row, column, 0), Quaternion.identity);
                else
                {
                    if (!_endPlaced && row > width - 3 && column > height - 3)
                    {
                        Instantiate(exit, new Vector3(row, column,0), Quaternion.identity);
                        _endPlaced = true;
                    }
                    else
                        Instantiate(floor, new Vector3(row, column, 0), Quaternion.identity);
                }
            }
        }
    
        // Draw Borders
        private void DrawBorders()
        {
            for (var column = 0; column < width; column++)
            for (var row = 0; row < height; row++)
            {
                //Draw left and right boundaries
                if (column == 0)
                    Instantiate(border, new Vector3(column - 1, row, 0), Quaternion.identity);
                else if (column == width - 1)
                    Instantiate(border, new Vector3(width, row, 0), Quaternion.identity);
                
                //Draw up and down boundary
                if (row == 0)
                    Instantiate(border, new Vector3(column, row -1, 0), Quaternion.identity);
                else if (row == height - 1)
                    Instantiate(border, new Vector3(column, height, 0), Quaternion.identity);    
            }

            // Draw corners
            Instantiate(border, new Vector3(-1, -1, 0), Quaternion.identity);
            Instantiate(border, new Vector3(-1, height, 0), Quaternion.identity);
            Instantiate(border, new Vector3(width, -1, 0), Quaternion.identity);
            Instantiate(border, new Vector3(width, height, 0), Quaternion.identity);
        }

        #endregion
    }
}
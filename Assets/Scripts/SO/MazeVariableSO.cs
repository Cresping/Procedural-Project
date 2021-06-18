using UnityEngine;

namespace HeroesGames.ProjectProcedural.SO
{
    [CreateAssetMenu(fileName = nameof(MazeVariableSO), menuName = "Scriptables/" + nameof(MazeVariableSO) + "/" + "MazeVariable")]
    public class MazeVariableSO : DungeonVariableSO
    {
        private void MazeSize()
        {
            if (dungeonLvl % 2 == 0)
            {
                dungeonWidth = 13 + dungeonLvl;
                dungeonHeight = 13 + dungeonLvl;
            }
            else
            {
                dungeonWidth = 13 + dungeonLvl + 1;
                dungeonHeight = 13 + dungeonLvl + 1;
            }
        }
        
        public override void CalculateDifficulty() => MazeSize();
    }
}

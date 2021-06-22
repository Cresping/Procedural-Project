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
                dungeonWidth = 15 + dungeonLvl;
                dungeonHeight = 15 + dungeonLvl;
            }
            else
            {
                dungeonWidth = 15 + dungeonLvl + 1;
                dungeonHeight = 15 + dungeonLvl + 1;
            }
        }
        
        public override void CalculateDifficulty() => MazeSize();
    }
}

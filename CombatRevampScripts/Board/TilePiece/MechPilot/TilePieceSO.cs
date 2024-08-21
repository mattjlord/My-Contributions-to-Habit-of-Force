using UnityEngine;

namespace CombatRevampScripts.Board.TilePiece.MechPilot
{
    public class TilePieceSO : ScriptableObject
    {
        public string prefabPath;
        public MechSO mechSO;
        public PilotSO pilotSO;
        public FaceDirection faceDirection;
    }
}

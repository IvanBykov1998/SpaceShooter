using UnityEngine;

namespace SpaceShooter
{
    public class LevelCompletionScore : LevelCondition
    {
        [SerializeField] private int m_Score; // Количество очков нужное для победы

        public override bool IsCompleted
        {
            get
            {
                if (Player.Instance.ActiveShip == null) return false;

                if (Player.Instance.Score >= m_Score)
                {
                    return true;
                }

                return false;
            }
        }
    }
}
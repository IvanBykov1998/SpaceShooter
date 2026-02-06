using UnityEngine;

namespace SpaceShooter
{
    public class ProjectServices : MonoBehaviour
    {
        [SerializeField] private LevelSequencesController m_LevelSequences;

        private void Awake()
        {
            m_LevelSequences.Init();
        }
    }
}
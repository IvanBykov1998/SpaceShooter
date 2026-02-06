using UnityEngine;

namespace SpaceShooter
{
    public class LevelSelectionPanel : MonoBehaviour
    {
        [SerializeField] private LevelSelectionButton m_LevelButtonPrefab;
        [SerializeField] private Transform m_Parent;

        private void Start()
        {
            LevelProperties[] levelProperties = LevelSequencesController.Instance.LevelSequences.LevelsProperties;

            for (int i = 0; i < levelProperties.Length; i++)
            {
                LevelSelectionButton levelSelectionButton = Instantiate(m_LevelButtonPrefab);
                levelSelectionButton.SetLevelProperties(levelProperties[i]);
                levelSelectionButton.transform.SetParent(m_Parent, false);
                // levelSelectionButton.transform.parent = m_Parent;
            }
        }
    }
}
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

namespace SpaceShooter
{
    public class LevelSelectionButton : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI m_LevelTitle;
        [SerializeField] private Image m_PreviewImage;

        private LevelProperties m_LevelProperties;

        public void SetLevelProperties(LevelProperties levelProperties)
        {
            m_LevelProperties = levelProperties;

            m_PreviewImage.sprite = m_LevelProperties.PreviewImage;
            m_LevelTitle.text = m_LevelProperties.Title;
        }

        public void LoadLevel()
        {
            SceneManager.LoadScene(m_LevelProperties.SceneName);
        }
    }
}
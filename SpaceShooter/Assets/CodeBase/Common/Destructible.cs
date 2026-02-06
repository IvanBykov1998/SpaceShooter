using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace Common
{
    /// <summary>
    /// Уничтожаемый объект на сцене. То что может иметь хит поинты.
    /// </summary>
    public class Destructible : Entity
    {
        #region Properties
        /// <summary>
        /// Объект игнорирует повреждения.
        /// </summary>
        [SerializeField] protected bool m_Indestructible;
        public bool IsIndestructible => m_Indestructible;

        /// <summary>
        /// Стартовое значение хит поинтов.
        /// </summary>
        [SerializeField] private int m_HitPoints;
        public int MaxHitPoints => m_HitPoints;

        /// <summary>
        /// Текущее значение хит поинтов.
        /// </summary>
        protected int m_CurrentHitPoints;
        public int HitPoints => m_CurrentHitPoints;
        #endregion

        #region Unity Events
        protected virtual void Start()
        {
            m_CurrentHitPoints = m_HitPoints;

            transform.SetParent(null);
        }
        #endregion

        #region Public API
        /// <summary>
        /// Применение дамага к объекту.
        /// </summary>
        /// <param name="damage"> Урон наносимый объекту </param>
        public virtual void ApplyDamage(int damage)
        {
            if (m_Indestructible) return;

            m_CurrentHitPoints -= damage;

            if (m_CurrentHitPoints <= 0)
                OnDeath();
        }
        #endregion

        /// <summary>
        /// Переопределяемое событие уничтожения объекта, когда хит поинты ниже нуля.
        /// </summary>
        protected virtual void OnDeath()
        {
            Destroy(gameObject);

            m_EventOnDeath?.Invoke();
        }

        private static HashSet<Destructible> m_AllDestructibles; // Список всех уничтожаемых объектов

        public static IReadOnlyCollection<Destructible> AllDestructibles => m_AllDestructibles;

        protected virtual void OnEnable() // Добавление в список
        {
            if (m_AllDestructibles == null)
            {
                m_AllDestructibles = new HashSet<Destructible>();
            }

            m_AllDestructibles.Add(this);
        }

        protected virtual void OnDestroy() // Удаление из списка
        {
            m_AllDestructibles.Remove(this);
        }

        // Создание команд (враги, союзники)
        public const int TeamIdNeutral = 0; // нейтральная команда
        [SerializeField] private int m_TeamId;
        public int TeamId => m_TeamId;

        [SerializeField] private UnityEvent m_EventOnDeath;
        public UnityEvent EventOnDeath => m_EventOnDeath;

        [SerializeField] private int m_ScoreValue;
        public int ScoreValue => m_ScoreValue; 
    }
}
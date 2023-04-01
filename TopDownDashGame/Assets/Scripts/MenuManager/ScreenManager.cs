using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripts.MenuManager
{
    public abstract class ScreenManager : MonoBehaviour 
    {
        [SerializeField] protected GameObject m_screen;

        private void Awake()
        {
            if(m_screen == null)
                Debug.Log($"{this.name} - Screen Missing");            
        }

        private void Start()
        {
            Init();
        }

        public virtual void DisplayScreen()
        {
            m_screen.SetActive(true);
        }
        public virtual void HideScreen()
        {
            m_screen.SetActive(false);
        }

        protected virtual void Init()
        {
            HideScreen();
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AceTheChase.UI
{
    public class AnimationEndedTrigger : MonoBehaviour
    {
        // Start is called before the first frame update
        private UIManager m_uiManager;

        public void SetUIManager(UIManager uiManager)
        {
            m_uiManager = uiManager;
        }

        public void TriggerAnimationEnded()
        {
            if (m_uiManager != null)
            {
                m_uiManager.AnimationEnded();
            }
        }
    }
}

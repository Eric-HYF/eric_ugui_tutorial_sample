using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

namespace Sample
{

    /// <summary>
    /// The script for every tutorial step 
    /// You should add this script on the tutorial target
    /// 教程步骤脚本
    /// 应该将此脚本添加到教程步骤的目标物上 
    /// </summary>
    public class TutorialNode : MonoBehaviour
    {



        public TutorialNode nextNode;


        public System.Action<TutorialNode> OnTutorialStepFinished;

        private Button m_button;

        public Vector2 Pos
        {
            get {
                Transform t = transform;
                if(m_button != null) t = m_button.transform; 
                return new Vector2(t.localPosition.x, t.localPosition.y); 
            }
        }


        void Awake()
        {
            m_button = gameObject.GetComponent<Button>();
            if(m_button == null)
            {
                // if we can't find <Button> on gameObject, then find it in the children level. 
                m_button = gameObject.GetComponentInChildren<Button>(true); 
            }
        }



        public void OnPassEvent<T>(PointerEventData data, ExecuteEvents.EventFunction<T> function) where T : IEventSystemHandler
        {
            if(m_button != null)
            {
                // Pass Event to target gameObject
                ExecuteEvents.Execute(m_button.gameObject, data, function); 
                if(OnTutorialStepFinished != null) OnTutorialStepFinished(this);
            }
        }

    }
}



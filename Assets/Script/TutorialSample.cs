using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Sample
{



    /// <summary>
    /// Main script for the Demo, controling all the step flow.
    /// Demo 的主类, 控制所有教程的流程
    /// </summary>
    public class TutorialSample : MonoBehaviour
    {


        [SerializeField]
        private TutorialMask m_mask;


        [SerializeField]
        private TutorialNode[] m_nodes;

        [SerializeField]
        private GameObject m_startPanel;

        [SerializeField]
        private Button m_startButton;

        [SerializeField]
        private GameObject m_contentPanel;

        [SerializeField]
        private Button m_contentButton;


        private int m_currentStepId;

        private TutorialNode m_currentNode;


        private void Awake() 
        {
            
            m_currentStepId = 0;

            // init all node at the beginning
            for( int i = 0; i < m_nodes.Length; i++)
            {
                if(i + 1 < m_nodes.Length)
                {
                    m_nodes[i].nextNode = m_nodes[i+1];
                }
                m_nodes[i].OnTutorialStepFinished += OnTutorialStepFinished;
            }

            // UI event register
            m_startButton.onClick.AddListener(OnClickStartButton);
            m_contentButton.onClick.AddListener(OnClickContentButton);

            m_startPanel.gameObject.SetActive(true);
            m_contentPanel.gameObject.SetActive(false);

            m_mask.Active = false;
        }


        void Start()
        {
            // Set the first node
            if(m_currentStepId == 0)
            {
                SetCurrentNode(m_nodes[0]);
            }
        }


        private void OnClickStartButton()
        {
            m_startPanel.gameObject.SetActive(false);
            m_contentPanel.gameObject.SetActive(true);
        }

        private void OnClickContentButton()
        {
            m_startPanel.gameObject.SetActive(true);
            m_contentPanel.gameObject.SetActive(false);
        }


        void SetCurrentNode(TutorialNode node)
        {
            m_currentNode = node;
            if(m_currentNode != null)
            {
                Debug.Log(">>>> Lock Node: " + node.name);
                m_mask.Active = true;
                m_mask.SetPos(node.Pos);
            }
            else
            {
                // All steps finished 
                OnAllStepsFinished();
            }
        }

        void OnTutorialStepFinished(TutorialNode node)
        {
            SetCurrentNode(node.nextNode);
        }

        void OnAllStepsFinished()
        {
            m_mask.Active = false;
            Debug.Log(">>>> All steps finished");
        }
    }
}



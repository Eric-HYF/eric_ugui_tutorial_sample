using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;


namespace Sample
{

    /// <summary>
    /// The class for the Black Mask 
    /// 黑色遮罩控制类 
    /// </summary>
    public class TutorialMask : MonoBehaviour, IPointerClickHandler
    {

        public const float K_DEFALUT_ROUND_MASK_RADIUS = 150f;

        /// <summary>
        /// The Material with the spec shader
        /// 使用了特定Shader的材质球
        /// </summary>
        [SerializeField]
        private Material m_maskMat;


        [SerializeField]
        private Image m_fullScreenMask;

        // [SerializeField]
        // private Image m_fullScreenMask;

        private Material m_material; 

        private Vector2 m_targetPos;

        private float m_maskRadius;



        public bool Active{
            get { return gameObject.activeSelf; }
            set { gameObject.SetActive(value); }
        }



        // Start is called before the first frame update
        void Awake()
        {
            m_material = Instantiate(m_maskMat);
        }

        
        public void SetPos(Vector2 pos)
        {
            m_targetPos = pos;
            m_maskRadius = K_DEFALUT_ROUND_MASK_RADIUS;
            SetRoundMask(pos,  m_maskRadius);
        }



        /// <summary>
        /// Set Round Mask's [Center] and [Radius]
        /// 设置原型遮罩的 [中心] 和 [半径]
        /// </summary> 
        /// <param name="pos"></param>
        /// <param name="r"></param>
        private void SetRoundMask(Vector2 pos,  float r)
        {
            m_fullScreenMask.material = m_material;
            m_material.SetFloat("_Radius", r);
            m_material.SetVector("_Center", new Vector4(pos.x, pos.y, 0, 0));
        }



        Vector2 clickPos;
        /// <summary>
        /// We shold implement this method on mask has been clicked, and pass the event to the target
        /// 我们应该实现这个接口方法, 然后把点击事件传递给目标对象.
        /// </summary>
        /// <param name="eventData"></param>
        public void OnPointerClick(PointerEventData eventData)
        {
            
            RectTransformUtility.ScreenPointToLocalPointInRectangle(transform as RectTransform, eventData.position, Camera.main, out clickPos);
            // Debug.Log(">>>> Click Point pos: " + clickPos);

            float len = Vector2.Distance(clickPos, m_targetPos);
            if(len < m_maskRadius)
            {
                SearchTarget(eventData, ExecuteEvents.pointerClickHandler);
            }
            else
            {
                Debug.Log("=== DISMISS ===");
            }
        }


        /// <summary>
        /// Pass the event to the target <T>
        /// 将事件传递给目标物件<T>
        /// </summary>
        /// <param name="data"></param>
        /// <param name="function"></param>
        /// <typeparam name="T"></typeparam>
        private void SearchTarget<T>(PointerEventData data, ExecuteEvents.EventFunction<T> function) where T : IEventSystemHandler
        {
            var results = new List<RaycastResult>();
            EventSystem.current.RaycastAll(data, results);
            var current = data.pointerCurrentRaycast.gameObject;
            for (int i = 0; i < results.Count; i++)
            {

                // Debug.Log("Current: " + current.name);

                if (current != results[i].gameObject)
                {
                    TutorialNode node = results[i].gameObject.GetComponent<TutorialNode>();
                    if(node != null)
                    {
                        this.Active = false;
                        node.OnPassEvent<T>(data, function);
                        break;
                    }

                }
            }
        }



    }
}



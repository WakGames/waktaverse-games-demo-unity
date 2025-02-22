using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace WakSDK
{
    public class AchievePanel : MonoBehaviour
    {
        [Header("UI")]
        [SerializeField] private RawImage icon;
        [SerializeField] private TextMeshProUGUI nameText;
        [SerializeField] private TextMeshProUGUI descriptionText;

        private RectTransform _rectTransform;
        private CanvasGroup _canvasGroup;

        private const float Height = 80f;
        private const float MoveTime = 0.5f;
        private const float WaitTime = 2f;
        private const float FadeTime = 0.5f;
    
        private int _direction;
        private int _index = 0;
    
        private Coroutine _slideCoroutine = null;
        private UnityAction _onHide;
    
        private void Awake()
        {
            _rectTransform = GetComponent<RectTransform>();
            _canvasGroup = GetComponent<CanvasGroup>();
        }
        
        public void SetOnHide(UnityAction onHide)
        {
            _onHide = onHide;
        }

        public void Setup(string achieveName, string achieveDesc, Texture2D texture, AchievePopupPosition position)
        {
            nameText.text = achieveName;
            descriptionText.text = achieveDesc;
            _direction = (int)position;
        
            if (texture)
                icon.texture = texture;
        
            ResetIndex();
            SlideUp();
            StartCoroutine(WaitForFade());
        }

        private void ResetIndex()
        {
            _index = 0;
            _rectTransform.anchoredPosition = GetPanelPivotLocation(_index);
            _canvasGroup.alpha = 1f;
        }
    
        public void SlideUp()
        {
            if (_slideCoroutine != null)
            {
                StopCoroutine(_slideCoroutine);
                _slideCoroutine = null;
            }
        
            _index++;
            _slideCoroutine = StartCoroutine(SlideUpCoroutine());
        }

        private IEnumerator SlideUpCoroutine()
        {
            float time = 0f;
            Vector2 origin = _rectTransform.anchoredPosition;
            Vector2 destination = GetPanelPivotLocation(_index);
        
            while(time < MoveTime)
            {
                time += Time.deltaTime;
                _rectTransform.anchoredPosition =
                    Vector2.Lerp(origin, destination, Mathf.SmoothStep(0f, 1f, time / MoveTime));
                yield return null;
            }
        
            _rectTransform.anchoredPosition = destination;
        }
    
        private IEnumerator WaitForFade()
        {
            yield return new WaitForSeconds(WaitTime);
            yield return FadeCoroutine();
        }

        private IEnumerator FadeCoroutine()
        {
            float time = 0f;
        
            while(time < FadeTime)
            {
                time += Time.deltaTime;
                _canvasGroup.alpha = Mathf.SmoothStep(1f, 0f, time / FadeTime);
                yield return null;
            }
        
            _canvasGroup.alpha = 0f;
            _onHide?.Invoke();
        }

        private Vector2 GetPanelPivotLocation(int idx)
        {
            // x 좌표: 절대값에 따라 좌우 결정
            float x = Mathf.Abs(_direction) == 2 ? 815 : -815;
            // y 좌표: 부호에 따라 상/하단 결정
            float y = _direction > 0 ? 580 : -580;
            // idx가 1이면 y 값에 80 또는 -80을 추가
            if (idx == 1)
            {
                y += _direction > 0 ? -80 : 80;
            }
            return new Vector2(x, y);
        }
    }
}
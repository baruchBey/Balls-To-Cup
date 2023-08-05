using TMPro;
using UnityEngine;
using DG.Tweening;
using System.Collections;

namespace Baruch
{

    public class FadingText : MonoBehaviour
    {
        TMP_Text _text;
        YieldInstruction _threeSecond = new WaitForSeconds(3f);

        private void Start()
        {
            _text = GetComponent<TMP_Text>();
        }
        private void Update()
        {
            if (Input.GetMouseButtonDown(0))
            {
                _text.DOFade(0f, 0.2f);
                StopAllCoroutines();
            }
            else if (Input.GetMouseButtonUp(0))
            {
                StartCoroutine(EnableMarbleCountText());
            }
        }
        IEnumerator EnableMarbleCountText()
        {
            yield return _threeSecond;
            _text.DOFade(1f, 0.2f);

        }
    }



}
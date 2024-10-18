using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class FightButton : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public Button button;
    [SerializeField] private float enlargeScale = 1.2f;

    // Update is called once per frame
    void Update()
    {
        if (Board.Instance.currentPhase != Board.combatPhase.Play)
        {
            button.image.color = Color.red;
        }
        else button.image.color = Color.white;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        transform.localScale *= enlargeScale;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        transform.localScale = new Vector3(1f, 1f, 1f);
    }
}

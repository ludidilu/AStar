using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class Unit : MonoBehaviour,IPointerEnterHandler,IPointerExitHandler {

	[SerializeField]
	public Image img;

	public int id;

	public bool isBlock;

	public void OnPointerEnter(PointerEventData eventData){

		if (isBlock) {

			return;
		}

		SendMessageUpwards ("UnitEnter", this);
	}

	public void OnPointerExit(PointerEventData eventData){
		
		if (isBlock) {
			
			return;
		}

		SendMessageUpwards ("UnitExit", this);
	}
}

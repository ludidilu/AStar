using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class NewBehaviourScript : MonoBehaviour {

	private const int width = 40;
	private const int height = 60;

	private const float cellWidth = 15;

	[SerializeField]
	private GameObject resourceGo;

	private Unit[] gos = new Unit[width * height];

	private Dictionary<int,bool> block = new Dictionary<int, bool>();

	// Use this for initialization
	void Start () {

		AStar.Init (width, height);
	
		for (int i = 0; i < height; i++) {

			for(int m = 0 ; m < width ; m++){

				GameObject go = GameObject.Instantiate<GameObject>(resourceGo);

				go.transform.SetParent(transform,false);

				Unit unit = go.GetComponent<Unit>();

				unit.id = i * width + m;

				gos[unit.id] = unit;

				(go.transform as RectTransform).anchoredPosition = new Vector2(-width * cellWidth / 2 + m * cellWidth,-height * cellWidth / 2 + i * cellWidth);

				if(Random.value < 0.1f){

					unit.isBlock = true;

					unit.img.color = Color.blue;

					block.Add(unit.id,false);
				}
			}
		}
	}

	private Unit nowUnit;
	
	void UnitEnter(Unit _unit){

		nowUnit = _unit;
	}

	void UnitExit(Unit _unit){
		
		nowUnit = null;
	}

	private Unit start;

	void Update(){

		if (Input.GetKeyUp (KeyCode.A)) {

			if(start != null){

				start.img.color = Color.white;
			}

			if(nowUnit != null){

				start = nowUnit;

				start.img.color = Color.red;
			}
		}

		if (Input.GetKeyUp (KeyCode.B)) {

			if(nowUnit != null){

				nowUnit.img.color = Color.green;

				if(start != null){
					
					List<int> result = AStar.Find(start.id,1,nowUnit.id,block,3);

					if(result != null){

						for(int i = 0 ; i < result.Count ; i++){

							gos[result[i]].img.color = Color.black;
						}
					}
				}
			}
		}


		if(Input.GetKeyUp(KeyCode.Space)){

			start = null;

			foreach(Unit u in gos){

				if(!u.isBlock){

					u.img.color = Color.white;
				}
			}
		}
	}
}

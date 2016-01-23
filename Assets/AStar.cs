using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class AStar{

	private struct AstarUnit{

		public int id;
		public int x;
		public int y;
		public int q;
		public int h;
		public int parent;
	}

	private static int width;
	private static int height;

	public static void Init(int _width,int _height){

		width = _width;
		height = _height;
	}

	public static List<int> Find(int _start,int _radios,int _end,Dictionary<int,bool> _block,int _range){

		List<int> openID = new List<int> ();

		List<AstarUnit> open = new List<AstarUnit> ();

		Dictionary<int,AstarUnit> close = new Dictionary<int, AstarUnit> ();

		AstarUnit start = new AstarUnit ();

		start.id = _start;

		start.x = _start % width;
		
		start.y = _start / width;

		AstarUnit end = new AstarUnit ();

		end.id = _end;

		end.x = _end % width;
		
		end.y = _end / width;

		openID.Add (_start);

		open.Add (start);

		while (true) {

			if(open.Count == 0){

				return null;
			}

			AstarUnit nowUnit = open[0];

			openID.RemoveAt(0);

			open.RemoveAt(0);

			close.Add(nowUnit.id,nowUnit);

			int[] neighbour = GetNeighbour(ref nowUnit,_block,_radios);

			for(int i = 0 ; i < 8 ; i++){

				int tmpID = neighbour[i];

				if(tmpID == -1){

					continue;
				}



//				if(tmpID == _end){
//
//					List<int> result = new List<int>();
//
//					AstarUnit kk = nowUnit;
//
//					while(true){
//
//						result.Add(kk.id);
//
//						if(kk.parent == _start){
//
//							result.Reverse();
//
//							return result;
//
//						}else{
//
//							kk = close[kk.parent];
//						}
//					}
//				}

				if(close.ContainsKey(tmpID)){

					continue;
				}

				int tt = openID.IndexOf(tmpID);

				if(tt == -1){
					
					AstarUnit newUnit = new AstarUnit();

					newUnit.id = tmpID;

					newUnit.x = tmpID % width;

					newUnit.y = tmpID / width;

					newUnit.q = nowUnit.q + i < 4 ? 2 : 3;

					newUnit.h = GetS(ref newUnit,ref end);

					newUnit.parent = nowUnit.id;

					if(newUnit.h <= _range){

						List<int> result = new List<int>();
						
						AstarUnit kk = newUnit;
						
						while(true){
							
							result.Add(kk.id);
							
							if(kk.parent == _start){
								
								result.Reverse();
								
								return result;
								
							}else{
								
								kk = close[kk.parent];
							}
						}
					}

					InsertToOpen(ref newUnit,open,openID);

				}else{

					AstarUnit oldUnit = open[tt];

					int newQ = i < 4 ? nowUnit.q + 2 : nowUnit.q + 3;

					if(newQ < oldUnit.q){

						oldUnit.q = newQ;

						oldUnit.parent = nowUnit.id;

						openID.RemoveAt(tt);

						open.RemoveAt(tt);

						InsertToOpen(ref oldUnit,open,openID);
					}
				}
			}
		}

		return null;
	}

	private static void InsertToOpen(ref AstarUnit _unit,List<AstarUnit> _unitList,List<int> _idList){

		for(int m = 0 ; m < _unitList.Count ; m++){
			
			AstarUnit k = _unitList[m];
			
			if(_unit.q + _unit.h <= k.q + k.h){
				
				_idList.Insert(m,_unit.id);
				
				_unitList.Insert(m,_unit);
				
				return;
			}
		}
		
		_idList.Add(_unit.id);
			
		_unitList.Add(_unit);
	}

	private static int[] GetNeighbour(ref AstarUnit _unit,Dictionary<int,bool> _block,int _radios){

		int[] result = new int[8];
		
		int checkNum = _radios * 2 + 1;

		if (_unit.y > _radios) {

			int start = _unit.id - width * (_radios + 1) - _radios;

			bool getBlock = false;

			for(int i = 0 ; i < checkNum ; i++){

				int checkPos = start + i;

				if(_block.ContainsKey(checkPos)){

					getBlock = true;

					break;
				}
			}

			if(!getBlock){

				result [0] = _unit.id - width;

			}else{

				result [0] = -1;
			}

		} else {

			result [0] = -1;
		}

		if (_unit.y < height - 1 - _radios) {

			int start = _unit.id + width * (_radios + 1) - _radios;
			
			bool getBlock = false;
			
			for(int i = 0 ; i < checkNum ; i++){
				
				int checkPos = start + i;
				
				if(_block.ContainsKey(checkPos)){
					
					getBlock = true;
					
					break;
				}
			}
			
			if(!getBlock){
				
				result [1] = _unit.id + width;
				
			}else{
				
				result [1] = -1;
			}

		} else {

			result [1] = -1;
		}

		if (_unit.x > _radios) {

			int start = _unit.id - _radios - 1 - width * _radios;
			
			bool getBlock = false;
			
			for(int i = 0 ; i < checkNum ; i++){
				
				int checkPos = start + i * width;
				
				if(_block.ContainsKey(checkPos)){
					
					getBlock = true;
					
					break;
				}
			}
			
			if(!getBlock){
				
				result [2] = _unit.id - 1;
				
			}else{
				
				result [2] = -1;
			}

		} else {
			
			result [2] = -1;
		}

		if (_unit.x < width - 1 - _radios) {

			int start = _unit.id + _radios + 1 - width * _radios;
			
			bool getBlock = false;
			
			for(int i = 0 ; i < checkNum ; i++){
				
				int checkPos = start + i * width;
				
				if(_block.ContainsKey(checkPos)){
					
					getBlock = true;
					
					break;
				}
			}
			
			if(!getBlock){
				
				result [3] = _unit.id + 1;
				
			}else{
				
				result [3] = -1;
			}

		} else {
			
			result [3] = -1;
		}

		checkNum = _radios * 2;

		if (_unit.y > _radios && _unit.x > _radios) {

			int start = _unit.id - _radios - 1 - width * (_radios + 1);

			bool getBlock = _block.ContainsKey(start);

			if(!getBlock){

				for(int i = 1 ; i <= checkNum ; i++){
					
					int checkPos = start + i;
					
					if(_block.ContainsKey(checkPos)){
						
						getBlock = true;
						
						break;
					}

					checkPos = start + i * width;

					if(_block.ContainsKey(checkPos)){
						
						getBlock = true;
						
						break;
					}
				}
			}

			if(!getBlock){
				
				result [4] = _unit.id - width - 1;
				
			}else{
				
				result [4] = -1;
			}

		} else {

			result [4] = -1;
		}

		if (_unit.y > _radios && _unit.x < width - 1 - _radios) {

			int start = _unit.id + _radios + 1 - width * (_radios + 1);
			
			bool getBlock = _block.ContainsKey(start);
			
			if(!getBlock){
				
				for(int i = 1 ; i <= checkNum ; i++){
					
					int checkPos = start - i;
					
					if(_block.ContainsKey(checkPos)){
						
						getBlock = true;
						
						break;
					}
					
					checkPos = start + i * width;
					
					if(_block.ContainsKey(checkPos)){
						
						getBlock = true;
						
						break;
					}
				}
			}
			
			if(!getBlock){
				
				result [5] = _unit.id - width + 1;
				
			}else{
				
				result [5] = -1;
			}

		} else {

			result [5] = -1;
		}

		if (_unit.y < height - 1 - _radios && _unit.x > _radios) {

			int start = _unit.id - _radios - 1 + width * (_radios + 1);
			
			bool getBlock = _block.ContainsKey(start);
			
			if(!getBlock){
				
				for(int i = 1 ; i <= checkNum ; i++){
					
					int checkPos = start + i;
					
					if(_block.ContainsKey(checkPos)){
						
						getBlock = true;
						
						break;
					}
					
					checkPos = start - i * width;
					
					if(_block.ContainsKey(checkPos)){
						
						getBlock = true;
						
						break;
					}
				}
			}

			if(!getBlock){
				
				result [6] = _unit.id + width - 1;
				
			}else{
				
				result [6] = -1;
			}

		} else {

			result [6] = -1;
		}

		if (_unit.y < height - 1 - _radios && _unit.x < width - 1 - _radios) {

			int start = _unit.id + _radios + 1 + width * (_radios + 1);
			
			bool getBlock = _block.ContainsKey(start);
			
			if(!getBlock){
				
				for(int i = 1 ; i <= checkNum ; i++){
					
					int checkPos = start - i;
					
					if(_block.ContainsKey(checkPos)){
						
						getBlock = true;
						
						break;
					}
					
					checkPos = start - i * width;
					
					if(_block.ContainsKey(checkPos)){
						
						getBlock = true;
						
						break;
					}
				}
			}

			if(!getBlock){
				
				result [7] = _unit.id + width + 1;
				
			}else{
				
				result [7] = -1;
			}

		} else {
			
			result [7] = -1;
		}

		return result;
	}

	private static int GetS(ref AstarUnit _start,ref AstarUnit _end){

		int dX = Mathf.Abs (_start.x - _end.x);
		int dY = Mathf.Abs (_start.y - _end.y);

		if (dX == dY) {

			return dX * 3;

		} else if (dX > dY) {

			return dY * 3 + (dX - dY) * 2;

		} else {

			return dX * 3 + (dY - dX) * 2;
		}
	}
}

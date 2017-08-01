using UnityEngine; // Debug,Mathf,JsonUtilityに必要（代用できるならいらないかも）
// using UnityEngine.UI;
using System;
using System.Collections;
using System.Collections.Generic;

public class GameMaster{
	public static int[] Die_Count = new int[8]{0,0,0,0,0,0,0,0};

	public MapBoard_Cal board;
	public static int times;

	public static List<Unit_Cal> units;
	public List<Unit_Cal> move_units;

	public static int My_Left_Count;
	public static int Enemy_Left_Count;
	public static int My_KING_HP;
	public static int Enemy_KING_HP;

	public string loseTeam;
	bool gameOver;
	Dictionarys dic = new Dictionarys();

	int[] DethSlot;
	Player player1;
	Player player2;
	public GameMaster(){
		NodeArrow.Init();
	}

	void SpawnUnit(int _id, Player _player){
		int tmp_position_x;
		float tmp_position_y = 0;
		int tmp_position_z = (Constants.MAP_SIZE_Z+1)/2;
		int _hp = 100;
		Unit_Plans _plans = new Unit_Plans();

		switch(_id){
			case 0:
			_hp = 500;
			tmp_position_y = 0.2f;
			if(_player.color == "RED"){
				tmp_position_x = 3;
			}
			else{
				tmp_position_x = Constants.MAP_SIZE_X+1 -3;
			}
			break;			
			case 1:
			if(_player.color == "RED"){
				tmp_position_x = 5;
			}
			else{
				tmp_position_x = Constants.MAP_SIZE_X+1 -5;
			}
			break;
			case 2:
			if(_player.color == "RED"){
				tmp_position_x = 5;
				tmp_position_z += 2;
			}
			else{
				tmp_position_x = Constants.MAP_SIZE_X+1 -5;
				tmp_position_z -= 2;
			}
			break;
			case 3:
			if(_player.color == "RED"){
				tmp_position_x = 5;
				tmp_position_z -= 2;
			}
			else{
				tmp_position_x = Constants.MAP_SIZE_X+1 -5;
				tmp_position_z += 2;
			}
			break;
			default:
			tmp_position_x = 3;
			break;
		}
		string _team = "My";
		if(_player.color == "BLUE"){
			_team = "Enemy";
		}		
		Unit_Cal cube =  new Unit_Cal(tmp_position_x, tmp_position_z, 2, 100+GetWaitDiff(GetType(_id, _player)) , _player, _hp, 25, 50, 4, _id, _player.plans[_id]);
		units.Add(cube);
		board.SetMapBoard(tmp_position_x, tmp_position_z, cube);
	}


	string GetType(int _id, Player _player){
		int tmp_type = dic.Chara_Dic[_player.Units[_id]][0];
		switch(tmp_type){
			case 0:
			return "KING";
			case 1:
			return "QUEEN";
			case 2:
			return "KNIGHT";
			case 3:
			return "BISHOP";
			case 4:
			return "PAWN";
			case 5:
			return "ROOK";
			default:
			return "KNIGHT";
		}
	}

	public void Respown(int _id, string _team){
		if(_team == "My"){
			My_Left_Count--;
			if(My_Left_Count <= 0){
				gameOver = true;
			}
		}
		else if (_team == "Enemy"){
			Enemy_Left_Count--;
			if(Enemy_Left_Count <= 0){
				gameOver = true;
			}
		}
	}

	int GetWaitTime(List<int []> _planList){
		int tmp_num = 0;
		foreach(int[] _array in _planList){
			for(int i=0; i<_array.Length; i++){
				if(_array[i] == 0){
					continue;
				}
				tmp_num++;
			}
		}
		tmp_num = (tmp_num/3)*3; // 0,1,2,3,4 = 0; 5,6,7,8,9 = 2; 
		return tmp_num;
	}

	int GetWaitDiff(string _type){
		switch(_type){
			case "KING":
			return 0;
			case "QUEEN":
			return 10;
			case "KNIGHT":
			return -20;
			case "BISHOP":
			return -10;
			case "PAWN":
			return -5;
			case "ROOK":
			return 5;
			default:
			return 0;
		}
	}
	//ゲーム開始
	string play_mode;
	bool IsGameOver;

	float start_time;

	public string Play(Player _player1, Player _player2){
		// start_time = Time.time;
		Init(_player1, _player2);
		MainPlay();

		if(loseTeam == "My"){
			return "PLAYER1";
		}
		else if(loseTeam == "Enemy"){
			return "PLAYER2";
		}
		return loseTeam;
	}

	void Init(Player _player1, Player _player2){
		_player1.color = "RED";
		_player2.color = "BLUE";
		this.player1 = _player1;
		this.player2 = _player2;
		/*
		if(_player1.color == "RED"){
			this.player1 = _player1;
			this.player2 = _player2;
		}
		else{
			this.player1 = _player2;
			this.player2 = _player1;			
		}*/
		// 時間の生成
		times = 200;
		gameOver = false;
		loseTeam = "";
		DethSlot = new int[8];
		Die_Count = new int[8]{0,0,0,0,0,0,0,0};

		// ボードの生成　初期化
		board = new MapBoard_Cal();

		// スコアの初期化
		My_Left_Count = 6;
		Enemy_Left_Count = 6;
		My_KING_HP = 500;
		Enemy_KING_HP = 500;
		tmp_count = 0;

		// ユニットの作成 & ボードに配置
		units = new List<Unit_Cal>();
		move_units = new List<Unit_Cal>();

		// ユニットの生成 初期化
		/*
		SpawnUnit(0, "My");			// キング
		SpawnUnit(0, "Enemy");	// キング
		SpawnUnit(1, "Enemy");	// 中
		SpawnUnit(1, "My");			// 中
		SpawnUnit(2, "My");			// 上
		SpawnUnit(2, "Enemy"); 	// 上
		SpawnUnit(3, "Enemy");	// 下
		SpawnUnit(3, "My");			// 下
		*/
		SpawnUnit(0, this.player1);			// キング
		SpawnUnit(0, this.player2);	// キング
		SpawnUnit(1, this.player2);	// 中
		SpawnUnit(1, this.player1);			// 中
		SpawnUnit(2, this.player1);			// 上
		SpawnUnit(2, this.player2); 	// 上
		SpawnUnit(3, this.player2);	// 下
		SpawnUnit(3, this.player1);			// 下


	}
	void MainPlay(){
		play_mode = "RESPOWN";
		IsGameOver = false;

		NextPhase();
	}

	int tmp_count;

	void NextPhase(){
		//Debug.Log(play_mode);
		switch(play_mode){
			case "RESPOWN":
			ResPownPhase();
			play_mode = "SELECT";
			break;
			case "SELECT":
			SelectUnitPhase();
			play_mode = "GET_EVENT";
			break;
			case "GET_EVENT":
			GetEventPhase();
			play_mode = "MOVE_CAL";
			break;
			case "MOVE_CAL":
			MoveCalPhase();
			play_mode = "BATTLE_CAL";
			break;
			case "BATTLE_CAL":
			BattleCalPhase();
			play_mode = "DELETE";
			break;
			case "DELETE":
			DeletePhase();
			play_mode = "COUNT_DOWN";
			break;
			case "COUNT_DOWN":
			CountDownPhase();
			play_mode = "END";
			break;
			case "END":
			EndPhase();
			break;
			case "GAME_OVER":
			GameOverPhase();
			// Debug.Log("tmp_count:"+tmp_count);
			return;
		}
		NextPhase();
		/*
		tmp_count++;
		// TODO NextPlase()をすぐに呼ぶとUnityがフリーズしたりする。フレーム等の問題なのかな？
		// => Debug.Logを入れなかったら通常処理の方が速かったのでInvokeはなし。
		// 5回に1回は遅くする処理を加えている。ここを直すと高速化ができるかも。
		if(play_mode == "GET_EVENT" && tmp_count%5 == 0){
			//Invoke("NextPhase", 0f); // こっちにするとスムーズに処理する。
			NextPhase(); // こっちにするとちょっと動きが止まる（数秒後に表示される）
		}
		else{
			NextPhase();
		}*/
	}


	void ResPownPhase(){
		// 死亡スロットを1増やし20なら復活
		for(int i=0; i<DethSlot.Length; i++){
			if(DethSlot[i]>0){
				DethSlot[i]++;
			}
			if(DethSlot[i] == 20){
				DethSlot[i] = 0;
				// IsRes = true;
				if(i<4){
					SpawnUnit(i, this.player1);// 復活
				}
				else{
					SpawnUnit(i-4, this.player2);
				}
			}
		}
	}
	void SelectUnitPhase(){
		int can_move_count = 10000;
		foreach(Unit_Cal unit in units){
			if(unit == null){
				continue;
			}
			// waitTimeを取得
			int tmp_waitTime = unit.waitTime;
			//Debug.Log(tmp_waitTime);
			// より早いunitがあればmove_unitsを新規作成
			if(can_move_count > tmp_waitTime){
				can_move_count = tmp_waitTime;
				move_units.Clear();
				move_units.Add(unit);
			}
			else if(can_move_count == tmp_waitTime){
				// 同じなら追加
				move_units.Add(unit);
			}
		}
	}
	void GetEventPhase(){
		foreach(Unit_Cal unit in move_units){
			if(unit == null){
				continue;
			}
			unit.SetMEvent(board);
		}
	}
	void MoveCalPhase(){
		// 移動関数 = 移動範囲を求める，ターゲットを決める，移動場所を決める
		foreach(Unit_Cal unit in move_units){
			if(unit == null){
				continue;
			}
			unit.Move(board);
		}		
	}

	void BattleCalPhase(){
		// Debug.Log("MyKING:"+units[0].hp);
		// Debug.Log("EnemyKING:"+units[4].hp);			
		// 攻撃関数 = ターゲットを決めてダメージ計算する
		// Debug.Log("時間:"+times);
		foreach(Unit_Cal unit in move_units){
			if(unit == null){
				continue;
			}
			unit.Battle(board);
			/*
			string tmp_action = dic.GetString(dic.Action_Dic, unit.action);
			if(unit.target_unit == null){
				Debug.Log("("+unit.xz_position[0]+","+unit.xz_position[1]+")"+":"+unit.team+":"+unit.type+":"+unit.hp+":"+"sta"+unit.stamina+":"+tmp_action);
			}
			else{
				Debug.Log("("+unit.xz_position[0]+","+unit.xz_position[1]+")"+unit.team+":"+unit.type+":"+unit.attack+":"+"sta"+unit.stamina+":"+tmp_action+":"+unit.target_unit.type+":"+unit.target_unit.hp+":id="+unit.target_unit._id);
			}*/
		}
		//スタミナ計算
		foreach(Unit_Cal unit in move_units){
			if(unit.IsAttack || unit.IsLongAttack || unit.IsS_LongAttack || unit.IsHeal || unit.IsCharge || unit.IsSpeedUP || unit.IsGuard){
				unit.StaminaDown();
			}
			else{
				unit.StaminaUp();
			}
			unit.sum_damage = 0;
			SetFalseFlag(unit);
		}
	}
	void SetFalseFlag(Unit_Cal _unit){
		_unit.IsAttack 				= false;
		_unit.IsLongAttack 		= false;
		_unit.IsS_LongAttack 	= false;
		_unit.IsHeal 			= false;
		_unit.IsCharge 		= false;
		_unit.IsSpeedUP 	= false;
		_unit.IsGuard 		= false;
		// 受け
		_unit.IsDamage 		= false;
		_unit.IsRecovery 	= false;
		_unit.IsCharged 	= false;
		_unit.IsSpeedUPed = false;
		_unit.IsGuarded		= false;
	}	

	void DeletePhase(){
		int unit_count = units.Count;
		for(int i=unit_count -1; i>=0; i--){
			Unit_Cal unit = units[i];
			if(unit.type == "KING"){
				if(unit.team == "My"){
					My_KING_HP = unit.hp;
				}
				else{
					Enemy_KING_HP = unit.hp;
				}
			}
			if(unit.hp <= 0){
				units.Remove(unit);				// ユニットを排除
				if(unit.team == "My"){
					Die_Count[unit._id]++;
					DethSlot[unit._id]++;
				}
				else{
					Die_Count[unit._id+4]++;
					DethSlot[unit._id+4]++;
				}
				Respown(unit._id, unit.team);
				board.SetMapBoard(unit.xz_position[0], unit.xz_position[1], null);
				if(unit.type == "KING"){
					gameOver = true;
					// SetLOSE(unit.team+"KING");
				}
				unit = null;
				// Destroy(unit);					
			}
		}
	}
	void CountDownPhase(){
		// カウントダウン
		times--;
	}
	void EndPhase(){
		// Debug.Log(times+":My_Left_Count:"+My_Left_Count);
		// Debug.Log(times+":Enemy_Left_Count:"+Enemy_Left_Count);
		// もしゲーム終了なら
		if(times < 1 || My_Left_Count < 1 || Enemy_Left_Count < 1 || gameOver){
			play_mode = "GAME_OVER";
			return;
		}
		else{
			play_mode = "RESPOWN";
		}


		int moved_waitTime = move_units[0].waitTime;
		// unitのwaitTimeを引く
		foreach(Unit_Cal unit in units){
			if(unit == null){
				continue;
			}

			unit.DownWaitTime(moved_waitTime);
		}
		// 動いたユニットのwaittimeを戻す
		foreach(Unit_Cal unit in move_units){
			if(unit == null){
				continue;
			}
			unit.ResetWaitTime();
		}
		//Debug.Log("TIME:"+times);
	}
	void GameOverPhase(){
		if(times < 1){// 時間切れなら キングのHPを比較
			if(My_KING_HP > Enemy_KING_HP){
				loseTeam = "Enemy";
			}
			else if(My_KING_HP < Enemy_KING_HP){
				loseTeam = "My";
			}
			else if(My_Left_Count > Enemy_Left_Count){
				loseTeam = "Enemy";
			}
			else if(My_Left_Count < Enemy_Left_Count){
				loseTeam = "My";
			}
			else{
				loseTeam = "時間切れ";
			}
		}
		else{// 時間内ならユニット破壊かキングのHP
			if(My_KING_HP < 1 && Enemy_KING_HP < 1){
				loseTeam = "引き分け";
			}
			else if(My_KING_HP < 1){
				loseTeam = "My";
			}
			else if(Enemy_KING_HP < 1){
				loseTeam = "Enemy";
			}
			else if(My_Left_Count < 1 && Enemy_Left_Count  < 1 ){
				loseTeam = "引き分け";
			}
			else if(My_Left_Count < 1){
				loseTeam = "My";
			}
			else if(Enemy_Left_Count < 1){
				loseTeam = "Enemy";
			}
			else{
				loseTeam = "引き分け";
			}
		}
		// float end_time = Time.time - start_time;
		// Debug.Log(end_time);		
		// Debug.Log("敵："+Enemys.name);
		// Debug.Log(times+":My_Left_Count:"+My_Left_Count+":"+My_KING_HP);
		// Debug.Log(times+":Enemy_Left_Count:"+Enemy_Left_Count+":"+Enemy_KING_HP);
		
		// 加藤が負けようが終わり
		return;
	}
}

public class MapBoard_Cal {
	public int[,] map_Board; // マップ構成
	Unit_Cal[,] map_Board_Unit;  // ユニット構成

	public MapBoard_Cal(){
		beginning_map_Board();
	}

	void beginning_map_Board(){
		map_Board = new int[Constants.MAP_SIZE_X+2, Constants.MAP_SIZE_Z+2];
		map_Board_Unit = new Unit_Cal[Constants.MAP_SIZE_X+2, Constants.MAP_SIZE_Z+2];
		for(int x=0; x<map_Board.GetLength(0); x++){
			for(int z=0; z<map_Board.GetLength(1); z++){
				// 外壁
				if(x==0 || x==Constants.MAP_SIZE_X+1 || z==0 || z == Constants.MAP_SIZE_Z+1){
					map_Board[x,z] = -100;
					map_Board_Unit[x,z] = null;
					continue;
				}
				// ユニット排出場所
				if((x==1 && z==1) || (x==Constants.MAP_SIZE_X && z == Constants.MAP_SIZE_Z)
					|| (x==1 && z == Constants.MAP_SIZE_Z) || (x==Constants.MAP_SIZE_X && z == 1)){
					map_Board[x,z] = -50;
					map_Board_Unit[x,z] = null;	
					continue;
				}
				if(x == 3 && z == (Constants.MAP_SIZE_Z+1)/2
					|| x == 5 && z == (Constants.MAP_SIZE_Z+1)/2
					|| x == 5 && z == (Constants.MAP_SIZE_Z+1)/2 -2
					|| x == 5 && z == (Constants.MAP_SIZE_Z+1)/2 +2
					|| x == Constants.MAP_SIZE_X+1 -3 && z == (Constants.MAP_SIZE_Z+1)/2
					|| x == Constants.MAP_SIZE_X+1 -5 && z == (Constants.MAP_SIZE_Z+1)/2
					|| x == Constants.MAP_SIZE_X+1 -5 && z == (Constants.MAP_SIZE_Z+1)/2 - 2
					|| x == Constants.MAP_SIZE_X+1 -5 && z == (Constants.MAP_SIZE_Z+1)/2 + 2){
					map_Board[x,z] = -50;
					map_Board_Unit[x,z] = null;	
					continue;				
				}
				// それ以外
				map_Board[x,z] = -1;
				map_Board_Unit[x,z] = null;
			}			
		}
	}

	public int GetMapBoard(int _x, int _z){
		return map_Board[_x, _z];
	}
	public void SetMapBoard(int _x, int _z, Unit_Cal _mCube){
		map_Board_Unit[_x, _z] = _mCube;
	}
	public Unit_Cal GetMapBoardUnit(int _x, int _z){
		if(_x < 0 || Constants.MAP_SIZE_X +1 < _x || _z < 0 || Constants.MAP_SIZE_Z +1 < _z){
			return null;
		}
		return map_Board_Unit[_x, _z];
	}

	public int[,] GetMoveArea(int _x, int _y, int _locomotion, string _team){
		int[,] map_Board_Clone = (int[,])map_Board.Clone();
		// 初期を設定
		map_Board_Clone[_x, _y] = _locomotion;
		// 上下左右を調べる。
		if(_team == "My"){
			Search4(ref map_Board_Clone, _x, _y, _locomotion, _team, "right");
			Search4(ref map_Board_Clone, _x, _y, _locomotion, _team, "left");
			Search4(ref map_Board_Clone, _x, _y, _locomotion, _team, "up");
			Search4(ref map_Board_Clone, _x, _y, _locomotion, _team, "down");
		}
		else{
			Search4(ref map_Board_Clone, _x, _y, _locomotion, _team, "left");
			Search4(ref map_Board_Clone, _x, _y, _locomotion, _team, "right");
			Search4(ref map_Board_Clone, _x, _y, _locomotion, _team, "down");
			Search4(ref map_Board_Clone, _x, _y, _locomotion, _team, "up");
		}
		return map_Board_Clone;
	}

	public void Search4(ref int[,] _clone_map, int _x, int _z, int _locomotion, string _team, string _text){
		int tmp_x = _x;
		int tmp_z = _z;
		switch(_text){
			case "up":
			tmp_z++;
			break;
			case "right":
			tmp_x ++;
			break;
			case "down":
			tmp_z--;
			break;
			case "left":
			tmp_x --;
			break;
			default:
			// Debug.Log("Search4エラー");
			break;
		}
		Search(ref _clone_map, tmp_x, tmp_z, _locomotion, _team, _text);
	}
	public void Search(ref int[,] _clone_map, int _x, int _z,  int _locomotion, string _team, string _text){
		// 壁にぶつかるなら終了
		if(_clone_map[_x, _z] == -100){
			return;
		}

		int tmp_loc = _locomotion;

		// 自分の敵なら-10
		if(map_Board_Unit[_x, _z] == null){
			tmp_loc += map_Board[_x, _z]; // 地形の影響を与える
		}
		else if(map_Board_Unit[_x, _z].enemy_team == _team){
			tmp_loc -= 10;
		}
		else if(map_Board_Unit[_x, _z].team == _team){
			tmp_loc += map_Board[_x, _z];
		}
		// 登録したやつより大きければ登録し直す。
		if(tmp_loc > _clone_map[_x, _z]){
			_clone_map[_x, _z] = tmp_loc;
		}
		if(tmp_loc <= 0){
			return;
		}
		if(_text != "left"){
			Search4(ref _clone_map, _x, _z, tmp_loc, _team, "right");
		}
		if(_text != "right"){
			Search4(ref _clone_map, _x, _z, tmp_loc, _team, "left");
		}
		if(_text != "down"){
			Search4(ref _clone_map, _x, _z, tmp_loc, _team, "up");
		}
		if(_text != "up"){
			Search4(ref _clone_map, _x, _z, tmp_loc, _team, "down");
		}
	}
}

public class Unit_Cal{
	public int[] 	xz_position = new int[2];	// 現在地
	public int locomotion;		// 移動力
	public int waitTime_max;			// 待ち時間
	public int waitTime;			// 待ち時間
	public string team;
	public string enemy_team;
	public string type;
	public int _id;

	// public int state; 				// 現在の状態
	public int plan;
	public int filter1;
	public int filter2;
	public int target_condition;
	public int action;
	public Unit_Plans plans;

	public int Max_hp;
	public int hp;
	public int attack; 				// 現在の状態
	public int defense; 				// 現在の状態

	public int heal;
	public int Max_stamina;
	public int stamina;
	public int charge;       // ため攻撃系
	public int guard;       // ため攻撃系
	public int speedUP;       // ため攻撃系

	Dictionarys dic = new Dictionarys();

	public int sum_damage;
	public bool IsAttack;
	public bool IsLongAttack;
	public bool IsHeal;
	public bool IsDamage;
	public bool IsDamaged;
	public bool IsRecovery;
	public bool IsCharge;
	public bool IsCharged;
	public bool IsGuard;
	public bool IsGuarded;
	public bool IsSpeedUP;
	public bool IsSpeedUPed;
	public bool IsS_LongAttack;


	int[,] attackRange;					// 攻撃範囲
	int[,] attackRange_LONG;
	int[,] attackRange_S_LONG;
	int[,] healRange;					// 攻撃範囲
	int[,] supportRange;					// 攻撃範囲

	bool[] plan_eventFlags = new bool[50];

	int plan_eventFlag;
	int connectStartId = -1;

	int[] target_position = new int[2]{-1, -2};
	public Unit_Cal target_unit;

	int[,] point_positions = new int[4,2]{ // 左奥，右奥，左前，右前
			{Constants.MAP_SIZE_X, Constants.MAP_SIZE_Z}, 
			{Constants.MAP_SIZE_X, 1},
			{1, Constants.MAP_SIZE_Z}, 
			{1, 1}
		};

  /*--------
  * 初期設定 *
  ----------*/
	public Unit_Cal(int _x, int _z, int _loc, int _waitTime, Player _player, int _hp, int _attack, int _heal, int _stamina, int _id, Unit_Plans _plans){
		this.xz_position[0] = _x;
		this.xz_position[1] = _z;
		this.locomotion 		= _loc;
		this.waitTime_max		= _waitTime;
		this.waitTime 			= _waitTime;
		this.action 				= Constants.A_WAIT;
		if(_player.color == "RED"){
			this.team = "My";
			this.enemy_team = "Enemy";
		}
		else{
			this.team = "Enemy";			
			this.enemy_team = "My";			
		}
		this.Max_hp 	= _hp;
		this.hp 			= _hp;
		this.attack 	= _attack;
		this.defense 	= 0;
		this.heal 		= _heal;
		this.Max_stamina = _stamina;
		this.stamina 	= _stamina;
		this.type 		= GetType(_id, _player);
		this._id 			= _id;
		this.plans = _plans;

		this.plan 	= 0;
		this.charge = 0;
		this.guard 	= 0;

		if(this.team == "My"){
			attackRange 	= new int[5,2]{{0,0}, {0,1}, {1,0}, {0,-1}, {-1,0}};
			healRange 		= new int[13,2]{{0,2}, {2,0}, {0,-2}, {-2,0}, {0,1}, {1,0}, {0,-1}, {-1,0},{0,0},{1,1},{-1,-1},{1,-1},{-1,1}};
			attackRange_LONG 	= new int[25,2]{
				{0,3},{3,0},{0,-3},{-3,0}, 
			 {0,2}, {2,0}, {0,-2}, {-2,0},
			 {0,1}, {1,0}, {0,-1}, {-1,0},
			 {1,1},{-1,-1},{1,-1},{-1,1},
			 {1,2}, {2,1}, {-1,-2}, {-2,-1},
			 {-1,2}, {-2,1}, {1,-2}, {2,-1},
			 {0,0}};
			supportRange 	= new int[5,2]{{0,0}, {0,1}, {1,0}, {0,-1}, {-1,0}};
			attackRange_S_LONG 	= new int[61,2]{
				{0,5},{5,0},{0,-5},{-5,0}, 
				{0,4},{4,0},{0,-4},{-4,0}, 
				{0,3},{3,0},{0,-3},{-3,0}, 
				{0,2},{2,0},{0,-2},{-2,0}, 
				{0,1},{1,0},{0,-1},{-1,0}, 
			 {1,1},{-1,-1},{1,-1},{-1,1},
			 {1,2}, {2,1}, {-1,-2}, {-2,-1},
			 {-1,2}, {-2,1}, {1,-2}, {2,-1},
			 {2,2},{-2,-2},{2,-2},{-2,2},
			 {1,3},{-1,-3},{1,-3},{-1,3},
			 {3,1},{-3,-1},{3,-1},{-3,1},
			 {1,4},{-1,-4},{1,-4},{-1,4},
			 {4,1},{-4,-1},{4,-1},{-4,1},
			 {2,3},{-2,-3},{2,-3},{-2,3},
			 {3,2},{-3,-2},{3,-2},{-3,2},
			 {0,0}};
		}
		else{
			attackRange 	= new int[5,2]{{0,0}, {0,-1}, {-1,0}, {0,1}, {1,0}};
			healRange 		= new int[13,2]{{0,-2}, {-2,0}, {0,2}, {2,0}, {0,-1}, {-1,0}, {0,1}, {1,0},{0,0},{-1,-1},{1,1},{-1,1},{1,-1}};
			attackRange_LONG 	= new int[25,2]{
				{0,-3},{-3,0},{0,3},{3,0}, 
			 {0,-2}, {-2,0}, {0,2}, {2,0},
			 {0,-1}, {-1,0}, {0,1}, {1,0},
			 {-1,-1},{1,1},{-1,1},{1,-1},
			 {-1,-2}, {-2,-1}, {1,2}, {2,1},
			 {1,-2}, {2,-1}, {-1,2}, {-2,1},
			 {0,0}};
			supportRange 	= new int[5,2]{{0,0}, {0,-1}, {-1,0}, {0,1}, {1,0}};
			attackRange_S_LONG 	= new int[61,2]{
			 {-1,-1},{1,1},{-1,1},{1,-1},
			 {-1,-2}, {-2,-1}, {1,2}, {2,1},
			 {1,-2}, {2,-1}, {-1,2}, {-2,1},
			 {-2,-2},{2,2},{-2,2},{2,-2},
			 {-1,-3},{1,3},{-1,3},{1,-3},
			 {-3,-1},{3,1},{-3,1},{3,-1},
			 {-1,-4},{1,4},{-1,4},{1,-4},
			 {-4,-1},{4,1},{-4,1},{4,-1},
			 {-2,-3},{2,3},{-2,3},{2,-3},
			 {-3,-2},{3,2},{-3,2},{3,-2},
				{0,-5},{-5,0},{0,5},{5,0}, 
				{0,-4},{-4,0},{0,4},{4,0}, 
				{0,-3},{-3,0},{0,3},{3,0}, 
				{0,-2},{-2,0},{0,2},{2,0}, 
				{0,-1},{-1,0},{0,1},{1,0}, 
			 {0,0}};
		}
		sum_damage = 0;
		IsDamage = false;
		IsHeal 	= false;
	}

	string GetType(int _id, Player _player){
		int tmp_type = dic.Chara_Dic[_player.Units[_id]][0];
		switch(tmp_type){
			case 0:
			return "KING";
			case 1:
			return "QUEEN";
			case 2:
			return "KNIGHT";
			case 3:
			return "BISHOP";
			case 4:
			return "PAWN";
			case 5:
			return "ROOK";
			default:
			return "KNIGHT";
		}
	}

  int ChangeSkill(int _origin){
	  if(_origin == Constants.A_SKILL){
	      if(this.type == "KING" || this.type == "BISHOP"){
	          return Constants.A_LONG_ATTACK;
	      }
	      else if(this.type == "QUEEN"){
	          return Constants.A_HEAL;
	      }
	      else if(this.type == "KNIGHT"){
	          return Constants.A_CHARGE;
	      }
	      else if(this.type == "PAWN"){
	          return Constants.A_GUARD;
	          //return Constants.A_SPEEDUP;
	      }
	      else if(this.type == "ROOK"){
	      		return Constants.A_S_LONG_ATTACK;
	      }
	  }
	  else if(_origin == Constants.A_RETREAT){
	  	return Constants.A_WAIT;
	  }
	  return _origin;
	}

	/*---------
	* 状態遷移 *
	---------*/

  // 遷移
  // 満たせば必ず遷移する
  bool IsPlanSelect(int s_plan, int plan_event1,int plan_event2, int e_plan){
  	if(e_plan < 0){
  		return false;
  	}
		if(this.plan != s_plan){
			return false; // 今の場所をリストから探す
		}

  	bool IsCondition1 = false;
  	bool IsCondition2 = false;
		// 無条件遷移も許す
		if(plan_event1 == 0 || plan_eventFlags[plan_event1]){
			IsCondition1 = true;
		}
		if(plan_event2 == 0 || plan_eventFlags[plan_event2]){
			IsCondition2 = true;
		}
		if(IsCondition1 && IsCondition2){
			this.plan = e_plan;
			return true;
		}
		return false;
  }

  bool IsActionSelect(Elements _elements){
  	// Idの場所を取得
  	if(this.plan != _elements.id){
  		return false;      // 次のプラン
  	}
 
  	// ターゲット条件を確認し，アクションを決定する
  	for(int i=0; i<5; i++){
  		if(SetTarget(this.plans.GetPlan(this.plan).elements.GetRow(i)[0], 
  								this.plans.GetPlan(this.plan).elements.GetRow(i)[1], 
  								this.plans.GetPlan(this.plan).elements.GetRow(i)[2],
  								ChangeSkill(this.plans.GetPlan(this.plan).elements.GetRow(i)[3]))
  		){
  			this.filter1 = _elements.GetRow(i)[0];
  			this.filter2 = _elements.GetRow(i)[1];
  			this.target_condition = _elements.GetRow(i)[2];
				this.action = ChangeSkill(_elements.GetRow(i)[3]);
				return true; 
  		}
  	}
  	// どれも当てはまらない
		this.filter1 = 0;
		this.filter2 = 0;
		this.target_condition = 0;
		this.action = 0;
  	return true;
  }

  // 単純に現在のPlanから回す _transitions.id
  void DoTransition(Unit_Plans _plans){// List<int[]> _connectLists, MapBoard _mcBoard){
  	int idx = connectStartId;// 現在のPlanを取得
		if(idx < 0){
  		idx = 0;
  		this.plan = 0; //GetMinPlan();// Constants.P_INIT; // 初期Plan = 1 //NodeArrowは0,1,2,3で登録			
  		if(this._id == 1){
  		}
		}
  	for(int i=0; i<6; i++){
  		int num = (i+idx)%6;
  		for(int j=0; j<5; j++){
  			if(_plans.GetPlan(num).transitions.GetRow(j).Length == 0){
  			}
	      if(IsPlanSelect(_plans.GetPlan(num).transitions.id, _plans.GetPlan(num).transitions.GetRow(j)[0], _plans.GetPlan(num).transitions.GetRow(j)[1], _plans.GetPlan(num).transitions.GetRow(j)[2]-1)){
	      	connectStartId = _plans.GetPlan(num).transitions.GetRow(j)[2]-1;
	      	break;
	      }
  		}
  	}
  	// 条件から行動を選択
  	for(int i=0; i<6; i++){
  		if(IsActionSelect(_plans.GetPlan(i).elements)){
  			break;
  		}
  	}
  	plan_eventFlag = 0;
  	RestFlag();
  }


  void RestFlag(){
  	for(int i=0; i<plan_eventFlags.Length; i++){
	  	plan_eventFlags[i] = false;
  	}
  }

	// もしPointに関することなら 赤＝＞青，緑＝＞黄色
  /*---------
  * イベント *
  ---------*/

	public void SetMEvent(MapBoard_Cal _mcBoard){
		SetMINEHPEvent();// 自分のHPイベント
		SetStaminaEvent(); 											// スタミナイベントの設定
		SetScoreEvent();
		SetTimeEvent();
		SetKINGHPEvent();
		SetPointEvent();
		SetUnitCount();
		DieCountEvent();

		// 状態遷移
		DoTransition(this.plans);
	}

	void SetMINEHPEvent(){
		if(this.Max_hp*75 <= this.hp*100){
			plan_eventFlags[Constants.F_HP_75_100] = true;
		}
		else if(this.Max_hp*50 <= this.hp*100){
			plan_eventFlags[Constants.F_HP_50_75] = true;
		}
		else if(this.Max_hp*25 <= this.hp*100){
			plan_eventFlags[Constants.F_HP_25_50] = true;
		}
		else if(this.Max_hp*0 <= this.hp*100){
			plan_eventFlags[Constants.F_HP_0_25] = true;
		}
	}

	void SetStaminaEvent(){
		if(this.stamina == 4){
			plan_eventFlags[Constants.F_STAMINA_4] = true;
			plan_eventFlags[Constants.F_STAMINA_3_4] = true;
		}
		else if(this.stamina == 3){
			plan_eventFlags[Constants.F_STAMINA_3] = true;
			plan_eventFlags[Constants.F_STAMINA_3_4] = true;
		}
		else if(this.stamina == 2){
			plan_eventFlags[Constants.F_STAMINA_2] = true;
		}
		else if(this.stamina > -1){
			plan_eventFlags[Constants.F_STAMINA_0_1] = true;
		}
	}

	void SetScoreEvent(){
		int red_left = GameMaster.My_Left_Count;
		int blue_left = GameMaster.Enemy_Left_Count;
		int myLeft;
		int EnemyLeft;

		if(this.team == "My"){
			myLeft = red_left;
			EnemyLeft = blue_left;
		}
		else{
			myLeft = blue_left;
			EnemyLeft = red_left;			
		}

		if(myLeft > 4){
			plan_eventFlags[Constants.F_LEFT_FRIEND_5_6] = true;
		}
		else if(myLeft > 2){
			plan_eventFlags[Constants.F_LEFT_FRIEND_3_4] = true;
		}
		else if(myLeft > 0){
			plan_eventFlags[Constants.F_LEFT_FRIEND_1_2] = true;
		}

		if(EnemyLeft > 4){
			plan_eventFlags[Constants.F_LEFT_ENEMY_5_6] = true;
		}
		else if(EnemyLeft > 2){
			plan_eventFlags[Constants.F_LEFT_ENEMY_3_4] = true;
		}
		else if(EnemyLeft > 0){
			plan_eventFlags[Constants.F_LEFT_ENEMY_1_2] = true;
		}
		if(myLeft > EnemyLeft){
			plan_eventFlags[Constants.F_LEFT_FRIEND_OVER_ENEMY] = true;
		}
		else if(EnemyLeft > myLeft){
			plan_eventFlags[Constants.F_LEFT_FRIEND_UNDER_ENEMY] = true;			
		}
		else{
			plan_eventFlags[Constants.F_LEFT_EQUAL] = true;			
		}
	}

	// ユニットの存在を調べる
	void SetKINGHPEvent(){
		List<Unit_Cal> tmp_units = GameMaster.units;
		Unit_Cal my_king = GetUnit(this.team, "KING", tmp_units);
		Unit_Cal enemy_king = GetUnit(this.enemy_team, "KING", tmp_units);

		if(IsHP(enemy_king, 75, "Hight") && IsHP(enemy_king, 100, "Low")){
			plan_eventFlags[Constants.F_E_KING_HP_75_100] = true;
		}
		else if(IsHP(enemy_king, 50, "Hight") && IsHP(enemy_king, 75, "Low")){
			plan_eventFlags[Constants.F_E_KING_HP_50_75] = true;
		}
		else if(IsHP(enemy_king, 25, "Hight") && IsHP(enemy_king, 50, "Low")){
			plan_eventFlags[Constants.F_E_KING_HP_25_50] = true;
		}
		else if(IsHP(enemy_king, 0, "Hight") && IsHP(enemy_king, 25, "Low")){
			plan_eventFlags[Constants.F_E_KING_HP_0_25] = true;
		}

		if(IsHP(my_king, 75, "Hight") && IsHP(my_king, 100, "Low")){
			plan_eventFlags[Constants.F_F_KING_HP_75_100] = true;
		}
		else if(IsHP(my_king, 50, "Hight") && IsHP(my_king, 75, "Low")){
			plan_eventFlags[Constants.F_F_KING_HP_50_75] = true;
		}
		else if(IsHP(my_king, 25, "Hight") && IsHP(my_king, 50, "Low")){
			plan_eventFlags[Constants.F_F_KING_HP_25_50] = true;
		}
		else if(IsHP(my_king, 0, "Hight") && IsHP(my_king, 25, "Low")){
			plan_eventFlags[Constants.F_F_KING_HP_0_25] = true;
		}
		if(my_king.hp > enemy_king.hp){
			plan_eventFlags[Constants.F_F_KING_HP_WIN_E] = true;
		}
		else if(my_king.hp < enemy_king.hp){
			plan_eventFlags[Constants.F_F_KING_HP_LOSE_E] = true;
		}
		else{
			plan_eventFlags[Constants.F_KING_HP_EQUAL] = true;			
		}
	}




	void SetTimeEvent(){
			int tmp_time = GameMaster.times;
			if(150 <= tmp_time && tmp_time <= 200){
				plan_eventFlags[Constants.F_TIME_150_200] = true;
			}
			if(100 <= tmp_time && tmp_time <= 150){
				plan_eventFlags[Constants.F_TIME_100_150] = true;
			}
			if(50 <= tmp_time && tmp_time <= 100){
				plan_eventFlags[Constants.F_TIME_50_100] = true;
			}
			if(0 <= tmp_time && tmp_time <= 50){
				plan_eventFlags[Constants.F_TIME_0_50] = true;
			}
	}

	void SetPointEvent(){
		int red_point = GetManhattanDistance(this.xz_position, new int[2]{this.point_positions[0,0],this.point_positions[0,1]});
		int green_point = GetManhattanDistance(this.xz_position, new int[2]{point_positions[1,0],point_positions[1,1]});
		int yellow_point = GetManhattanDistance(this.xz_position, new int[2]{point_positions[2,0],point_positions[2,1]});
		int blue_point = GetManhattanDistance(this.xz_position, new int[2]{point_positions[3,0],point_positions[3,1]});
		// 敵の場合は点対称にしないといけない
		if(this.team == "Enemy"){
			if(red_point < 2){
				plan_eventFlags[Constants.F_POINT_BLUE] = true;
			}
			if(green_point < 2){
				plan_eventFlags[Constants.F_POINT_YELLOW] = true;
			}
			if(yellow_point < 2){
				plan_eventFlags[Constants.F_POINT_GREEN] = true;
			}
			if(blue_point < 2){
				plan_eventFlags[Constants.F_POINT_RED] = true;
			}
			return;
		}
		if(red_point < 2){
			plan_eventFlags[Constants.F_POINT_RED] = true;
		}
		if(green_point < 2){
			plan_eventFlags[Constants.F_POINT_GREEN] = true;
		}
		if(yellow_point < 2){
			plan_eventFlags[Constants.F_POINT_YELLOW] = true;
		}
		if(blue_point < 2){
			plan_eventFlags[Constants.F_POINT_BLUE] = true;
		}
	}

	// 近くの敵，味方の数
	void SetUnitCount(){
		List<Unit_Cal> tmp_units = GameMaster.units;		
		int tmp_friend_unit_count = 0;
		int tmp_enemy_unit_count 	= 0;
		foreach(Unit_Cal _unit in tmp_units){
			if(_unit == null){
				continue; // 死んでるやつは飛ばす
			}

			if(_unit.team == this.team){
				if(_unit._id == this._id){
					continue;
				}
				// _unitとの距離が_dist以下か？
				if(IsNearUnit(3, _unit)){
					tmp_friend_unit_count++;
				}
			}
			else{
				if(IsNearUnit(3, _unit)){
					tmp_enemy_unit_count++;
				}
			}
		}

		switch(tmp_friend_unit_count){
			case 0:
			plan_eventFlags[Constants.F_UNIT_F_COUNT_0] = true;
			break;
			case 1:
			plan_eventFlags[Constants.F_UNIT_F_COUNT_1] = true;
			break;
			case 2:
			plan_eventFlags[Constants.F_UNIT_F_COUNT_2] = true;
			break;
			case 3:
			plan_eventFlags[Constants.F_UNIT_F_COUNT_3] = true;
			break;
		}
		switch(tmp_enemy_unit_count){
			case 0:
			plan_eventFlags[Constants.F_UNIT_E_COUNT_0] = true;
			break;
			case 1:
			plan_eventFlags[Constants.F_UNIT_E_COUNT_1] = true;
			break;
			case 2:
			plan_eventFlags[Constants.F_UNIT_E_COUNT_2] = true;
			break;
			case 3:
			plan_eventFlags[Constants.F_UNIT_E_COUNT_3] = true;
			break;
			case 4:
			plan_eventFlags[Constants.F_UNIT_E_COUNT_4] = true;
			break;
		}
	}	

	void DieCountEvent(){
		int[] tmp_die_count = GameMaster.Die_Count;
		if(tmp_die_count[this._id] == 0){
			plan_eventFlags[Constants.F_DIE_COUNT_0] = true;
		}
		else{
			plan_eventFlags[Constants.F_DIE_COUNT_NOT_0] = true;
		}
	}


	List<Unit_Cal> GetUnits(string _team, string _type, List<Unit_Cal> _units, int _action){
		List<Unit_Cal> tmp_units = new List<Unit_Cal>();
		foreach(Unit_Cal _unit in _units){
			if(IsTeamMate(_unit) != _team){
				continue;
			}
			if(IsMine(_unit)){
				continue;
			}
			if(_unit.type == "KING" && _action == Constants.A_HEAL){
				continue;
			}
			if(_type == "ALL"){
				tmp_units.Add(_unit);				
			}
			else if(_unit.type == _type){
				 tmp_units.Add(_unit);
			}
		}
		return tmp_units;
	}

	List<Unit_Cal> GetNotUnits(string _type, List<Unit_Cal> _units){
		List<Unit_Cal> tmp_units = new List<Unit_Cal>();
		foreach(Unit_Cal _unit in _units){
			if(_unit == null){
				continue;
			}
			if(IsMine(_unit)){
				continue;
			}
			if(_unit.type != _type){
				 tmp_units.Add(_unit);
			}
		}
		return tmp_units;
	}		


	Unit_Cal GetUnit(string _team , string _type, List<Unit_Cal> _units){
		foreach(Unit_Cal _unit in _units){
			if(IsTeamMate(_unit) != _team){
				continue;
			}
			if(_unit.type == _type){
				return _unit;
			}
		}
		return null;
	}

	// ユニットのHPが??以下であるかどうか
	bool IsHP(Unit_Cal _unit, int _par, string _type){
		if(_type == "Low"){
			if(_unit.Max_hp*_par >= _unit.hp*100){
				return true;
			}
		}
		else if(_type == "Hight"){
			if(_unit.Max_hp*_par <= _unit.hp*100){
				return true;
			}
		}
		return false;
	}
	// ユニットのHPが??以下であるかどうか
	bool IsStamina(Unit_Cal _unit, int _value, string _type){
		if(_type == "Low"){
			if(_unit.stamina <= _value){
				return true;
			}
		}
		else if(_type == "Hight"){
			if(_value <= _unit.stamina){
				return true;
			}
		}
		return false;
	}

	bool IsMine(Unit_Cal _unit){
		if(IsTeamMate(_unit) == this.team){
			if(_unit._id == this._id){
				return true;
			}
		}
		return false;
	}
	/*------イベント終了------*/



	/*----------
	*移動フェイズ*
	-----------*/
	public void Move(MapBoard_Cal _mcBoard){
		// キングなら移動しない	
		if(this.type == "KING"){
			return;
		}
		// 待機は移動しない
		if(this.action == Constants.A_WAIT){
			return;
		}
		ViewArea(_mcBoard);
	}	

	// 範囲内にポイントがあるかどうか
	bool IsNear4Point(int _filter, int[] _point){
		switch(_filter){
			case Constants.FI_DIST_1:
			return IsNearPoint("UNDER", 1, _point);
			case Constants.FI_DIST_4:
			return IsNearPoint("UNDER", 4, _point);
			case Constants.FI_DIST_7:
			return IsNearPoint("UNDER", 7, _point);
			case Constants.FI_DIST_ALL:
			return true;
			case Constants.FI_DIST_2_OVER:
			return IsNearPoint("OVER", 2, _point);
			case Constants.FI_DIST_5_OVER:
			return IsNearPoint("OVER", 5, _point);
			case Constants.FI_DIST_8_OVER:
			return IsNearPoint("OVER", 8, _point);
			return true;

			default:
			return true;
		}
	}
	bool IsNearPoint(string _type, int _dist, int[] _point){
		int tmp_dist = GetManhattanDistance(this.xz_position, _point);
		if(_type == "UNDER"){
			if(tmp_dist <= _dist){
				return true;
			}
		}
		else if(_type == "OVER"){
			if(tmp_dist >= _dist){
				return true;
			}
		}
		return false;
	}

	int GetPointChange(string _team, int _target_condition){
		if(_team == "Enemy"){
			switch(_target_condition){
				case Constants.T_POINT_0:
				return Constants.T_POINT_1;
				case Constants.T_POINT_1:
				return Constants.T_POINT_0;
				case Constants.T_POINT_2:
				return Constants.T_POINT_3;
				case Constants.T_POINT_3:
				return Constants.T_POINT_2;
				case Constants.T_E_UNIT_UP:
				return Constants.T_E_UNIT_DOWN;
				case Constants.T_E_UNIT_DOWN:
				return Constants.T_E_UNIT_UP;
				case Constants.T_F_UNIT_UP:
				return Constants.T_F_UNIT_DOWN;
				case Constants.T_F_UNIT_DOWN:
				return Constants.T_F_UNIT_UP;
				default:
				return _target_condition;
			}
		}
		return _target_condition;
	}

	List<Unit_Cal> TargetFilter(int _target_condition , int _action){
		List<Unit_Cal> tmp_units = new List<Unit_Cal>();
		List<Unit_Cal> tmp_units_g = GameMaster.units;		
		switch(_target_condition){
			case Constants.T_E_KING:
			tmp_units = GetUnits(this.enemy_team, "KING", tmp_units_g, _action);
			break;
			case Constants.T_E_QUEEN:
			tmp_units = GetUnits(this.enemy_team, "QUEEN", tmp_units_g, _action);
			break;
			case Constants.T_E_KNIGHT:
			tmp_units = GetUnits(this.enemy_team, "KNIGHT", tmp_units_g, _action);
			break;
			case Constants.T_E_BISHOP:
			tmp_units = GetUnits(this.enemy_team, "BISHOP", tmp_units_g, _action);
			break;
			case Constants.T_E_PAWN:
			tmp_units = GetUnits(this.enemy_team, "PAWN", tmp_units_g, _action);
			break;
			case Constants.T_E_ROOK:
			tmp_units = GetUnits(this.enemy_team, "ROOK", tmp_units_g, _action);
			break;
			case Constants.T_F_KING: // キングの回復を除外
			tmp_units = GetUnits(this.team, "KING", tmp_units_g, _action);
			break;
			case Constants.T_F_QUEEN:
			tmp_units = GetUnits(this.team, "QUEEN", tmp_units_g, _action);
			break;
			case Constants.T_F_KNIGHT:
			tmp_units = GetUnits(this.team, "KNIGHT", tmp_units_g, _action);
			break;
			case Constants.T_F_BISHOP:
			tmp_units = GetUnits(this.team, "BISHOP", tmp_units_g, _action);
			break;
			case Constants.T_F_PAWN:
			tmp_units = GetUnits(this.team, "PAWN", tmp_units_g, _action);
			break;
			case Constants.T_F_ROOK:
			tmp_units = GetUnits(this.team, "ROOK", tmp_units_g, _action);
			break;
			case Constants.T_E_UNIT:
			tmp_units = GetUnits(this.enemy_team, "ALL", tmp_units_g, _action);
			break;
			case Constants.T_E_UNIT_UP:
			tmp_units.Add(GetFilToUnit(this.enemy_team, "上", tmp_units_g, _action));
			break;
			case Constants.T_E_UNIT_CENTER:
			tmp_units.Add(GetFilToUnit(this.enemy_team, "中", tmp_units_g, _action));
			break;
			case Constants.T_E_UNIT_DOWN:
			tmp_units.Add(GetFilToUnit(this.enemy_team, "下", tmp_units_g, _action));
			break;
			case Constants.T_F_UNIT: // キングの回復を除外
			tmp_units = GetUnits(this.team, "ALL", tmp_units_g, _action);
			break;
			case Constants.T_F_UNIT_UP:
			tmp_units.Add(GetFilToUnit(this.team, "上", tmp_units_g, _action));
			break;
			case Constants.T_F_UNIT_CENTER:
			tmp_units.Add(GetFilToUnit(this.team, "中", tmp_units_g, _action));
			break;
			case Constants.T_F_UNIT_DOWN:
			tmp_units.Add(GetFilToUnit(this.team, "下", tmp_units_g, _action));
			break;
			case Constants.T_MINE:
			tmp_units.Add(GetFilToUnit(this.team, "MINE", tmp_units_g, _action));
			break;			
			default:
			break;
		}
		return tmp_units;
	}


	public bool SetTarget(int _filter1, int _filter2, int _target_condition, int _action){
		Unit_Cal _target = null;
		List<Unit_Cal> target_filter_units = new List<Unit_Cal>();

		_target_condition = GetPointChange(this.team, _target_condition);
		// ターゲットから絞る
		// ポイントなら終了
		switch(_target_condition){
			case Constants.T_POINT_0:
			int[] tmp_point0 = new int[2]{point_positions[0,0], point_positions[0,1]};
			if(IsNear4Point(_filter1, tmp_point0)
				&& IsNear4Point(_filter2, tmp_point0)){
				target_position[0] = point_positions[0,0];// 赤
				target_position[1] = point_positions[0,1];
				target_unit = null;
				return true;
			}
			break;
			case Constants.T_POINT_3:
			int[] tmp_point1 = new int[2]{point_positions[1,0], point_positions[1,1]};
			if(IsNear4Point(_filter1, tmp_point1)
				&& IsNear4Point(_filter2, tmp_point1)){
				target_position[0] = point_positions[1,0];// 緑
				target_position[1] = point_positions[1,1];
				target_unit = null;
				return true;
			}
			break;
			case Constants.T_POINT_2:
			int[] tmp_point2 = new int[2]{point_positions[2,0], point_positions[2,1]};
			if(IsNear4Point(_filter1, tmp_point2)
				&& IsNear4Point(_filter2, tmp_point2)){
				target_position[0] = point_positions[2,0];//黄
				target_position[1] = point_positions[2,1];
				target_unit = null;
				return true;
			}
			break;
			case Constants.T_POINT_1:
			int[] tmp_point3 = new int[2]{point_positions[3,0], point_positions[3,1]};
			if(IsNear4Point(_filter1, tmp_point3)
				&& IsNear4Point(_filter2, tmp_point3)){
				target_position[0] = point_positions[3,0];//青
				target_position[1] = point_positions[3,1];
				target_unit = null;
				return true;
			}
			break;
			default:
			// ポイント以外ならターゲットを絞る
			target_filter_units = TargetFilter(_target_condition, _action);
			break;
		}

		List<Unit_Cal> filter1_units = GetFilUnit(_filter1, target_filter_units, _target_condition, _action);
		List<Unit_Cal> filter2_units = GetFilUnit(_filter2, filter1_units, _target_condition, _action);
		_target = GetNearUnit_Last(filter2_units, this, _action);
		// 絞り込んだ中で最も近いユニット
		target_unit = _target;
		if(_target == null){
			target_position[0] = -1;
			target_position[1] = -1;
			return false;
		}
		else{
			target_position[0] = _target.xz_position[0];
			target_position[1] = _target.xz_position[1];			
			//if(this.type == "BISHOP"){
			//Debug.Log("("+target_position[0]+","+target_position[1]+")");
			//}
			return true;
		}		
	}



	Unit_Cal GetFilToUnit(string _team, string _type, List<Unit_Cal> _units, int _action){
		foreach(Unit_Cal _unit in _units){
			// 指定のチームでない
			if(IsTeamMate(_unit) != _team){
				continue;
			}
			// キングは回復しない
			if(_unit.type == "KING" && _action == Constants.A_HEAL){
				continue; //　キングを回復はしない
			}

			// 味方の上
			if(_type == "上" && _unit._id == 2 && _team == "My"){
				return _unit;
			}
			else if(_type == "上" && _unit._id == 3 && _team == "Enemy"){
				return _unit;
			}
			if(_type == "中" && _unit._id == 1){
				return _unit;
			}
			if(_type == "下" && _unit._id == 3 && _team == "My"){
				return _unit;
			}
			else if(_type == "下" && _unit._id == 2 && _team == "Enemy"){
				return _unit;
			}
			// 自分自身
			if(_type == "MINE" && _unit._id == this._id){
				return this;
			}

			// 指定のタイプで，自分自身は外す。　相手チームならok
			if((_unit.type == _type && _unit._id != this._id)
				|| (_unit.type == _type && _team != this.team)){
				return _unit;
			}
		}
		return null;
	}

	// filterにかければ
	List<Unit_Cal> GetFilUnit(int _filter, List<Unit_Cal> _units, int _target_condition, int _action){
		List<Unit_Cal> tmp_units = new List<Unit_Cal>();
		switch(_filter){
			case Constants.FI_DIST_1:
			tmp_units = GetFilDIstUnit(1, "NEAR", _units, _target_condition);
			break;
			case Constants.FI_DIST_4:
			tmp_units = GetFilDIstUnit(4, "NEAR", _units, _target_condition);
			break;			
			case Constants.FI_DIST_7:
			tmp_units = GetFilDIstUnit(7, "NEAR", _units, _target_condition);
			break;
			case Constants.FI_DIST_ALL:
			tmp_units = GetFilDIstUnit(100, "NEAR", _units, _target_condition);
			break;
			case Constants.FI_DIST_8_OVER:
			tmp_units = GetFilDIstUnit(8, "OVER", _units, _target_condition);
			break;
			case Constants.FI_DIST_5_OVER:
			tmp_units = GetFilDIstUnit(5, "OVER", _units, _target_condition);
			break;
			case Constants.FI_DIST_2_OVER:
			tmp_units = GetFilDIstUnit(2, "OVER", _units, _target_condition);
			break;

			case Constants.FI_HP_0_25:
			tmp_units = GetHpFilterUnit(0, 25, _units);
			break;
			case Constants.FI_HP_25_50:
			tmp_units = GetHpFilterUnit(25, 50, _units);
			break;
			case Constants.FI_HP_50_75:
			tmp_units = GetHpFilterUnit(50, 75, _units);
			break;
			case Constants.FI_HP_75_100:
			tmp_units = GetHpFilterUnit(75, 100, _units);
			break;
			case Constants.FI_HP_0_75:
			tmp_units = GetHpFilterUnit(0, 75, _units);
			break;
			case Constants.FI_HP_25_100:
			tmp_units = GetHpFilterUnit(25, 100, _units);
			break;
			case Constants.FI_STAMINA_4:
			tmp_units = GetStaminaFilterUnit(4, _units);
			break;
			case Constants.FI_STAMINA_3:
			tmp_units = GetStaminaFilterUnit(3, _units);
			break;
			case Constants.FI_STAMINA_2:
			tmp_units = GetStaminaFilterUnit(2, _units);
			break;
			case Constants.FI_STAMINA_1:
			tmp_units = GetStaminaFilterUnit(1, _units);
			break;
			case Constants.FI_LOW_HP:
			tmp_units.Add(GetMinHPUnit(this.team, _units));
			tmp_units.Add(GetMinHPUnit(this.enemy_team, _units));
			break;
			case Constants.FI_NEAR_DIS: // キングの回復時は選択しない？
			tmp_units.Add(GetNearUnit(this.team, _units, this, _action));
			tmp_units.Add(GetNearUnit(this.enemy_team, _units, this, _action));
			break;
			case Constants.FI_NON_CHERGE:
			tmp_units = GetNonPowerUp(Constants.FI_NON_CHERGE, _units);
			break;
			case Constants.FI_NON_SPEEDUP:
			tmp_units = GetNonPowerUp(Constants.FI_NON_SPEEDUP, _units);
			break;
			case Constants.FI_NON_GUARD:
			tmp_units = GetNonPowerUp(Constants.FI_NON_GUARD, _units);
			break;

			case Constants.FI_NOT_KING:
			tmp_units = GetNotUnits("KING", _units);
			break;
			case Constants.FI_NOT_QUEEN:
			tmp_units = GetNotUnits("QUEEN", _units);
			break;
			case Constants.FI_NOT_KNIGHT:
			tmp_units = GetNotUnits("KNIGHT", _units);
			break;
			case Constants.FI_NOT_BISHOP:
			tmp_units = GetNotUnits("BISHOP", _units);
			break;
			case Constants.FI_NOT_PAWN:
			tmp_units = GetNotUnits("PAWN", _units);
			break;
			case Constants.FI_NOT_ROOK:
			tmp_units = GetNotUnits("ROOK", _units);
			break;


			case Constants.FI_DIE_COUNT_0:
			tmp_units = GetDieCount(true, 0, _units);
			break;
			case Constants.FI_DIE_COUNT_NOT_0:
			tmp_units = GetDieCount(false, 0, _units);
			break;
			default:
			tmp_units = _units;
			break;
		}
		return tmp_units;
	}

	List<Unit_Cal> GetDieCount(bool _bool, int _count, List<Unit_Cal> _units){
		int[] tmp_die_count = GameMaster.Die_Count;
		List<Unit_Cal> tmp_units = new List<Unit_Cal>();
		foreach(Unit_Cal _unit in _units){
			if(_unit == null){
				continue;
			}
			int tmp_id = 0;
			if(_unit.team == "My"){
				tmp_id = _unit._id;
			}
			else{
				tmp_id = _unit._id+4;
			}
			if(_bool){
				if(tmp_die_count[tmp_id] == _count){
					tmp_units.Add(_unit);
				}				
			}
			else{
				if(tmp_die_count[tmp_id] != _count){
					tmp_units.Add(_unit);
				}				
			}
		}
		return tmp_units;
	}	

	// _unitsの中からHp条件にあうunitsをかえす
	List<Unit_Cal> GetHpFilterUnit(int _min, int _max, List<Unit_Cal> _units){
		List<Unit_Cal> tmp_units = new List<Unit_Cal>();
		foreach(Unit_Cal _unit in _units){
			if(_unit == null){
				continue;
			}
			if(_min*_unit.Max_hp <= 100*_unit.hp && 100*_unit.hp <= _max*_unit.Max_hp){
				tmp_units.Add(_unit);
			}
		}
		return tmp_units;
	}

	List<Unit_Cal> GetStaminaFilterUnit(int _value, List<Unit_Cal> _units){
		List<Unit_Cal> tmp_units = new List<Unit_Cal>();
		foreach(Unit_Cal _unit in _units){
			if(_unit == null){
				continue;
			}
			if(_unit.stamina == _value){
				tmp_units.Add(_unit);
			}
		}
		return tmp_units;
	}

	// 距離感で判断？　歩数で判断？
	bool IsNearUnit(int _dist, Unit_Cal _unit){
		if(_unit == null){
			return false;
		}
		int tmp_dist = GetManhattanDistance(_unit.xz_position, this.xz_position);
		if(tmp_dist <= _dist){
			return true;
		}
		return false;
	}

	// 距離感で判断？　歩数で判断？
	bool IsOverUnit(int _dist, Unit_Cal _unit){
		if(_unit == null){
			return false;
		}
		int tmp_dist = GetManhattanDistance(_unit.xz_position, this.xz_position);
		if(tmp_dist >= _dist){
			return true;
		}
		return false;
	}	

	List<Unit_Cal> GetFilDIstUnit(int _dist, string _type, List<Unit_Cal> _units, int _target_condition){
		List<Unit_Cal> tmp_units = new List<Unit_Cal>();		
		foreach(Unit_Cal _unit in _units){
			/*
			if(IsTeamMate(_unit) == this.team){
				if(_unit.type == this.type){
					if(_target_condition == Constants.T_MINE){
					}
					else{
						continue;
					}
				}
			}*/
			if(_unit == null){
				continue;
			}
			if(_type == "NEAR"){
				if(_dist == 100){
					tmp_units.Add(_unit);				
				}
				else if(IsNearUnit(_dist, _unit)){//自分自身との距離が指定以下か
					tmp_units.Add(_unit);
				}
			}
			else if(_type == "OVER"){
				if(IsOverUnit(_dist, _unit)){//自分自身との距離が指定以下か
					tmp_units.Add(_unit);
				}
			}
		}
		return tmp_units;
	}
	List<Unit_Cal> GetNonPowerUp(int _const_num, List<Unit_Cal> _units){		
		List<Unit_Cal> tmp_units = new List<Unit_Cal>();
		foreach(Unit_Cal _unit in _units){
			if(_unit == null){
				continue;
			}
			switch(_const_num){
				case Constants.FI_NON_CHERGE:
				if(_unit.charge == 0){
					tmp_units.Add(_unit);
				}
				break;
				case Constants.FI_NON_SPEEDUP:
				if(_unit.speedUP == 0){
					tmp_units.Add(_unit);
				}
				break;
				case Constants.FI_NON_GUARD:
				if(_unit.guard == 0){
					tmp_units.Add(_unit);
				}
				break;
			}
		}
		return tmp_units;
	}

	// 近いUnit  ただし自分は外す
	Unit_Cal GetNearUnit(string _team, List<Unit_Cal> _units, Unit_Cal s_unit, int _action){
		int min_dist = 10000;
		Unit_Cal min_target = null;

		foreach(Unit_Cal _unit in _units){
			if(IsTeamMate(_unit) != _team){
				continue;
			}
			if(IsMine(_unit)){
				continue;
			}
			if(_unit.type == "KING" && _action == Constants.A_HEAL){// && this.team == _team){
				continue; // 自分自身のキングの回復はしない
			}			

			int dist = GetManhattanDistance(s_unit.xz_position, _unit.xz_position);
			if(min_dist>dist){
				min_dist = dist;
				min_target = _unit;
			}
		}
		return min_target;
	}

	// 近いUnit  ただし自分は外す
	Unit_Cal GetNearUnit_Last(List<Unit_Cal> _units, Unit_Cal s_unit, int _action){
		int min_dist = 10000;
		Unit_Cal min_target = null;

		foreach(Unit_Cal _unit in _units){
			if(_unit == null){
				continue;
			}
			if(_unit.type == "KING" && _action == Constants.A_HEAL){
				continue; // 自分自身のキングの回復はしない
			}			

			int dist = GetManhattanDistance(s_unit.xz_position, _unit.xz_position);
			if(min_dist>dist){
				min_dist = dist;
				min_target = _unit;
			}
		}
		return min_target;
	}
	Unit_Cal GetMinHPUnit(string _team, List<Unit_Cal> _units){
		int min_hp = 10000;
		Unit_Cal min_target = null;
		foreach(Unit_Cal _unit in _units){
			if(IsTeamMate(_unit) != _team){
				continue;
			}
			if(IsMine(_unit)){
				continue;
			}
			int tmp_hp = _unit.hp;
			if(min_hp>tmp_hp){
				min_hp = tmp_hp;
				min_target = _unit;
			}
		}
		return min_target;
	}

	string IsTeamMate(Unit_Cal _mCube){
		if(_mCube == null){
			return "Empty";
		}
		return _mCube.team;
	}
	/*-------ターゲットの決定終了---------*/


	/*-------移動描画------*/
	// 評価関数

	int IsGoodValue(int _action, int _top_value,int _compar_value){
		switch(_action){
			case Constants.A_ATTACK:
			case Constants.A_HEAL:
			case Constants.A_MOVE_TO:
			if(this.target_condition == Constants.T_MINE){
				return -1*IsSmallValue(_top_value, _compar_value);
			}
			return IsSmallValue(_top_value, _compar_value);
			case Constants.A_MOVE_FROM:
			case Constants.A_RETREAT:
			return -1*IsSmallValue(_top_value, _compar_value);
			default:
			return IsSmallValue(_top_value, _compar_value);
		}
	}

	// 0=同じ, 1=小さい＝近い， -1=大きい＝遠い
	int IsSmallValue(int _top_value,int _compar_value){
		if(_top_value == _compar_value){//同じならランダム
			return 0; 
		}
		else if(_top_value > _compar_value){
			return 1;
		}
		else{
			return -1;
		}
	}



	int SetTopValue(int _action){
		switch(_action){
			case Constants.A_WAIT:
			case Constants.A_ATTACK:
			case Constants.A_HEAL:
			case Constants.A_MOVE_TO:
			if(this.target_condition == Constants.T_MINE){
				return -1000;
			}
			return 1000000;
			case Constants.A_MOVE_FROM:
			case Constants.A_RETREAT:
			return -1000;
			default:
			return 1000000;
		}
	}

	/*
	target_positionを決定する。
	ターゲットの４方で空いている場所でもっとも近い場所を取得。
	なければターゲット自身を選ぶ
	*/

	int GetEvaluationDist(int _action, int _i, int _j, int[] _target_position){
		int tmp_dist_target = distancePoint(new int[2]{_i,_j}, _target_position);
		int tmp_dist_me = distancePoint(xz_position, new int[2]{_i,_j});
		if(_target_position[0] == -1){
			tmp_dist_target = 0;
		}

		if(_action == Constants.A_HEAL && this.target_condition == Constants.T_MINE){
			return GetAllEnemyUnitDistance(new int[2]{_i,_j}); //全ての敵からの距離			
		}

		switch(_action){
			case Constants.A_ATTACK:
			case Constants.A_HEAL:
			case Constants.A_MOVE_FROM:
			case Constants.A_MOVE_TO:
			return tmp_dist_target*6+tmp_dist_me;
			case Constants.A_RETREAT:
			return GetAllEnemyUnitDistance(new int[2]{_i,_j}); //全ての敵からの距離
			case Constants.A_S_LONG_ATTACK:
			int gmd = GetManhattanDistance(new int[2]{_i,_j}, _target_position);
			tmp_dist_target = 2*gmd*gmd - tmp_dist_target;
			return tmp_dist_target*6+tmp_dist_me;
			default:
			return tmp_dist_target*6+tmp_dist_me;
		}	}

	int GetLocomotion(int _action){
		if(this.stamina <= 1){
			return 1;
		}
		return this.locomotion;
	}

	int GetAllEnemyUnitDistance(int[] _start){
		int sum_dist = 0;
		foreach(Unit_Cal unit in GameMaster.units){
			if(IsTeamMate(unit) == this.enemy_team){
				sum_dist += GetManhattanDistance(_start, unit.xz_position);
			}
		}
		return sum_dist;
	}

	int distancePoint(int[] _next, int[] _target){
		switch(this.target_condition){
			case Constants.T_POINT_0:
			case Constants.T_POINT_1:
			case Constants.T_POINT_2:
			case Constants.T_POINT_3:
			if(IsNearWall(this.xz_position[0], this.xz_position[1])){
				return (_target[0] - _next[0])*(_target[0] - _next[0]) + (_target[1] - _next[1])*(_target[1] - _next[1]);
			}
			return (_target[0] - _next[0])*(_target[0] - _next[0]) + (Constants.MAP_SIZE_X)*(Constants.MAP_SIZE_X)*(_target[1] - _next[1])*(_target[1] - _next[1]);
			default:
			return (_target[0] - _next[0])*(_target[0] - _next[0]) + (_target[1] - _next[1])*(_target[1] - _next[1]);
		}
	}

	bool IsNearWall(int x, int z){
		if(x==1 || x==Constants.MAP_SIZE_X || z==1 || z == Constants.MAP_SIZE_Z){
			return true;
		}
		return false;
	}


	bool IsWall(int x, int z){
		if(x==0 || x==Constants.MAP_SIZE_X+1 || z==0 || z == Constants.MAP_SIZE_Z+1){
			return true;
		}
		if((x==1 && z==1) || (x==Constants.MAP_SIZE_X && z == Constants.MAP_SIZE_Z)
			|| (x==1 && z == Constants.MAP_SIZE_Z) || (x==Constants.MAP_SIZE_X && z == 1)){
			return true;
		}
		if(x == 3 && z == (Constants.MAP_SIZE_Z+1)/2
			|| x == 5 && z == (Constants.MAP_SIZE_Z+1)/2
			|| x == 5 && z == (Constants.MAP_SIZE_Z+1)/2 -2
			|| x == 5 && z == (Constants.MAP_SIZE_Z+1)/2 +2
			|| x == Constants.MAP_SIZE_X+1 -3 && z == (Constants.MAP_SIZE_Z+1)/2
			|| x == Constants.MAP_SIZE_X+1 -5 && z == (Constants.MAP_SIZE_Z+1)/2
			|| x == Constants.MAP_SIZE_X+1 -5 && z == (Constants.MAP_SIZE_Z+1)/2 - 2
			|| x == Constants.MAP_SIZE_X+1 -5 && z == (Constants.MAP_SIZE_Z+1)/2 + 2){
			return true;
		}
		return false;
	}
	public void ViewArea(MapBoard_Cal _mcBoard){
		/*----- 移動場所の決定 ------*/
		int top_value = SetTopValue(this.action); // アクションに応じて　近くのか離れるのかを決める 近くなら初期距離 10000　離れるなら初期距離 -1000
		int[] next_position = new int[2]{xz_position[0],xz_position[1]};
		int[,] marea = _mcBoard.GetMoveArea(xz_position[0],xz_position[1], GetLocomotion(this.action), this.team);
		// 自分自身をチャージするなら動かない
		if(this.action == Constants.A_CHARGE && this.target_unit._id == this._id && this.target_unit.team == this.team){
		}
		else{
			int[] tmp_target = new int[2]{target_position[0], target_position[1]};
			// 対象が1ます以内なら四方をターゲットにしない
			if(distancePoint(xz_position, target_position)<=1){
			}
			else{
				// 4方向で行ける場所
				int[,] arround_point = new int[4,2]{
					{0,-1},
					{0,1},
					{1,0},
					{-1,0},
				};
				if(this.team == "Enemy"){
					arround_point = new int[4,2]{
										{0,1},
										{0,-1},
										{-1,0},
										{1,0},
									};
				}
				switch(this.target_condition){
					case Constants.T_POINT_0:
					case Constants.T_POINT_1:
					case Constants.T_POINT_2:
					case Constants.T_POINT_3:
					if(this.team == "My"){
						arround_point = new int[4,2]{
							{1,0},
							{-1,0},
							{0,-1},
							{0,1},
						};						
					}
					else if(this.team == "Enemy"){
						arround_point = new int[4,2]{
							{-1,0},
							{1,0},
							{0,1},
							{0,-1},
						};
					}
					break;
					default:
					break;
				}				

				int min_dist_arround = 10000;
				for(int i=0; i<arround_point.GetLength(0); i++){
					int tmp_x_dist = target_position[0]+ arround_point[i,0];
					int tmp_z_dist = target_position[1]+ arround_point[i,1];
					if(IsWall(tmp_x_dist, tmp_z_dist)){
						continue;//壁
					}
					if(IsTeamMate(_mcBoard.GetMapBoardUnit(tmp_x_dist, tmp_z_dist)) != "Empty"){
						continue;//ユニットで埋まっている
					}
					// 相手いてい，もっとも近い場所に移動
					int tmp_dist_arround = GetManhattanDistance(xz_position, new int[2]{tmp_x_dist, tmp_z_dist});
					if(min_dist_arround>tmp_dist_arround){
						min_dist_arround = tmp_dist_arround;
						tmp_target = new int[2]{tmp_x_dist, tmp_z_dist};
					}
				}				
			}
			// 対象が1マス以内ならその場で。　ただし，離れる処理はする
			if(distancePoint(xz_position, target_position)<=1  && this.action != Constants.A_MOVE_FROM && this.action != Constants.A_LONG_ATTACK && this.action != Constants.A_S_LONG_ATTACK){
			}
			else{
				for(int i_x=0; i_x<marea.GetLength(0); i_x++){
					for(int j_y=0; j_y<marea.GetLength(1); j_y++){
						int i = i_x;
						int j = j_y;
						if(this.team == "My"){
						}
						else{
							i = marea.GetLength(0) - i_x -1;
							j = marea.GetLength(1) - j_y -1;
						}
						if(marea[i,j] >= 0){
							// 仲間なら飛ばす 自分なら飛ばさない
							if(IsTeamMate(_mcBoard.GetMapBoardUnit(i,j)) == this.team && !(xz_position[0] == i && xz_position[1] == j)){
							// if(IsTeamMate(_mcBoard.GetMapBoardUnit(i,j)) == this.team){
								continue;
							}

							if(this.action == Constants.A_S_LONG_ATTACK && this.target_unit != null){
								// 遠距離攻撃なら離れる
								if(GetManhattanDistance(this.target_unit.xz_position, new int[2]{i,j}) < 5){
									continue;
								}
							}
							else if(this.action == Constants.A_LONG_ATTACK && this.target_unit != null){
								// 遠距離攻撃なら離れる
								if(GetManhattanDistance(this.target_unit.xz_position, new int[2]{i,j}) < 3){
									continue;
								}
							}

							// 評価関数
							int tmp_dist = GetEvaluationDist(this.action, i,j, tmp_target);
							// 目的に地に対して 他の場所が今の場所より遠いなら今の場所がいい。

							if(IsGoodValue(this.action, top_value, tmp_dist) == 0){
								top_value = tmp_dist;
								next_position = new int[2]{i,j};
							}
							else if(IsGoodValue(this.action, top_value, tmp_dist) == 1){
								top_value = tmp_dist;
								next_position = new int[2]{i,j};
							}
						}
					}
				}
			}
		}
		_mcBoard.SetMapBoard(xz_position[0],xz_position[1], null); 			// 今の場所をボードから消す

		/*----- 移動場所の決定終了 ------*/
		List<string> path = GetPath(marea, next_position[0], next_position[1]);		
		if(path.Count > 0){
			float sum_time = 0.1f;
			float play_time = 0.3f;
			// Vector3[] path_array =  new Vector3[path.Count];

			for(int i=0; i<path.Count; i++){
				if(i==0){
					GetRoot(path[i]);
				}
				else{
					GetRoot(path[i]);
				}
				play_time += 0.2f;
			}
			/*--- 移動場所までのパス取得終了 -----*/

			/*--- 移動アニメーション -----*/
		}

		_mcBoard.SetMapBoard(xz_position[0],xz_position[1], this);

	}

	// パスの取得 & 移動の更新
	void GetRoot(string _direction){
		switch(_direction){
			case "上":
			this.xz_position[1]++;
			return;// Vector3.forward*Constants.BLOCK_SIZE;
			case "右":
			this.xz_position[0]++;
			return;// Vector3.right*Constants.BLOCK_SIZE;
			case "下":
			this.xz_position[1]--;
			return;// Vector3.back*Constants.BLOCK_SIZE;
			case "左":
			this.xz_position[0]--;
			return;// Vector3.left*Constants.BLOCK_SIZE;
			default:
			return;// Vector3.zero;
		}
	}	

	List<string> GetPath(int[,] _marea, int _target_x, int _target_z){
		List<string> tmp_path_text = new List<string>();
		int count = 0;
		int max = _marea[_target_x, _target_z];
		while(max < GetLocomotion(this.action)){
			count ++ ;
			if(count >2000){
				break;
			}
			int[,] arround = new int[4,2]{{1,0}, {-1,0}, {0,1}, {0,-1}};
			string[] arround_text = new string[4]{"左","右","下","上"};
			for(int i=0; i<arround.GetLength(0); i++){
				int tmp_x = _target_x + arround[i, 0];
				int tmp_z = _target_z + arround[i, 1];

				int value = _marea[tmp_x, tmp_z];
				if(value > max){
					max = value;
					tmp_path_text.Add(arround_text[i]);
					_target_x = tmp_x;
					_target_z = tmp_z;
				}
			}
		}
		tmp_path_text.Reverse();
		return tmp_path_text;// list[list.Count-1];
	}

	int GetManhattanDistance(int[] _start, int[] _end){
		return Mathf.Abs(_start[0] - _end[0]) + Mathf.Abs(_start[1] - _end[1]);
	}


	/*-------バトルフェイズ　計算-----*/
	public void Battle(MapBoard_Cal _mcBoard){
		switch(this.action){
			case Constants.A_ATTACK:
			Attack(_mcBoard, "Attack");
			break;
			case Constants.A_LONG_ATTACK:
			if(this.type == "KING" || this.type == "BISHOP"){
				Attack(_mcBoard, "Long_Attack");
			}
			break;
			case Constants.A_HEAL:
			if(this.type == "QUEEN"){
				Heal(_mcBoard);
			}
			break;
			case Constants.A_CHARGE:
			if(this.type == "KNIGHT"){
				Charge(_mcBoard);
			}
			break;
			//case Constants.A_SPEEDUP:
			case Constants.A_GUARD:
			if(this.type == "PAWN"){
				Guard(_mcBoard);
			}
			break;
			case Constants.A_S_LONG_ATTACK:
			if(this.type == "ROOK"){
				Attack(_mcBoard, "S_Long_Attack");
			}
			break;
			case Constants.A_WAIT:
			case Constants.A_MOVE_TO:
			case Constants.A_MOVE_FROM:
			case Constants.A_RETREAT:
			break;
		}
	}


	void Attack(MapBoard_Cal _mcBoard, string _type){
		if(this.target_unit == null ){ // ターゲットがいないなら近くの敵を攻撃
			return;
		}
		else{
			if(!IsSkill(_type, _mcBoard)){ // ターゲットはいるけど攻撃が成功しないなら　近くの敵を攻撃
				return;
			}			
			// ダメージ計算をして終了
			int tmp_attack = this.attack;
			if(_type == "Long_Attack"){
				tmp_attack -= 10;
				this.IsLongAttack = true;
			}
			else if(_type == "S_Long_Attack"){
				tmp_attack -= 12;
				this.IsS_LongAttack = true;
			}
			else{
				this.IsAttack = true;
			}
			if(this.charge > 0){
				tmp_attack = 2*tmp_attack; // 攻撃力2倍
				this.charge--;							// 一回使用
			}

			if(this.type == "KNIGHT"){
				this.attack = this.attack+2;
			}
			else if(this.type == "BISHOP"){
				this.attack = (int)(this.attack*1.1f);
			}
			else if(this.type == "QUEEN"){
				target_unit.StaminaDown();
			}
			else if(this.type == "PAWN"){
				target_unit.waitTime_max = target_unit.waitTime_max + 30;
				//target_unit.waitTime_max = target_unit.waitTime_max + 15;
			}
			else if(this.type == "ROOK"){
				this.defense += 7;
				if(this.defense > 100){
					this.defense = 100;
				}
			}

			if(this.stamina < 2){
				tmp_attack = tmp_attack/2;
			}
			target_unit.OnDamage(tmp_attack);
		}
	}

	// 呼ばれるたびにHpが減る(防御力が優っていれば１) 
	public void OnDamage(int _atk){
		this.IsDamage = true;
		int _damage = _atk;
		int damage_off = this.defense;
		if(this.guard > 0){
			damage_off += 50;
			this.guard--;
		}
		_damage = (int)((_damage*(100-damage_off))/100);
		if(_damage<0){
			_damage = 0;
		}
		this.sum_damage -= _damage;		
		this.hp -= _damage;
	}
	void Heal(MapBoard_Cal _mcBoard){

		if(target_unit == null){
			return;
		}
		if(!IsSkill("Heal", _mcBoard)){
			return;
		}
		// ターゲットを回復
		// int tmp_heal = this.heal;
		int tmp_heal = 50;
		if(this.stamina < 2){
			tmp_heal = tmp_heal/2;
		}		
		target_unit.OnRecovery(tmp_heal);
		// 自分自身の回復
		// target_unit.type == this.type && target_unit.team == this.team){
		if(this.target_unit._id == this._id && this.target_unit.team == this.team){
			this.IsHeal = true;
		}
		else{
			this.IsHeal = true;
		}
	}
	public void OnRecovery(int _heal){
		this.sum_damage += _heal;
		this.hp += _heal;
		if(this.Max_hp<this.hp){
			this.hp = this.Max_hp;
		}
		this.IsRecovery = true;
	}
	public void StaminaDown(){
		if(this.type == "KING"){
			return;
		}
		this.stamina--;
		if(this.stamina <= 0){
			this.stamina = 0;
		}
	}
	public void StaminaUp(){
		if(this.action == Constants.A_RETREAT){
			this.stamina += 2;
		}
		else if(this.action == Constants.A_WAIT){
			this.stamina += 2;
		}
		else{
			this.stamina += 1;
		}
		if(this.stamina > this.Max_stamina){
			this.stamina = this.Max_stamina;
		}
	}

	void Charge(MapBoard_Cal _mcBoard){
		if(target_unit == null){
			return;// ターゲットがいないなら何もしない
		}
		if(!IsSkill("Charge", _mcBoard)){
			return;// チャージが届かないなら何もしない
		}

		// スタミナがなければ
		if(this.stamina < 2){
			// 特になし
		}
		// ターゲットのチャージを + 1する
		target_unit.OnCharge();
		// 自分自身のチャージ
		this.IsCharge = true;//target_unit.type == this.type && target_unit.team == this.team
	}

	void SpeedUP(MapBoard_Cal _mcBoard){
		if(target_unit == null){
			return;// ターゲットがいないなら何もしない
		}
		if(!IsSkill("SpeedUP", _mcBoard)){
			return;// チャージが届かないなら何もしない
		}

		// スタミナがなければ
		if(this.stamina < 2){
			// 特になし
		}
		// ターゲットのチャージを + 1する
		target_unit.OnSpeedUP();
		// 自分自身のチャージ
		this.IsSpeedUP = true;//target_unit.type == this.type && target_unit.team == this.team
	}	
	void Guard(MapBoard_Cal _mcBoard){
		if(target_unit == null){
			return;// ターゲットがいないなら何もしない
		}
		if(!IsSkill("Guard", _mcBoard)){
			return;// チャージが届かないなら何もしない
		}

		// ターゲットのチャージを + 1する
		target_unit.OnGuard();
		// 自分自身のチャージ
		this.IsGuard = true;
	}

	public void OnCharge(){
		this.charge++;
		this.IsCharged = true;		
	}
	public void OnSpeedUP(){
		this.speedUP++;
		this.IsSpeedUPed = true;		
	}
	public void OnGuard(){
		this.guard++;
		this.IsGuarded = true;		
	}


	Unit_Cal GetTargetUnit(List<Unit_Cal> _units, string _type){
		Unit_Cal tmp_taeget_unit = null;
		// 評価 Evaluation
		int max_value = -1;
		foreach(Unit_Cal _unit in _units){
			int value = GetEvaluation(_unit);
			if(max_value < value){
				max_value = value;
				tmp_taeget_unit = _unit;
			}
		}
		return tmp_taeget_unit;
	}

	// 評価関数
	int GetEvaluation(Unit_Cal _mCube){
		int value = 0;
		// ダメージがもっとも多い敵 得意
		return value;
	}

	// スキルが使えるかどうか
	bool IsSkill(string _type, MapBoard_Cal _mcBoard){
		if(_type == "Attack"){
			for(int i=0; i<attackRange.GetLength(0); i++){
				// 攻撃範囲に敵がいるか調べる
				int tmp_x = xz_position[0]+attackRange[i,0];
				int tmp_z = xz_position[1]+attackRange[i,1];
				if(target_unit.xz_position[0] == tmp_x && target_unit.xz_position[1] == tmp_z){
					return true;
				}
			}
		}
		else if(_type == "Long_Attack"){
			for(int i=0; i<attackRange_LONG.GetLength(0); i++){
				// 攻撃範囲に敵がいるか調べる
				int tmp_x = xz_position[0]+attackRange_LONG[i,0];
				int tmp_z = xz_position[1]+attackRange_LONG[i,1];
				if(target_unit.xz_position[0] == tmp_x && target_unit.xz_position[1] == tmp_z){
					return true;
				}
			}
		}
		else if(_type == "S_Long_Attack"){
			for(int i=0; i<attackRange_S_LONG.GetLength(0); i++){
				// 攻撃範囲に敵がいるか調べる
				int tmp_x = xz_position[0]+attackRange_S_LONG[i,0];
				int tmp_z = xz_position[1]+attackRange_S_LONG[i,1];
				if(target_unit.xz_position[0] == tmp_x && target_unit.xz_position[1] == tmp_z){
					return true;
				}
			}
		}
		else if(_type == "Heal"){
			for(int i=0; i<healRange.GetLength(0); i++){
				// 攻撃範囲に敵がいるか調べる
				int tmp_x = xz_position[0]+healRange[i,0];
				int tmp_z = xz_position[1]+healRange[i,1];
				if(target_unit.xz_position[0] == tmp_x && target_unit.xz_position[1] == tmp_z){
					return true;
				}
			}
		}
		else if(_type == "Charge" || _type == "SpeedUP" || _type == "Guard"){
			for(int i=0; i<supportRange.GetLength(0); i++){
				// 攻撃範囲に敵がいるか調べる
				int tmp_x = xz_position[0]+supportRange[i,0];
				int tmp_z = xz_position[1]+supportRange[i,1];
				if(target_unit.xz_position[0] == tmp_x && target_unit.xz_position[1] == tmp_z){
					return true;
				}
			}
		}
		return false;
	}
	/*-------バトルフェイズ終了----------*/

	/*-------------
	* エンドフェイズ*
	--------------*/
	public void DownWaitTime(int _moved_waitTime){
		if(this.stamina <= 1){
			_moved_waitTime = _moved_waitTime/2;
		}
		this.waitTime -= _moved_waitTime;
	}
	public void ResetWaitTime(){
		if(this.speedUP > 0){
			this.speedUP--;
			this.waitTime = this.waitTime_max/2;
			return;
		}
		this.waitTime = this.waitTime_max;
	}
}






public class NodeArrow {
    public static Unit_Plans[] plans = new Unit_Plans[4];
    public static int plan_count = 6;

    // 指定のチームに，
    public static void GetPlans(Player _player, int _type, string _jsonString){
      plans[_type] = plans[_type].CreateFromJSON(_jsonString);
      _player.plans[_type] = plans[_type];
    }

    /*
    public static void GetPlans(string _team, int _type, string _jsonString){
      plans[_type] = plans[_type].CreateFromJSON(_jsonString);
      if(_team == "My"){
          MyConnects.plans[_type] = plans[_type];
      }
      else if(_team == "Enemy"){
          Enemys.plans[_type] = plans[_type];
      }
    }*/
    public static void Init(){
        for(int i=0; i<4; i++){// aiの数
            Unit_Plans tmp_plans = new Unit_Plans(); // Planの数
            for(int j=0; j<plan_count; j++){
                Transitions tmp_transitions = new Transitions(j, new int[3]{0,0,0}, new int[3]{0,0,0}, new int[3]{0,0,0}, new int[3]{0,0,0}, new int[3]{0,0,0});
                Elements tmp_elements = new Elements(j, new int[4]{0,0,0,0}, new int[4]{0,0,0,0}, new int[4]{0,0,0,0}, new int[4]{0,0,0,0}, new int[4]{0,0,0,0});
                tmp_plans.SetPlan(j, new Plan(j, tmp_transitions, tmp_elements));

            }
            plans[i] = tmp_plans;
        }
    }  
}


public class Unit_Plans{
    public Plan plan0;
    public Plan plan1;
    public Plan plan2;
    public Plan plan3;
    public Plan plan4;
    public Plan plan5;

    public void Init(Plan _plan0, Plan _plan1, Plan _plan2, Plan _plan3, Plan _plan4, Plan _plan5){
        this.plan0 = _plan0;
        this.plan1 = _plan1;
        this.plan2 = _plan2;
        this.plan3 = _plan3;
        this.plan4 = _plan4;
        this.plan5 = _plan5;
    }
    public Plan GetPlan(int _num){
        switch(_num){
            case 0:
            return this.plan0;
            case 1:
            return this.plan1;
            case 2:
            return this.plan2;
            case 3:
            return this.plan3;
            case 4:
            return this.plan4;
            case 5:
            return this.plan5;
            default:
            return  this.plan0;
        }
    }

    public void SetPlan(int _num, Plan _plan){        
        switch(_num){
            case 0:
            this.plan0 = _plan;
            break;
            case 1:
            this.plan1 = _plan;
            break;
            case 2:
            this.plan2 = _plan;
            break;
            case 3:
            this.plan3 = _plan;
            break;
            case 4:
            this.plan4 = _plan;
            break;
            case 5:
            this.plan5 = _plan;
            break;
            default:
            this.plan0 = _plan;
            break;
        }
    }

    public string SaveToString(){
        string tmp_json = JsonUtility.ToJson(this);
        return tmp_json;
    }

    public Unit_Plans CreateFromJSON(string _jsonString){
        if(_jsonString == ""){
            Unit_Plans tmp_up = new Unit_Plans(); // Planの数
            for(int j=0; j<NodeArrow.plan_count; j++){
                Transitions tmp_transitions = new Transitions(j, new int[3]{0,0,0}, new int[3]{0,0,0}, new int[3]{0,0,0}, new int[3]{0,0,0}, new int[3]{0,0,0});
                Elements tmp_elements = new Elements(j, new int[4]{0,0,0,0}, new int[4]{0,0,0,0}, new int[4]{0,0,0,0}, new int[4]{0,0,0,0}, new int[4]{0,0,0,0});
                tmp_up.SetPlan(j, new Plan(j, tmp_transitions, tmp_elements));

            }
            return tmp_up;
        }
        return JsonUtility.FromJson<Unit_Plans>(_jsonString);
    } 
}



[Serializable]
public class Plan{
    public Transitions transitions;
    public Elements elements;
    public int id;

    public Plan(int _id, Transitions _transitions, Elements _elements){
        this.id = _id;
        this.transitions = _transitions;
        this.elements = _elements;
    }
}


[Serializable]
public class Transitions{
    public int[] row0 = new int[3]{0,0,0};
    public int[] row1 = new int[3]{0,0,0};
    public int[] row2 = new int[3]{0,0,0};
    public int[] row3 = new int[3]{0,0,0};
    public int[] row4 = new int[3]{0,0,0};
    public int id;
    [NonSerialized]
    public int count = 5;

    public Transitions(int _id, int[] _row0, int[] _row1, int[] _row2, int[] _row3, int[] _row4){
        this.id = _id;
        this.row0 = _row0;
        this.row1 = _row1;
        this.row2 = _row2;
        this.row3 = _row3;
        this.row4 = _row4;
    }

    public int[] GetRow(int _num){
        switch(_num){
            case 0:
            return row0;
            case 1:
            return row1;
            case 2:
            return row2;
            case 3:
            return row3;
            case 4:
            return row4;
            default:
            return row0;
        }
    }
    public void SetRow(int _num, int _column, int _value){
        switch(_num){
            case 0:
            this.row0[_column] = _value;
            break;
            case 1:
            this.row1[_column] = _value;
            break;
            case 2:
            this.row2[_column] = _value;
            break;
            case 3:
            this.row3[_column] = _value;
            break;
            case 4:
            this.row4[_column] = _value;
            break;
            default:
            this.row0[_column] = _value;
            break;
        }
    }   
}
[Serializable]
public class Elements{
    public int[] row0 = new int[4]{0,0,0,0};
    public int[] row1 = new int[4]{0,0,0,0};
    public int[] row2 = new int[4]{0,0,0,0};
    public int[] row3 = new int[4]{0,0,0,0};
    public int[] row4 = new int[4]{0,0,0,0};
    public int id;

    [NonSerialized]
    public int count = 5;


    public Elements(int _id, int[] _row0, int[] _row1, int[] _row2, int[] _row3, int[] _row4){
        this.id = _id;
        this.row0 = _row0;
        this.row1 = _row1;
        this.row2 = _row2;
        this.row3 = _row3;
        this.row4 = _row4;
    }
    
    public int[] GetRow(int _num){
        switch(_num){
            case 0:
            return row0;
            case 1:
            return row1;
            case 2:
            return row2;
            case 3:
            return row3;
            case 4:
            return row4;
            default:
            return row0;
        }
    }

    // （行，列，入れる値）
    public void SetRow(int _num, int _column, int _value){
        switch(_num){
            case 0:
            this.row0[_column] = _value;
            break;
            case 1:
            this.row1[_column] = _value;
            break;
            case 2:
            this.row2[_column] = _value;
            break;
            case 3:
            this.row3[_column] = _value;
            break;
            case 4:
            this.row4[_column] = _value;
            break;
            default:
            this.row0[_column] = _value;
            break;
        }
    }
}

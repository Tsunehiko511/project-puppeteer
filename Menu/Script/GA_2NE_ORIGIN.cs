using UnityEngine; // Debug.Logを使うため
using System.Collections;
using System.Collections.Generic;

// 未完成
// 現状は初期PlanのPlan内設定のみを遺伝的アルゴリズムで生成する。今後はPlan変更設定も？
// キャラはキング・クイーン・ナイト・ビショップで固定
public class GA_2NE_ORIGIN{
	int generation = 0;
	int generation_max 	 = 10; // こいつを小さくすると処理の時間が短くなるかも
	Player[] children = new Player[6]{new Player(),new Player(),new Player(),new Player(),new Player(),new Player()}; 	// 5つの個体（こいつらをふるいにかける）
	Player[] parents = new Player[2]{new Player(),new Player()}; 		// 親個体
	Player battle_player = new Player(); 														// こいつに勝てる個体を見つける
	int[] evaluations = new int[6]{0,0,0,0,0,0}; 											// 5個体の評価
	int number1_id;																									// 評価が最も良い個体の番号
	int number2_id;																									// 評価が２番目に良い個体の番号
	int before_number1_id; 																					// 前世代で最も良い個体の番号（閲覧用に作った）

	AI_Database ai_db = new AI_Database();
	System.Random cRandom = new System.Random();

	public GA_2NE_ORIGIN(){
		// 登場人物はあらかじめ生成しておく
		for(int i=0; i<children.Length; i++){
			children[i].SetData(ai_db.GetUnits(""), ai_db.GetCodes(""));
		}
		for(int i=0; i<parents.Length; i++){
			parents[i].SetData(ai_db.GetUnits(""), ai_db.GetCodes(""));
		}
		// Plan0のPlan内設定をランダムに設定する
		Initial_Setting();
	}

	public bool Play(GameMaster _gamemaster){
		int old_generation = generation;
		string result = "";
		before_number1_id = number1_id;

		while(generation < old_generation+generation_max){
			Evaluation(_gamemaster);
			Selection();
			Crossover();
			Mutation();
			// 前の一番良かったやつと違うならコードを表示（前回よりいい成績のコードが閲覧できる）
			if(number1_id != before_number1_id){
				result = _gamemaster.Play(parents[0], battle_player);
				Debug.LogFormat("勝者：{0}  詳細(赤,青)：残機({1},{2})，キングHP({3}, {4})", result, _gamemaster.GetResult("RED_LEFT"), _gamemaster.GetResult("BLUE_LEFT"), _gamemaster.GetResult("RED_KING_HP"), _gamemaster.GetResult("BLUE_KING_HP"));
				Debug.Log(parents[0].plans[0].SaveToString()+"\n"
					+parents[0].plans[1].SaveToString()+"\n"
					+parents[0].plans[2].SaveToString()+"\n"
					+parents[0].plans[3].SaveToString());
			}
			generation++;
		}
		if(result == "RED"){
			Debug.Log("終了："+generation+"世代");
			return false;
		}
		Debug.Log("まだまだ："+generation+"世代");
		return true;
	}

	// 初期設定
	void Initial_Setting(){
		// 子供をランダム設定
		for(int i=0; i<children.Length; i++){
			SetInitRandom(children[i]);
		}
		// 評価する時に対戦する相手の設定
		battle_player.SetData(ai_db.GetUnits("第7回(fred)"), ai_db.GetCodes("第7回(fred)"));
	}

	// 評価
	void Evaluation(GameMaster _gamemaster){
		for(int i=0; i<children.Length; i++){
			// 対戦に勝利すればRED，負ければBLUEを取得する
			string tmp_win = _gamemaster.Play(children[i], battle_player);
			//評価関数は適当
			evaluations[i] = 3*(_gamemaster.GetResult("RED_KING_HP") - _gamemaster.GetResult("BLUE_KING_HP")) + 250*(_gamemaster.GetResult("RED_LEFT") - _gamemaster.GetResult("BLUE_LEFT"));
			// 勝てばめっちゃ評価する
			if(tmp_win == "RED"){
				evaluations[i] += 10000;
			}
		}
	}

	// 選択（evaluationsの最も高い2組を選ぶ）
	void Selection(){
		int number1 = evaluations[0];
		int number2 = evaluations[1];
		number1_id = 0;
		number2_id = 1;
		if(number1 < number2){
			number1 = evaluations[1];
			number2 = evaluations[0];
			number1_id = 1;
			number2_id = 0;
		}

		// まず１位と比較，小さければ２位と比較
		for(int i=2; i<evaluations.Length; i++){
			if(number1 < evaluations[i]){
				// １位を２位へ
				number2 = number1;
				number2_id = number1_id;
				// 新しいのを１位へ
				number1 = evaluations[i];
				number1_id = i;
			}
			else if(number2 < evaluations[i]){
				// 新しいのを２位へ
				number2 = evaluations[i];
				number2_id = i;
			}
		}
		// 上位2名を親とする（参照渡しが怖いので1度Json化してからデータのみ渡す）
		string[] json1 = new string[4]{"","","",""};
		for(int i=0; i<json1.Length; i++){
			json1[i] = children[number1_id].plans[i].SaveToString();
		}
		string[] json2 = new string[4]{"","","",""};
		for(int i=0; i<json2.Length; i++){
			json2[i] = children[number2_id].plans[i].SaveToString();
		}
		parents[0].SetData(children[number1_id].Units, json1);
		parents[1].SetData(children[number2_id].Units, json2);
	}

	// 交叉（自信ない）
	void Crossover(){
		OnePointCrossover(parents[0], parents[1], children[2], children[3]);
		OnePointCrossover(parents[0], parents[1], children[4], children[5]);

		// Plan内設定を交叉する。
		// 特に思いつかんのでそのまま子を作る
		string[] json1 = new string[4]{"","","",""};
		for(int i=0; i<json1.Length; i++){
			json1[i] = parents[0].plans[i].SaveToString();
		}
		string[] json2 = new string[4]{"","","",""};
		for(int i=0; i<json2.Length; i++){
			json2[i] = parents[1].plans[i].SaveToString();
		}
		children[0].SetData(parents[0].Units, json1);
		children[1].SetData(parents[1].Units, json2);
	}

	// 一点交叉：とりあえず作ったけど，あっているかわからん
	void OnePointCrossover(Player _parent1, Player _parent2, Player _child1, Player _child2){
		// 全てのユニットに交叉を適用する
		for(int unit_id=0; unit_id<_parent1.plans.Length; unit_id++){
			// 親の初期Planを所得
			Plan tmp_plan0 = _parent1.plans[unit_id].GetPlan(0);
			Plan tmp_plan1 = _parent2.plans[unit_id].GetPlan(0);
			// 交叉点を決める
			int cross_point = cRandom.Next(5);
			for(int i=0; i<5; i++){
				// 交叉点まで親1の初期Planを入れる。交叉点以降は親2。
				if(i<cross_point){
					_child1.plans[unit_id].GetPlan(0).elements.SetRow(i, tmp_plan0.elements.GetRow(i));
					_child2.plans[unit_id].GetPlan(0).elements.SetRow(i, tmp_plan1.elements.GetRow(i));
				}
				else{
					_child1.plans[unit_id].GetPlan(0).elements.SetRow(i, tmp_plan1.elements.GetRow(i));
					_child2.plans[unit_id].GetPlan(0).elements.SetRow(i, tmp_plan0.elements.GetRow(i));
				}
			}
		}
	}

	// 特然変異
	void Mutation(){
		// とりあえず３体の子に突然変異を起こす。最初の2体はエリートとして残す
		for(int i=2; i<children.Length; i++){
			// 4体のユニットのplan0のどこなの行を入れ替える
			for(int j=0; j<4; j++){
				int r1 = cRandom.Next(5);
				children[i].plans[j].GetPlan(0).elements.SetRow(r1, GetPlanElementCode());
			}			
		}
	}

	// Plan0のPlan内設定をランダム生成・Plan変更設定はリセット
	void SetInitRandom(Player _player){
		// 今回はキング，クイーン，ナイト，ビショップで固定
		_player.Units = new int[4]{0,1,2,3}; 
		// 4体のPlan0のデータを初期設定する i=ユニットのid, j=Plan0の行のid
		for(int i=0; i<4; i++){
			// Plan0のPlan変更設定を初期にする
			for(int j=0; j<5; j++){
		    _player.plans[i].GetPlan(0).transitions.SetRow(j, new int[3]{0,0,0}); // Plan変更設定を初期にする=Plan変更しない
		    _player.plans[i].GetPlan(0).elements.SetRow(j, GetPlanElementCode()); // Plan内設定にランダムな行を入れる
			}
		}
	}

	// AIのデータベースからPlan内設定の行をランダムに取得する。
	int[] GetPlanElementCode(){
		List<int[]> tmp_list = ai_db.GetPlanSettingCodeRowList();
		int rndm =  cRandom.Next(tmp_list.Count);
		return tmp_list[rndm];
	}
}
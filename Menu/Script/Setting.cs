using UnityEngine;
using System.Collections.Generic;

public class Setting : MonoBehaviour {
	GameMaster Game;
	Player player1;
	Player player2;
	AI_Database ai_db;

	void Start(){
		ai_db = new AI_Database();
		Game = new GameMaster();
		player1 = new Player();
		player2 = new Player();
		// 歴代の優勝AIのJsonデータに関して，Plan内設定の行を重複なく配列として取得し，コンソールに表示する
		// Debug.Log(ai_db.GetConsoleLogCodeArray(ai_db.GetPlanSettingCodeRowList()));
	}

	// ボタンを押すとこの関数を実行する
	public void PushTestButton(){
		// player1とplayer2にデータをセットする。SetData(セットするプレイヤ，ユニットの種類の配列，AIの配列);
		player1.SetData(ai_db.GetUnits(ai_db.GetName(7)), ai_db.GetCodes(ai_db.GetName(7)));
		player2.SetData(ai_db.GetUnits(ai_db.GetName(10)), ai_db.GetCodes(ai_db.GetName(10)));
		//Game.Play(赤, 青)：1回処理するのに0.02秒程度かかる
		for(int i=0; i<1; i++){
			Debug.LogFormat("結果：{0}  詳細(赤,青)：残機({1},{2})，キングHP({3}, {4})", Game.Play(player1, player2), Game.GetResult("RED_LEFT"), Game.GetResult("BLUE_LEFT"), Game.GetResult("RED_KING_HP"), Game.GetResult("BLUE_KING_HP"));
			Debug.LogFormat("結果：{0}  詳細(赤,青)：残機({1},{2})，キングHP({3}, {4})", Game.Play(player2, player1), Game.GetResult("RED_LEFT"), Game.GetResult("BLUE_LEFT"), Game.GetResult("RED_KING_HP"), Game.GetResult("BLUE_KING_HP"));
		}
	}
}
// 時間計測
// float start_time = Time.time;
// float end_time = Time.time - start_time;
// Debug.Log(end_time);っとすればstart_timeから end_time の時間を計測できる
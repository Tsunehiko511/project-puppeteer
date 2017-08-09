using UnityEngine; // Debug.Logを使うため
using System.Collections;
using System.Collections.Generic;

// 未完成
// 現状は初期PlanのPlan内設定のみを遺伝的アルゴリズムで生成する。今後はPlan変更設定も？
// キャラはキング・クイーン・ナイト・ビショップで固定
public class GA_SHIGURAL_ORIGIN : MonoBehaviour
{
    int generation = 0;
    int generation_max = 1;
    //Player[] children = new Player[20] {  new Player(), new Player(), new Player(), new Player(), new Player(), new Player(), new Player(), new Player(), new Player(), new Player(), new Player(), new Player(), new Player(), new Player(), new Player(), new Player(), new Player(), new Player(), new Player(), new Player() }; 	// 5つの個体（こいつらをふるいにかける）
    Player[] children = new Player[2] { new Player(), new Player() }; 	// 5つの個体（こいつらをふるいにかける）
    Player[] parents = new Player[2] { new Player(), new Player() }; 		// 親個体
    const int enemy_num = 10;//敵の数
    Player[] battle_player = new Player[10] { new Player(), new Player(), new Player(), new Player(), new Player(), new Player(), new Player(), new Player(), new Player(), new Player() }; // こいつに勝てる個体を見つける
    //int[] evaluations = new int[20] {  0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 }; 											// 5個体の評価
    int[] evaluations = new int[2] { 0, 0 }; 											// 5個体の評価
    int number1_id;																									// 評価が最も良い個体の番号
    int number2_id;																									// 評価が２番目に良い個体の番号
    int before_number1_id; 																					// 前世代で最も良い個体の番号（閲覧用に作った）
    int top_score;//最大評価値
    GameMaster _gamemaster;
    AI_Database ai_db = new AI_Database();
    System.Random cRandom = new System.Random();


    public GA_SHIGURAL_ORIGIN()
    {
        // 登場人物はあらかじめ生成しておく
        for (int i = 0; i < children.Length; i++)
        {
            children[i].SetData(ai_db.GetUnits(""), ai_db.GetCodes(""));
        }
        for (int i = 0; i < parents.Length; i++)
        {
            parents[i].SetData(ai_db.GetUnits(""), ai_db.GetCodes(""));
        }
        // Plan0のPlan内設定をランダムに設定する
        Initial_Setting();

        //基本の姿を設定
        _gamemaster = new GameMaster();
    }





    public void Play()
    {
        before_number1_id = number1_id;


        Evaluation(_gamemaster);
        Selection();
        Crossover();
        Mutation();
        // 前の一番良かったやつと違うならコードを表示（前回よりいい成績のコードが閲覧できる）
        Debug.Log("■" + generation + "世代:評価値" + top_score);
        if (number1_id != before_number1_id)
        {
            Debug.Log("■" + parents[0].plans[0].SaveToString() + "\n" + "■" + parents[0].plans[1].SaveToString() + "\n" + "■" + parents[0].plans[2].SaveToString() + "\n" + "■" + parents[0].plans[3].SaveToString() + "■");
        }
        generation++;


        if (top_score <= generation*100 && top_score<12000)
        {
            Debug.Log("◯" + generation + "世代:評価値" + top_score + "は将来性がないため初期化します");
            Debug.Log("■" + parents[0].plans[0].SaveToString() + "\n" + "■" + parents[0].plans[1].SaveToString() + "\n" + "■" + parents[0].plans[2].SaveToString() + "\n" + "■" + parents[0].plans[3].SaveToString() + "■");
            Debug.Log("◯-----------------◯");
            top_score = 0;
            Initial_Setting();

        }

    }


    // 初期設定
    void Initial_Setting()
    {
        // 子供をランダム設定
        for (int i = 0; i < children.Length; i++)
        {
            SetInitRandom(children[i]);
        }
        // 評価する時に対戦する相手の設定
        for (int i = 0; i < 10; i++) battle_player[i].SetData(ai_db.GetUnits(ai_db.GetName(i)), ai_db.GetCodes(ai_db.GetName(i)));

        //初期化
        generation = 0;

    }

    // 評価
    void Evaluation(GameMaster _gamemaster)
    {
        int plus = 0;
        int king_score = 0, life_score = 0;
        for (int i = 0; i < children.Length; i++)
        {
            if (i == 0 && top_score != 0)
            {
                evaluations[0] = top_score;
                continue;
            }
            evaluations[i] = 0;
            for (int j = 7; j < enemy_num; j++)
            {
                // 対戦に勝利すればRED，負ければBLUEを取得する
                string tmp_win = _gamemaster.Play(children[i], battle_player[j]);
                //評価関数は適当
                if (tmp_win == "RED")
                {
                    evaluations[i] += 4000;
                }
                else
                {
                    if (tmp_win == "DRAW") plus = 200 - _gamemaster.GetResult("TIME") + 1000;
                    else plus = 0;
                    king_score = 6 * (_gamemaster.GetResult("RED_KING_HP") - _gamemaster.GetResult("BLUE_KING_HP"));
                    life_score = 250 * (_gamemaster.GetResult("RED_LEFT") - _gamemaster.GetResult("BLUE_LEFT"));
                    //Debug.Log("◯" + generation + "世代 " + i +"vs" +j +"結果"+king_score+"と"+life_score+"で"+evaluations[i]+"より成長");
                    if (king_score > life_score) evaluations[i] += king_score + plus;
                    else evaluations[i] += life_score + plus;
                }
            }

        }
    }

    // 選択（evaluationsの最も高い2組を選ぶ）
    void Selection()
    {
        int number1 = evaluations[0];
        int number2 = evaluations[1];
        number1_id = 0;
        number2_id = 1;
        if (number1 < number2)
        {
            number1 = evaluations[1];
            number2 = evaluations[0];
            number1_id = 1;
            number2_id = 0;
        }

        // まず１位と比較，小さければ２位と比較
        for (int i = 2; i < evaluations.Length; i++)
        {
            if (number1 < evaluations[i])
            {
                /*  // １位を２位へ
                  number2 = number1;
                  number2_id = number1_id;*/
                // 新しいのを１位へ
                number1 = evaluations[i];
                number1_id = i;
            }
            /* else if (number2 < evaluations[i])
             {
                 // 新しいのを２位へ
                 number2 = evaluations[i];
                 number2_id = i;
             }*/
        }
        top_score = number1;
        // 上位2名を親とする（参照渡しが怖いので1度Json化してからデータのみ渡す）
        string[] json1 = new string[4] { "", "", "", "" };
        for (int i = 0; i < json1.Length; i++)
        {
            json1[i] = children[number1_id].plans[i].SaveToString();
        }
        /*string[] json2 = new string[4] { "", "", "", "" };
        for (int i = 0; i < json2.Length; i++)
        {
            json2[i] = children[number2_id].plans[i].SaveToString();
        }*/
        parents[0].SetData(children[number1_id].Units, json1);
        // parents[1].SetData(children[number2_id].Units, json2);
    }

    // 交叉
    void Crossover()
    {
        // Plan内設定を交叉する。
        // 特に思いつかんのでそのまま子を作る
        string[] json1 = new string[4] { "", "", "", "" };
        for (int i = 0; i < json1.Length; i++)
        {
            json1[i] = parents[0].plans[i].SaveToString();
        }
        /*string[] json2 = new string[4] { "", "", "", "" };
        for (int i = 0; i < json2.Length; i++)
        {
            json2[i] = parents[1].plans[i].SaveToString();
        }*/
        for (int i = 0; i < children.Length; i++)
        {
            children[i].SetData(parents[0].Units, json1);
        }
        /* children[1].SetData(parents[1].Units, json2);
         children[2].SetData(parents[0].Units, json1);
         children[3].SetData(parents[0].Units, json1);
         children[4].SetData(parents[1].Units, json2);*/
    }
    // 特然変異
    void Mutation()
    {
        // とりあえず20体の子に突然変異を起こす。最初の1体はエリートとして残す
        for (int i = 1; i < children.Length; i++)
        {

            for (int j = 0; j < 4; j++)
            {
                for (int k = 0; k < 6; k++)
                {
                    if (cRandom.Next(2) == 0)//突然変異の確率は50%
                    {
                        //プランの内容設定
                        int r1 = cRandom.Next(5);
                        children[i].plans[j].GetPlan(k).elements.SetRow(r1, GetPlanElementCode());
                    }
                    if (cRandom.Next(2) == 0)//突然変異の確率は50%
                    {
                        //プラン移動の条件設定
                        int r1 = cRandom.Next(5);
                        children[i].plans[j].GetPlan(k).transitions.SetRow(r1, GetTransitionsCode());

                    }
                }

            }
        }
    }



    // Plan0のPlan内設定をランダム生成・Plan変更設定はリセット
    void SetInitRandom(Player _player)
    {
        // 今回はキング，クイーン，ナイト，ビショップで固定
        _player.Units = new int[4] { 0, 13, 14, 15 };
        // 4体のPlan0のデータを初期設定する i=ユニットのid, j=Plan0の行のid
        for (int i = 0; i < 4; i++)
        {
            // PlanおよびPlan変更設定をランダムにする
            for (int j = 0; j < 5; j++)
            {
                for (int k = 0; k < 6; k++)
                {
                    _player.plans[i].GetPlan(k).transitions.SetRow(j, GetTransitionsCode()); // Plan変更設定を初期にする=Plan変更しない
                    _player.plans[i].GetPlan(k).elements.SetRow(j, GetPlanElementCode()); // Plan内設定にランダムな行を入れる
                }
            }
        }
    }

    // AIのデータベースからPlan内設定の行をランダムに取得する。
    int[] GetPlanElementCode()
    {
        List<int[]> tmp_list = ai_db.GetPlanSettingCodeRowList();
        int rndm = cRandom.Next(tmp_list.Count);
        return tmp_list[rndm];
    }

    // AIのデータベースからPlan移動の行をランダムに取得する。
    int[] GetTransitionsCode()
    {
        List<int[]> tmp_list = ai_db.GetTransitionsCodeRowList();
        int rndm = cRandom.Next(tmp_list.Count);
        return tmp_list[rndm];
    }
}
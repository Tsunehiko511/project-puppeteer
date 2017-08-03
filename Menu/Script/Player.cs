public class Player{
	public string color = ""; 											// 赤？青？
	public int[] Units = new int[4]{0,1,2,3};     	// 初期は[キング,クイーン,ナイト,ビショップ]
	public Unit_Plans[] plans = new Unit_Plans[4];	// 4ユニット分のPlan(AI)が入る。

	public void SetData(int[] _ids, string[] _json){
    for(int i=0; i<4; i++){
      NodeArrow.GetPlans(this, i, _json[i]);
      this.Units[i] = _ids[i];
    }
	}
}
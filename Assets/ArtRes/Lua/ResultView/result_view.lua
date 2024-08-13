ResultView = ResultView or BaseClass(BaseView)

function ResultView:__init()
	self.ui_config = {"ui", "ResultPanel"}

    self.heroRawImage=nil
    self.resultText=nil
    self.returnButton=nil
end

function ResultView:__delete()

end

function ResultView:ReleaseCallBack()
    self.heroRawImage=nil
    self.resultText=nil
    self.returnButton=nil
end

function ResultView:LoadCallBack()
    self.root_node.transform:SetParent(Canvas, false)

    self.resultText = self.root_node.transform:Find("ResultText"):GetComponent(typeof(Text))
    self.scoreText = self.root_node.transform:Find("AddScore/ScoreText"):GetComponent(typeof(Text))
    --重新赋值RenderText
    self.heroRawImage = self.root_node.transform:Find("HeroRawImage"):GetComponent(typeof(RawImage))
    self.heroRawImage.texture = Resources.Load("Target")

    --Btn
    self.returnButton = self.root_node.transform:Find("ReturnButton"):GetComponent(typeof(Button))
    --Anim
    self.showAnim =  CurrectHero:GetComponent(typeof(Anim))
    --如果直接.传入自己的函数 在函数内部 无法使用self获取内容
    self.returnButton.onClick:AddListener(
        function()
           self:Close()
        end
    )
end

function ResultView:OpenCallBack()
    ViewManager.Instance:Close(ViewName.BattleMessageView)
    self.root_node:SetActive(true)
    if resultText == "胜利" then
        self.scoreText.text = "+100" 
        self.showAnim:SetBool("win",true)
    else
        self.scoreText.text = "-10"
        self.showAnim:SetBool("defeat",true)
    end
    self.resultText.text=resultText.."!"
end

function ResultView:CloseCallBack()
    self.root_node:SetActive(false)
    self.showAnim:SetBool("win",false)
    self.showAnim:SetBool("defeat",false)
    ShowMainCtrl.Instance:ShowPanel()
end
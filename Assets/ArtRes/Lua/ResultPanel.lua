ResultPanel = {}

--关联的面板对象
ResultPanel.panelObj = nil
ResultPanel.heroRawImage=nil
ResultPanel.resultText=nil
ResultPanel.returnButton=nil

function ResultPanel:Init()
    self.panelObj = ABManager:LoadRes("ui", "ResultPanel", typeof(GameObject))
    self.panelObj.transform:SetParent(Canvas, false)

    self.resultText = self.panelObj.transform:Find("ResultText"):GetComponent(typeof(Text))
    self.scoreText = self.panelObj.transform:Find("AddScore/ScoreText"):GetComponent(typeof(Text))
    --重新赋值RenderText
    self.heroRawImage = self.panelObj.transform:Find("HeroRawImage"):GetComponent(typeof(RawImage))
    self.heroRawImage.texture = Resources.Load("Target")

    --Btn
    self.returnButton = self.panelObj.transform:Find("ReturnButton"):GetComponent(typeof(Button))
    --Anim
    self.showAnim =  CurrectHero:GetComponent(typeof(Anim))
    --如果直接.传入自己的函数 在函数内部 无法使用self获取内容
    self.returnButton.onClick:AddListener(
        function()
           self:Close()
        end
    )
end

function ResultPanel:Show(resultText)
    if self.panelObj == nil then
        self:Init()
    end
    BattleMessagePanel:Close()
    self.panelObj:SetActive(true)
    if resultText == "胜利" then
        self.scoreText.text = "+100" 
        self.showAnim:SetBool("win",true)
    else
        self.scoreText.text = "-10"
        self.showAnim:SetBool("defeat",true)
    end
    self.resultText.text=resultText.."!"
end

function ResultPanel:Close()
    self.panelObj:SetActive(false)
    self.showAnim:SetBool("win",false)
    self.showAnim:SetBool("defeat",false)
    GameMainPanel:Show()
end
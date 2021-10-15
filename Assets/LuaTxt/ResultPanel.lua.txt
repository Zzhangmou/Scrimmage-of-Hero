ResultPanel = {}

--关联的面板对象
ResultPanel.panelObj = nil
ResultPanel.heroRawImage=nil
ResultPanel.resultText=nil
ResultPanel.returnButton=nil

function ResultPanel:Init()
    self.panelObj = ABMgr:LoadRes("ui", "ResultPanel", typeof(GameObject))
    self.panelObj.transform:SetParent(Canvas, false)

    self.resultText = self.panelObj.transform:Find("ResultText"):GetComponent(typeof(Text))
    --重新赋值RenderText
    self.heroRawImage = self.panelObj.transform:Find("HeroRawImage"):GetComponent(typeof(RawImage))
    self.heroRawImage.texture = Resources.Load("Target")

    --Btn
    self.returnButton = self.panelObj.transform:Find("ReturnButton"):GetComponent(typeof(Button))
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
    self.panelObj:SetActive(true)
    self.resultText.text=resultText
end

function ResultPanel:Close()
    self.panelObj:SetActive(false)
    GameMainPanel:Show()
end
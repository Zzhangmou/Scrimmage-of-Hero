GameMainPanel = {}

--关联的面板对象
GameMainPanel.panelObj = nil
--对应的面板控件
GameMainPanel.showHeroRImage = nil
GameMainPanel.showHeroBtn = nil
GameMainPanel.setBtn = nil
GameMainPanel.mapBtn = nil
GameMainPanel.startBtn = nil

function GameMainPanel:Init()
    self.panelObj = ABMgr:LoadRes("ui", "GameMainPanel", typeof(GameObject))
    self.panelObj.transform:SetParent(Canvas, false)

    --重新赋值RenderText
    self.showHeroRImage = self.panelObj.transform:Find("ShowHeroRawImage"):GetComponent(typeof(RawImage))
    self.showHeroRImage.texture = CS.ResManager.Load("Target")

    --Btn
    self.showHeroBtn = self.panelObj.transform:Find("ShowHeroRawImage"):GetComponent(typeof(Button))
    --如果直接.传入自己的函数 在函数内部 无法使用self获取内容
    --self.showHeroBtn.onClick:AddListener(self.ShowChoiceHeroPanel)
    self.showHeroBtn.onClick:AddListener(
        function()
            self:ShowChoiceHeroPanel()
        end
    )
end

function GameMainPanel:Show()
    if self.panelObj == nil then
        self:Init()
    end
    self.panelObj:SetActive(true)
end

function GameMainPanel:Close()
    self.panelObj:SetActive(false)
end

function GameMainPanel:ShowChoiceHeroPanel()
    ChoiceHeroPanel:Show()
end

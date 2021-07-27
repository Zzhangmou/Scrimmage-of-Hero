ChoiceHeroPanel = {}

ChoiceHeroPanel.panelObj = nil
ChoiceHeroPanel.showHeroRImage = nil
ChoiceHeroPanel.Content = nil
ChoiceHeroPanel.returnBtn = nil

local iconDataList = heroiconDataList

function ChoiceHeroPanel:Init()
    self.panelObj = ABMgr:LoadRes("ui", "ChoiceHeroPanel", typeof(GameObject))
    self.panelObj.transform:SetParent(Canvas, false)

    --获取组件
    self.returnBtn = self.panelObj.transform:Find("ReturnButton"):GetComponent(typeof(Button))

    --重新赋值RenderText
    self.showHeroRImage = self.panelObj.transform:Find("ShowHeroRawImage"):GetComponent(typeof(RawImage))
    self.showHeroRImage.texture = CS.ResManager.Load("Target")

    self.Content = self.panelObj.transform:Find("ChoiceHeroView/Viewport/Content")

    self:CreateHeroIcon()

    --添加事件
    self.returnBtn.onClick:AddListener(
        function()
            self:Close()
        end
    )
end

function ChoiceHeroPanel:CreateHeroIcon()
    for _, value in pairs(iconDataList) do
        --创建格子
        local grid = IconItemGrid:new()
        grid:Init(self.Content)
        grid:InitData(value)
    end
end

function ChoiceHeroPanel:Show()
    if self.panelObj == nil then
        self:Init()
    end
    self.panelObj:SetActive(true)
end

function ChoiceHeroPanel:Close()
    self.panelObj:SetActive(false)
end

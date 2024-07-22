ChoiceHeroPanel = {}

ChoiceHeroPanel.panelObj = nil
ChoiceHeroPanel.showHeroRImage = nil
ChoiceHeroPanel.Content = nil
ChoiceHeroPanel.returnBtn = nil

local iconDataList = HeroiconDataList

function ChoiceHeroPanel:Init()
    self.panelObj = ABManager:LoadRes("ui", "ChoiceHeroPanel", typeof(GameObject))
    self.panelObj.transform:SetParent(Canvas, false)

    --获取组件
    self.returnBtn = self.panelObj.transform:Find("ReturnButton"):GetComponent(typeof(Button))
    self.backBtn = self.panelObj.transform:Find("BackButton"):GetComponent(typeof(Button))

    self.Content = self.panelObj.transform:Find("ChoiceHeroView/Viewport/Content")

    self:CreateHeroIcon()

    --添加事件
    self.returnBtn.onClick:AddListener(
        function()
            self:Close()
        end
    )
    self.backBtn.onClick:AddListener(
        function()
            self:Close()
        end
    )
end

function ChoiceHeroPanel:CreateHeroIcon()
    local list = RecyclingList:new()
    list:InitContentAndSVH(self.panelObj.transform:Find("ChoiceHeroView"), IconItemGrid, iconDataList)
    -- for _, value in pairs(iconDataList) do
    --     --创建格子
    --     local grid = IconItemGrid:new()
    --     grid:Init(self.Content)
    --     grid:InitData(value)
    --     --加载人物模型
    --     local hero = HeroShowItem:new()
    --     hero:Init(value.name, false)
    -- end
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

ProgressPanel = {}
--进度面板
ProgressPanel.panelObj = nil

ProgressPanel.progressSlider = nil

function ProgressPanel:Init()
    self.panelObj = ABManager:LoadRes("ui", "ProgressPanel", typeof(GameObject))
    self.panelObj.transform:SetParent(TipCanvas, false)

    self.progressSlider = self.panelObj.transform:Find("ProgressBar"):GetComponent(typeof(Slider))
end

function ProgressPanel:Show(showPtName)
    if self.panelObj == nil then
        self:Init()
    end
    local userHeroId=tonumber(showPtName)
    local ptName=HeroiconDataList[userHeroId].name
    local ptShow=ABManager:LoadRes("progressui", ptName, typeof(Sprite))
    self.panelObj.transform:GetComponent(typeof(Image)).sprite=ptShow
    self.panelObj:SetActive(true)
end

function ProgressPanel:Close()
    self.panelObj:SetActive(false)
end

function ProgressPanel:ChangeSliderValue(value)
    self.progressSlider.value = value
end
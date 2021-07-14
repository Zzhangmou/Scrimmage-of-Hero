ChoiceHeroPanel = {}

ChoiceHeroPanel.panObj = nil
ChoiceHeroPanel.showHeroRImage = nil

function ChoiceHeroPanel:Init()
    self.panelObj = ABMgr:LoadRes("ui", "ChoiceHeroPanel", typeof(GameObject))
    self.panelObj.transform:SetParent(Canvas, false)

    --重新赋值RenderText
    self.showHeroRImage = self.panelObj.transform:Find("ShowHeroRawImage"):GetComponent(typeof(RawImage))
    self.showHeroRImage.texture = CS.ResManager.Load("Target")
end
ControlPanel = {}

ControlPanel.panelObj = nil

function ControlPanel:Init()
    self.panelObj = ABMgr:LoadRes("ui", "ControlPanel", typeof(GameObject))
    self.panelObj.transform:SetParent(Canvas, false)
end

function ControlPanel:Show()
    if self.panelObj == nil then
        self:Init()
    end
    self.panelObj:SetActive(true)
end

function ControlPanel:Close()
    self.panelObj:SetActive(false)
end
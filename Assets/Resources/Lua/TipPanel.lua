TipPanel = {}

TipPanel.panelObj = nil

TipPanel.contentText = nil
TipPanel.closeBtn = nil

function TipPanel:Init()
    self.panelObj = ABMgr:LoadRes("ui", "TipPanel", typeof(GameObject))
    self.panelObj.transform:SetParent(TipCanvas, false)

    --Btn
    self.contentText = self.panelObj.transform:Find("ContentText"):GetComponent(typeof(Text))
    self.closeBtn = self.panelObj.transform:Find("CloseButton"):GetComponent(typeof(Button))

    self.closeBtn.onClick:AddListener(
        function()
            self:Close()
        end
    )
end

function TipPanel:Show(showText)
    if self.panelObj == nil then
        self:Init()
    end
    self.panelObj:SetActive(true)
    self.contentText.text = showText
end

function TipPanel:Close()
    self.panelObj:SetActive(false)
end

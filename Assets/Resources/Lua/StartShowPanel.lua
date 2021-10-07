StartShowPanel = {}

StartShowPanel.panelObj = nil

StartShowPanel.loginBtn = nil
StartShowPanel.QuitBtn = nil

function StartShowPanel:Init()
    self.panelObj = ABMgr:LoadRes("ui", "StartShowPanel", typeof(GameObject))
    self.panelObj.transform:SetParent(Canvas, false)

    --Btn
    self.loginBtn = self.panelObj.transform:Find("LoginButton"):GetComponent(typeof(Button))
    self.QuitBtn = self.panelObj.transform:Find("QuitButton"):GetComponent(typeof(Button))

    self.loginBtn.onClick:AddListener(
        function()
            LoginPanel:Show()
        end
    )
end

function StartShowPanel:Show()
    if self.panelObj == nil then
        self:Init()
    end
    self.panelObj:SetActive(true)
end

function StartShowPanel:Close()
    self.panelObj:SetActive(false)
end
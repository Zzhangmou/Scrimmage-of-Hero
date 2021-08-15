RegisterPanel = {}

RegisterPanel.panelObj = nil

RegisterPanel.UserNameInput = nil
RegisterPanel.UserInput = nil
RegisterPanel.PwInput = nil
RegisterPanel.RegisterBtn = nil

function RegisterPanel:Init()
    self.panelObj = ABMgr:LoadRes("ui", "RegisterPanel", typeof(GameObject))
    self.panelObj.transform:SetParent(Canvas, false)

    --InputText
    self.UserNameInput = self.panelObj.transform:Find("UserNameInput"):GetComponent(typeof(InputField))
    self.UserInput = self.panelObj.transform:Find("UserInput"):GetComponent(typeof(InputField))
    self.PwInput = self.panelObj.transform:Find("PwInput"):GetComponent(typeof(InputField))
    --Btn
    self.RegisterBtn = self.panelObj.transform:Find("RegisterButton"):GetComponent(typeof(Button))

    self.RegisterBtn.onClick:AddListener(
        function()
        end
    )
end

function RegisterPanel:Show()
    if self.panelObj == nil then
        self:Init()
    end
    self.panelObj:SetActive(true)
end

function RegisterPanel:Close()
    self.panelObj:SetActive(false)
end

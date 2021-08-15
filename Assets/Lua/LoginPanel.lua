LoginPanel = {}

LoginPanel.panelObj = nil

LoginPanel.UserInput = nil
LoginPanel.PwInput = nil
LoginPanel.loginBtn = nil
LoginPanel.RegisterBtn = nil

function LoginPanel:Init()
    self.panelObj = ABMgr:LoadRes("ui", "LoginPanel", typeof(GameObject))
    self.panelObj.transform:SetParent(Canvas, false)

    --InputText
    self.UserInput = self.panelObj.transform:Find("UserInput"):GetComponent(typeof(InputField))
    self.PwInput = self.panelObj.transform:Find("PwInput"):GetComponent(typeof(InputField))
    --Btn
    self.loginBtn = self.panelObj.transform:Find("LoginButton"):GetComponent(typeof(Button))
    self.RegisterBtn = self.panelObj.transform:Find("RegisterButton"):GetComponent(typeof(Button))

    self.loginBtn.onClick:AddListener(
        function()
        end
    )
    self.RegisterBtn.onClick:AddListener(
        function()
            RegisterPanel:Show()
        end
    )
end

function LoginPanel:Show()
    if self.panelObj == nil then
        self:Init()
    end
    self.panelObj:SetActive(true)
end

function LoginPanel:Close()
    self.panelObj:SetActive(false)
end
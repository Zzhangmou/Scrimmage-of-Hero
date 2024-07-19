RegisterPanel = {}

RegisterPanel.panelObj = nil

RegisterPanel.UserNameInput = nil
RegisterPanel.UserInput = nil
RegisterPanel.PwInput = nil
RegisterPanel.RegisterBtn = nil

function RegisterPanel:Init()
    self.panelObj = ABManager:LoadRes("ui", "RegisterPanel", typeof(GameObject))
    self.panelObj.transform:SetParent(Canvas, false)

    --InputText
    self.UserNameInput = self.panelObj.transform:Find("Input/UserNameInput"):GetComponentInChildren(typeof(InputField))
    self.UserInput = self.panelObj.transform:Find("Input/UserInput"):GetComponentInChildren(typeof(InputField))
    self.PwInput = self.panelObj.transform:Find("Input/PwInput"):GetComponentInChildren(typeof(InputField))
    --Btn
    self.RegisterBtn = self.panelObj.transform:Find("RegisterButton"):GetComponent(typeof(Button))
    self.CloseBtn = self.panelObj.transform:Find("Btn_Close"):GetComponent(typeof(Button))

    self.RegisterBtn.onClick:AddListener(
        function()
            self:OnRegisterClick()
        end
    )
    self.CloseBtn.onClick:AddListener(
        function ()
            self:Close()
        end
    )
end

function RegisterPanel:Show()
    if self.panelObj == nil then
        self:Init()
    end
    self.panelObj:SetActive(true)
    RegisterHelper.OnShow()
end

function RegisterPanel:Close()
    self.panelObj:SetActive(false) 
    RegisterHelper.OnClose()
end

function RegisterPanel:OnRegisterClick()
    if self.UserInput.text == "" or self.PwInput.text == "" or self.UserNameInput.text == "" then
        print("用户名,账号和密码不能为空")
        --需要调用提示面板
        TipPanel:Show("用户名和密码不能为空")
        return
    end
    local msgRegister = MsgRegister()
    msgRegister.userName = self.UserNameInput.text
    print(self.UserNameInput.text)
    msgRegister.id = self.UserInput.text
    msgRegister.pw = self.PwInput.text
    NetManager.Send(msgRegister)
end
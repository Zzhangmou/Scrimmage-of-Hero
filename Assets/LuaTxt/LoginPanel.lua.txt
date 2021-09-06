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
            --添加点击事件
            self:OnLoginClick()
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
    LoginHelper.OnShow()
end

function LoginPanel:Close()
    self.panelObj:SetActive(false)
    LoginHelper.OnClose()
end

function LoginPanel:OnLoginClick()
    if self.UserInput.text == "" or self.PwInput.text == "" then
        print("用户名和密码不能为空")
        --需要调用提示面板
        TipPanel:Show("用户名和密码不能为空")
        return
    end
    local msgLogin = MsgLogin()
    msgLogin.id = self.UserInput.text
    msgLogin.pw = self.PwInput.text
    NetManager.Send(msgLogin)
end
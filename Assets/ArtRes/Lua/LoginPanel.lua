LoginPanel = {}

LoginPanel.panelObj = nil

LoginPanel.UserInput = nil
LoginPanel.PwInput = nil
LoginPanel.loginBtn = nil
LoginPanel.RegisterBtn = nil

function LoginPanel:Init()
    self.panelObj = ABManager:LoadRes("ui", "LoginPanel", typeof(GameObject))
    self.panelObj.transform:SetParent(Canvas, false)

    --InputText
    self.UserInput = self.panelObj.transform:Find("UserInput"):GetComponentInChildren(typeof(InputField))
    self.PwInput = self.panelObj.transform:Find("PwInput"):GetComponentInChildren(typeof(InputField))
    --Btn
    self.loginBtn = self.panelObj.transform:Find("LoginButton"):GetComponent(typeof(Button))
    self.RegisterBtn = self.panelObj.transform:Find("RegisterButton"):GetComponent(typeof(Button))
    self.CloseBtn = self.panelObj.transform:Find("Btn_Close"):GetComponent(typeof(Button))
    self.RememberMe = self.panelObj.transform:Find("Btn_Remember/RememberMe"):GetComponent(typeof(Button))

    self.rememberflag = self.panelObj.transform:Find("Btn_Remember/RememberMe/Flag")
    self.flag = false

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
    self.CloseBtn.onClick:AddListener(
        function ()
            self:Close()
        end
    )
    self.RememberMe.onClick:AddListener(
        function ()
            self:Remember()
        end
    )
end

function LoginPanel:Show()
    if self.panelObj == nil then
        self:Init()
    end
    self.panelObj:SetActive(true)
    self:LoginInTween()
    self:GetDefaultPassword()
    LoginHelper.OnShow()
    if PlayerPrefs.HasKey("Flag") then
        self.flag = true
        self.rememberflag.gameObject:SetActive(self.flag)
    end
end

function LoginPanel:Close()
    self:LoginOutTween()
    LoginHelper.OnClose()
end

function LoginPanel:OnLoginClick()
    if self.UserInput.text == "" or self.PwInput.text == "" then
        print("用户名和密码不能为空")
        --需要调用提示面板
        TipPanel:Show("用户名和密码不能为空")
        return
    end
    local id = self.UserInput.text
    local pw = self.PwInput.text
    self:SetDefaultPassword(tostring(id),tostring(pw))

    local msgLogin = MsgLogin()
    msgLogin.id = self.UserInput.text
    msgLogin.pw = self.PwInput.text
    print(self.UserInput.text)
    print(self.PwInput.text)

    NetManager.Send(msgLogin)
end

function LoginPanel:LoginInTween()
   self.panelObj.transform.localScale = Vector3.zero
   local time = 0.2
   self.panelObj.transform:DOScale(Vector3(1,1,1), time);
end

function LoginPanel:LoginOutTween()
    local time = 0.1
    local tween = self.panelObj.transform:DOScale(Vector3.zero, time);
    tween:OnComplete(
        function ()
            self.panelObj:SetActive(false) 
        end
        )
 end

 function LoginPanel:GetDefaultPassword()
    if PlayerPrefs.HasKey("Flag") and PlayerPrefs.HasKey("ID") and PlayerPrefs.HasKey("PW") then
        local id = PlayerPrefs.GetString("ID");
        local pw = PlayerPrefs.GetString("PW");
        self.UserInput.text = id
        self.PwInput.text = pw
    end
 end

 function LoginPanel:SetDefaultPassword(id,pw)
    PlayerPrefs.SetString("ID",id);
    PlayerPrefs.SetString("PW",pw);
    PlayerPrefs.Save()
 end

 function LoginPanel:Remember()
    self.flag = not self.flag
    self.rememberflag.gameObject:SetActive(self.flag)
    if self.flag then
        PlayerPrefs.SetString("Flag","true")
    else
        PlayerPrefs.DeleteKey("Flag")
    end
 end
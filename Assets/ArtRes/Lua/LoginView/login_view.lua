LoginView = LoginView or BaseClass(BaseView)

function LoginView:__init()
	self.ui_config = {"ui", "LoginPanel"}

    self.UserInput = nil
    self.PwInput = nil
    self.loginBtn = nil
    self.RegisterBtn = nil
    self.RememberMe = nil
    self.CloseBtn = nil
    self.flag = false
end

function LoginView:__delete()

end

function LoginView:ReleaseCallBack()
    self.UserInput = nil
    self.PwInput = nil
    self.loginBtn = nil
    self.RegisterBtn = nil
    self.RememberMe = nil
    self.CloseBtn = nil
    self.flag = false
end

function LoginView:LoadCallBack()
    self.root_node.transform:SetParent(Canvas, false)

    --InputText
    self.UserInput = self.root_node.transform:Find("UserInput"):GetComponentInChildren(typeof(InputField))
    self.PwInput = self.root_node.transform:Find("PwInput"):GetComponentInChildren(typeof(InputField))
    --Btn
    self.loginBtn = self.root_node.transform:Find("LoginButton"):GetComponent(typeof(Button))
    self.RegisterBtn = self.root_node.transform:Find("RegisterButton"):GetComponent(typeof(Button))
    self.CloseBtn = self.root_node.transform:Find("Btn_Close"):GetComponent(typeof(Button))
    self.RememberMe = self.root_node.transform:Find("Btn_Remember/RememberMe"):GetComponent(typeof(Button))

    self.rememberflag = self.root_node.transform:Find("Btn_Remember/RememberMe/Flag")
    self.flag = false

    self.loginBtn.onClick:AddListener(
        function()
            --添加点击事件
            self:OnLoginClick()
        end
    )
    self.RegisterBtn.onClick:AddListener(
        function()
            ViewManager.Instance:Open(ViewName.RegisterView)
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

function LoginView:OpenCallBack()
    self:LoginInTween()
    self:GetDefaultPassword()
    LoginHelper.OnShow()
    if PlayerPrefs.HasKey("Flag") then
        self.flag = true
        self.rememberflag.gameObject:SetActive(self.flag)
    end
end

function LoginView:CloseCallBack()
    -- self:LoginOutTween()
    LoginHelper.OnClose()
end

function LoginView:OnLoginClick()
    if self.UserInput.text == "" or self.PwInput.text == "" then
        --需要调用提示面板
        TipCtrl.Instance:ShowMessage("用户名和密码不能为空")
        return
    end
    local id = self.UserInput.text
    local pw = self.PwInput.text
    self:SetDefaultPassword(tostring(id),tostring(pw))

    local msgLogin = MsgLogin()
    msgLogin.id = self.UserInput.text
    msgLogin.pw = self.PwInput.text

    NetManager.Send(msgLogin)
end

function LoginView:LoginInTween()
   self.root_node.transform.localScale = Vector3.zero
   local time = 0.2
   self.root_node.transform:DOScale(Vector3(1,1,1), time);
end

function LoginView:LoginOutTween()
    local time = 0.1
    local tween = self.root_node.transform:DOScale(Vector3.zero, time);
    tween:OnComplete(
        function ()
            self.root_node:SetActive(false) 
        end
        )
 end

 function LoginView:GetDefaultPassword()
    if PlayerPrefs.HasKey("Flag") and PlayerPrefs.HasKey("ID") and PlayerPrefs.HasKey("PW") then
        local id = PlayerPrefs.GetString("ID");
        local pw = PlayerPrefs.GetString("PW");
        self.UserInput.text = id
        self.PwInput.text = pw
    end
 end

 function LoginView:SetDefaultPassword(id,pw)
    PlayerPrefs.SetString("ID",id);
    PlayerPrefs.SetString("PW",pw);
    PlayerPrefs.Save()
 end

 function LoginView:Remember()
    self.flag = not self.flag
    self.rememberflag.gameObject:SetActive(self.flag)
    if self.flag then
        PlayerPrefs.SetString("Flag","true")
    else
        PlayerPrefs.DeleteKey("Flag")
    end
 end
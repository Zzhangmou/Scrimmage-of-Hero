RegisterView = RegisterView or BaseClass(BaseView)

function RegisterView:__init()
	self.ui_config = {"ui", "RegisterPanel"}

    self.UserNameInput = nil
    self.UserInput = nil
    self.PwInput = nil
    self.RegisterBtn = nil
end

function RegisterView:__delete()

end

function RegisterView:ReleaseCallBack()
    self.UserInput = nil
    self.PwInput = nil
    self.loginBtn = nil
    self.RegisterBtn = nil
    self.flag = false
end

function RegisterView:LoadCallBack()
    self.root_node.transform:SetParent(Canvas, false)
    --InputText
    self.UserNameInput = self.root_node.transform:Find("Input/UserNameInput"):GetComponentInChildren(typeof(InputField))
    self.UserInput = self.root_node.transform:Find("Input/UserInput"):GetComponentInChildren(typeof(InputField))
    self.PwInput = self.root_node.transform:Find("Input/PwInput"):GetComponentInChildren(typeof(InputField))
    --Btn
    self.RegisterBtn = self.root_node.transform:Find("RegisterButton"):GetComponent(typeof(Button))
    self.CloseBtn = self.root_node.transform:Find("Btn_Close"):GetComponent(typeof(Button))

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

function RegisterView:OpenCallBack()
    RegisterHelper.OnShow()
end

function RegisterView:CloseCallBack()
    RegisterHelper.OnClose()
end

function RegisterView:OnRegisterClick()
    if self.UserInput.text == "" or self.PwInput.text == "" or self.UserNameInput.text == "" then
        --需要调用提示面板
        TipCtrl.Instance:ShowMessage("用户名和密码不能为空")
        return
    end
    local msgRegister = MsgRegister()
    msgRegister.userName = self.UserNameInput.text
    msgRegister.id = self.UserInput.text
    msgRegister.pw = self.PwInput.text
    NetManager.Send(msgRegister)
end
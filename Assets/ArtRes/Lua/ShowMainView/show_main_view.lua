ShowMainView = ShowMainView or BaseClass(BaseView)

function ShowMainView:__init()
	self.ui_config = {"ui", "ShowMainPanel"}

    --对应的面板控件
    self.showHeroRImage = nil
    self.showHeroBtn = nil
    self.setBtn = nil
    self.mapBtn = nil
    self.startBtn = nil
    --UserInfo
    self.UserNameText = nil
    self.UserRecordText = nil
end

function ShowMainView:__delete()

end

function ShowMainView:ReleaseCallBack()
    self.showHeroRImage = nil
    self.showHeroBtn = nil
    self.setBtn = nil
    self.mapBtn = nil
    self.startBtn = nil
    self.UserNameText = nil
    self.UserRecordText = nil
end

function ShowMainView:LoadCallBack()
    self.root_node.transform:SetParent(Canvas, false)
    self.UserNameText = self.root_node.transform:Find("UserInfo/UserNameText"):GetComponent(typeof(Text))
    self.UserRecordText = self.root_node.transform:Find("UserInfo/UserRecordText"):GetComponent(typeof(Text))
    --重新赋值RenderText
    self.showHeroRImage = self.root_node.transform:Find("ShowHeroRawImage"):GetComponent(typeof(RawImage))
    self.showHeroRImage.texture = Resources.Load("Target")
    self.showHeroRImage.gameObject:AddComponent(typeof(CS.Scrimmage.ObjTouchRotate))
    --Btn
    self.showHeroBtn = self.root_node.transform:Find("ShowHeroRawImage"):GetComponent(typeof(Button))
    self.startBtn = self.root_node.transform:Find("StartButton"):GetComponent(typeof(Button))
    self.mapBtn = self.root_node.transform:Find("MapButton"):GetComponent(typeof(Button))
    self.heroBtn = self.root_node.transform:Find("HeroButton"):GetComponent(typeof(Button))
    self.setBtn = self.root_node.transform:Find("SetButton"):GetComponent(typeof(Button))
    --如果直接.传入自己的函数 在函数内部 无法使用self获取内容
    --self.showHeroBtn.onClick:AddListener(self.ShowChoiceHeroPanel)
    self.showHeroBtn.onClick:AddListener(
        function()
            ViewManager.Instance:Open(ViewName.ChoiceHeroView)
        end
    )
    self.startBtn.onClick:AddListener(
        function()
            ViewManager.Instance:Open(ViewName.MatchView)
        end
    )
    self.mapBtn.onClick:AddListener(
        function()
            TipCtrl.Instance:ShowMessage("该功能暂未开启!!!")
        end
    )
    self.setBtn.onClick:AddListener(
        function()
            ViewManager.Instance:Open(ViewName.SettingView)
        end
    )
    self.heroBtn.onClick:AddListener(
        function()
            ViewManager.Instance:Open(ViewName.ChoiceHeroView)
        end
    )
end

function ShowMainView:OpenCallBack()
    GameMainHelper:OnShow()
    --发送协议
    local msgGetUserInfo = MsgGetUserInfo()
    NetManager.Send(msgGetUserInfo)
end

function ShowMainView:CloseCallBack()
    GameMainHelper:OnClose()
end

function ShowMainView:SetUserInfo(userName, userRecord)
    self.UserNameText.text = userName
    self.UserRecordText.text = userRecord
end
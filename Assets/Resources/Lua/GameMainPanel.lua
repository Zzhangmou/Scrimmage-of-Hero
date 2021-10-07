GameMainPanel = {}

--关联的面板对象
GameMainPanel.panelObj = nil
GameMainPanel.heroId = 5
--对应的面板控件
GameMainPanel.showHeroRImage = nil
GameMainPanel.showHeroBtn = nil
GameMainPanel.setBtn = nil
GameMainPanel.mapBtn = nil
GameMainPanel.startBtn = nil
--UserInfo
GameMainPanel.UserNameText = nil
GameMainPanel.UserRecordText = nil

function GameMainPanel:Init()
    self.panelObj = ABMgr:LoadRes("ui", "GameMainPanel", typeof(GameObject))
    self.panelObj.transform:SetParent(Canvas, false)

    self.UserNameText = self.panelObj.transform:Find("UserInfo/UserNameText"):GetComponent(typeof(Text))
    self.UserRecordText = self.panelObj.transform:Find("UserInfo/UserRecordText"):GetComponent(typeof(Text))
    --重新赋值RenderText
    self.showHeroRImage = self.panelObj.transform:Find("ShowHeroRawImage"):GetComponent(typeof(RawImage))
    self.showHeroRImage.texture = Resources.Load("Target")

    --Btn
    self.showHeroBtn = self.panelObj.transform:Find("ShowHeroRawImage"):GetComponent(typeof(Button))
    self.startBtn = self.panelObj.transform:Find("StartButton"):GetComponent(typeof(Button))
    --如果直接.传入自己的函数 在函数内部 无法使用self获取内容
    --self.showHeroBtn.onClick:AddListener(self.ShowChoiceHeroPanel)
    self.showHeroBtn.onClick:AddListener(
        function()
            self:ShowChoiceHeroPanel()
        end
    )
    self.startBtn.onClick:AddListener(
        function()
            MatchPanel:Show()
        end
    )
end

function GameMainPanel:Show()
    if self.panelObj == nil then
        self:Init()
    end
    self.panelObj:SetActive(true)
    GameMainHelper:OnShow()
    --发送协议
    local msgGetUserInfo = MsgGetUserInfo()
    NetManager.Send(msgGetUserInfo)
end

function GameMainPanel:Close()
    self.panelObj:SetActive(false)
    GameMainHelper:OnClose()
end

function GameMainPanel:ShowChoiceHeroPanel()
    ChoiceHeroPanel:Show()
end

function GameMainPanel:SetUserInfo(userName, userRecord)
    self.UserNameText.text = userName
    self.UserRecordText.text = userRecord
end

MatchPanel = {}
--关联的面板对象
MatchPanel.panelObj = nil
--对应的面板控件
MatchPanel.showText = nil
MatchPanel.returnBtn = nil

function MatchPanel:Init()
    self.panelObj = ABMgr:LoadRes("ui", "MatchPanel", typeof(GameObject))
    self.panelObj.transform:SetParent(Canvas, false)

    self.showText = self.panelObj.transform:Find("ShowText"):GetComponent(typeof(Text))

    --Btn
    self.returnBtn = self.panelObj.transform:Find("ReturnButton"):GetComponent(typeof(Button))
    --如果直接.传入自己的函数 在函数内部 无法使用self获取内容
    self.returnBtn.onClick:AddListener(
        function()
            self:Close()
            self:SendMsgLeavematch()
        end
    )
end

function MatchPanel:Show()
    if self.panelObj == nil then
        self:Init()
    end
    self.panelObj:SetActive(true)
    MatchHelper:OnShow()
    local msgStartMatch = MsgStartMatch()
    --获取玩家当前使用的角色id
    print(GameMainPanel.heroId)
    msgStartMatch.heroId = GameMainPanel.heroId
    NetManager.Send(msgStartMatch)
end

function MatchPanel:Close()
    self.panelObj:SetActive(false)
    MatchHelper:OnClose()
end

--发送离开协议
function MatchPanel:SendMsgLeavematch()
    local msgLeaveMatch = MsgLeaveMatch()
    NetManager.Send(msgLeaveMatch)
end

function MatchPanel:UpdateText(text)
    self.showText.text = text
end

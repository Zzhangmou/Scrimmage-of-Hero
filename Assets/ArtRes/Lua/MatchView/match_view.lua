MatchView = MatchView or BaseClass(BaseView)

function MatchView:__init()
	self.ui_config = {"ui", "MatchPanel"}

    --对应的面板控件
    self.showText = nil
    self.returnBtn = nil
end

function MatchView:__delete()

end

function MatchView:ReleaseCallBack()
    self.showText = nil
    self.returnBtn = nil
end

function MatchView:LoadCallBack()
    self.root_node.transform:SetParent(Canvas, false)

    self.showText = self.root_node.transform:Find("ShowText"):GetComponent(typeof(Text))

    --Btn
    self.returnBtn = self.root_node.transform:Find("ReturnButton"):GetComponent(typeof(Button))
    --如果直接.传入自己的函数 在函数内部 无法使用self获取内容
    self.returnBtn.onClick:AddListener(
        function()
            self:Close()
            self:SendMsgLeavematch()
        end
    )
end

function MatchView:OpenCallBack()
    MatchHelper:OnShow()
    local msgStartMatch = MsgStartMatch()
    --获取玩家当前使用的角色id
    msgStartMatch.heroId = ShowMainData.Instance:GetHeroId()
    NetManager.Send(msgStartMatch)
end

function MatchView:CloseCallBack()
    MatchHelper:OnClose()
end

--发送离开协议
function MatchView:SendMsgLeavematch()
    local msgLeaveMatch = MsgLeaveMatch()
    NetManager.Send(msgLeaveMatch)
end

function MatchView:UpdateText(text)
    self.showText.text = text
end
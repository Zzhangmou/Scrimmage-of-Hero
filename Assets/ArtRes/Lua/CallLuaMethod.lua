--CSharpCallLua

CallLuaMethod = CallLuaMethod or {}

--ViewManager Open
function CallLuaMethod:OpenView(panelName)
    ViewManager.Instance:Open(panelName)
end

function CallLuaMethod:CloseView(panelName)
    ViewManager.Instance:Close(panelName)
end
--TipCtrl
function  CallLuaMethod:ShowMessage(message)
    TipCtrl.Instance:ShowMessage(message)
end

function CallLuaMethod:ShowHero(heroId)
    ProgressCtrl.Instance:ShowHero(heroId)
end

function CallLuaMethod:ChangeSliderValue(value)
    ProgressCtrl.Instance:ChangeSliderValue(value)
end

function CallLuaMethod:SetUserInfo(userName, userRecord)
    ShowMainCtrl.Instance:SetUserInfo(userName, userRecord)
end

function CallLuaMethod:UpdateText(text)
    MatchCtrl.Instance:UpdateText(text)
end

function CallLuaMethod:InitBattleMessage(camp, heroId, id)
    BattleMessageCtrl.Instance:InitBattleMessage(camp, heroId, id)
end

function CallLuaMethod:FlushData(camp, id)
    BattleMessageCtrl.Instance:FlushData(camp, id)
end
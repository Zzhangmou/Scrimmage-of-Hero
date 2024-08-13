SettingView = SettingView or BaseClass(BaseView)

local EndPos = Vector2(-200,0)
local StartPos = Vector2(200,0)
local moveTime = 0.25

function SettingView:__init()
	self.ui_config = {"ui", "SettingPanel"}
end

function SettingView:__delete()

end

function SettingView:ReleaseCallBack()

end

function SettingView:LoadCallBack()
    self.root_node.transform:SetParent(TipCanvas, false)
    self.closeBtn = self.root_node.transform:Find("CloseBg"):GetComponent(typeof(Button))
    self.quickBtn = self.root_node.transform:Find("ShowArea/Button_1"):GetComponent(typeof(Button))

    self.showArea = self.root_node.transform:Find("ShowArea")
    self.closeBtn.onClick:AddListener(
        function()
            self:Close()
        end
    )
    self.quickBtn.onClick:AddListener(
        function()
            self:QuitGame()
        end
    )
end

function SettingView:Show(heroId)
    if self.root_node == nil then
        self:Init()
    end
    self.root_node:SetActive(true) 
    self.showArea.transform.anchoredPosition = StartPos
    self.showArea.transform:DOAnchorPos(EndPos, moveTime);
end

function SettingView:Close()
    local tween = self.showArea.transform:DOAnchorPos(StartPos, moveTime);
    tween:OnComplete(
        function ()
            self.root_node:SetActive(false) 
        end
        )
end

function SettingView:QuitGame()
    CS.UnityEngine.Application.Quit()
end
SettingPanel = {}

SettingPanel.panelObj = nil

local EndPos = Vector2(-200,0)
local StartPos = Vector2(200,0)
local moveTime = 0.25

function SettingPanel:Init()
    self.panelObj = ABManager:LoadRes("ui", "SettingPanel", typeof(GameObject))
    self.panelObj.transform:SetParent(TipCanvas, false)

    self.closeBtn = self.panelObj.transform:Find("CloseBg"):GetComponent(typeof(Button))
    self.quickBtn = self.panelObj.transform:Find("ShowArea/Button_1"):GetComponent(typeof(Button))

    self.showArea = self.panelObj.transform:Find("ShowArea")
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
function SettingPanel:Show(heroId)
    if self.panelObj == nil then
        self:Init()
    end
    self.panelObj:SetActive(true) 
    self.showArea.transform.anchoredPosition = StartPos
    self.showArea.transform:DOAnchorPos(EndPos, moveTime);
end

function SettingPanel:Close()
    local tween = self.showArea.transform:DOAnchorPos(StartPos, moveTime);
    tween:OnComplete(
        function ()
            self.panelObj:SetActive(false) 
        end
        )
end

function SettingPanel:QuitGame()
    CS.UnityEngine.Application.Quit()
end
StartShowPanel = {}

StartShowPanel.panelObj = nil

StartShowPanel.loginBtn = nil

local setEndPos = Vector2(0, 288)
local setStartPos = Vector2(0, -288)
local moveTime = 0.15

function StartShowPanel:Init()
    self.isopenSet = false

    self.panelObj = ABMgr:LoadRes("ui", "StartShowPanel", typeof(GameObject))
    self.panelObj.transform:SetParent(Canvas, false)
    self.areaObj = self.panelObj.transform:Find("SetArea")
    self.areaObj.gameObject:SetActive(false)
    -- Btn
    self.loginBtn = self.panelObj.transform:Find("LoginButton"):GetComponent(typeof(Button))
    self.linkBtn = self.panelObj.transform:Find("SetArea/LinkButton"):GetComponent(typeof(Button))
    self.setBtn = self.panelObj.transform:Find("SetButton"):GetComponent(typeof(Button))
    self.testlinkBtn = self.panelObj.transform:Find("SetArea/TestButton"):GetComponent(typeof(Button))

    self.loginBtn.onClick:AddListener(function()
        self:EnterLogin()
    end)

    self.linkBtn.onClick:AddListener(function()
        self:Link()
    end)

    self.testlinkBtn.onClick:AddListener(function()
        self:TestLink()
    end)

    self.setBtn.onClick:AddListener(function()
        self:StartTween()
    end)
end

function StartShowPanel:StartTween()
    if not self.isopenSet then
        self.areaObj.transform.anchoredPosition = setStartPos
        self.areaObj.gameObject:SetActive(true)
        self.areaObj.transform:DOAnchorPos(setEndPos, moveTime)
    else
        local endMove = self.areaObj.transform:DOAnchorPos(setStartPos, moveTime)
        endMove:OnComplete(function()
            self.areaObj.gameObject:SetActive(false)
        end)
    end
    self.isopenSet = not self.isopenSet
end

function StartShowPanel:EnterLogin()
    -- 判断服务器是否连接
    local desc = CS.NetWorkFK.NetManager.GetDesc()
    if desc == "" then
        TipPanel:Show("服务器还未连接,暂且不能登录")
        return
    end
    LoginPanel:Show()
end

function StartShowPanel:TestLink()
    CS.NetWorkFK.NetManager.Connect("127.0.0.1", 18188)
    self:StartTween()
end

function StartShowPanel:Link()
    local ip = self.panelObj.transform:Find("SetArea/InputIP"):GetComponent(typeof(InputField))
    local port = self.panelObj.transform:Find("SetArea/InputPort"):GetComponent(typeof(InputField))
    if port.text == "" then
        CS.NetWorkFK.NetManager.Connect("127.0.0.1", 18188)
    else
        CS.NetWorkFK.NetManager.Connect(ip.text, port.text)
    end
    self:StartTween()
end

function StartShowPanel:Show()
    if self.panelObj == nil then
        self:Init()
    end
    self.panelObj:SetActive(true)
end

function StartShowPanel:Close()
    self.panelObj:SetActive(false)
end

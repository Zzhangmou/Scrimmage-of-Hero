StartShowView = StartShowView or BaseClass(BaseView)

local setEndPos = Vector2(0, 288)
local setStartPos = Vector2(0, -288)
local moveTime = 0.15

function StartShowView:__init()
	self.ui_config = {"ui", "StartShowPanel"}

    self.isopenSet = false

    self.areaObj = nil
    self.loginBtn = nil
    self.linkBtn = nil
    self.setBtn = nil
    self.testlinkBtn = nil
end

function StartShowView:__delete()

end

function StartShowView:ReleaseCallBack()
    self.isopenSet = false

    self.areaObj = nil
    self.loginBtn = nil
    self.linkBtn = nil
    self.setBtn = nil
    self.testlinkBtn = nil
end

function StartShowView:LoadCallBack()
    self.root_node.transform:SetParent(Canvas, false)
    self.areaObj = self.root_node.transform:Find("SetArea")
    self.areaObj.gameObject:SetActive(false)
    -- Btn
    self.loginBtn = self.root_node.transform:Find("LoginButton"):GetComponent(typeof(Button))
    self.linkBtn = self.root_node.transform:Find("SetArea/LinkButton"):GetComponent(typeof(Button))
    self.setBtn = self.root_node.transform:Find("SetButton"):GetComponent(typeof(Button))
    self.testlinkBtn = self.root_node.transform:Find("SetArea/TestButton"):GetComponent(typeof(Button))

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

function StartShowView:StartTween()
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

function StartShowView:EnterLogin()
    StartShowCtrl.Instance:EnterLogin()
end

function StartShowView:TestLink()
    StartShowCtrl.Instance:TestLink(function ()
        self:StartTween()
    end)
end

function StartShowView:Link()
    local ip = self.root_node.transform:Find("SetArea/InputIP"):GetComponent(typeof(InputField))
    local port = self.root_node.transform:Find("SetArea/InputPort"):GetComponent(typeof(InputField))
    StartShowCtrl.Instance:Link(ip.text, port.text, function ()
        self:StartTween()
    end)
end
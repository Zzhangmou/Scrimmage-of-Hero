require("StartShowView/start_show_view")
require("StartShowView/start_show_data")

StartShowCtrl = StartShowCtrl or BaseClass(BaseController)

function StartShowCtrl:__init()
	if nil ~= StartShowCtrl.Instance then
		print("[StartShowCtrl] Attemp to create a singleton twice !")
		return
	end
	StartShowCtrl.Instance = self
	self.start_show_view = StartShowView.New(ViewName.StartShowView)
	self.start_show_data = StartShowData.New()
end

function StartShowCtrl:__delete()
	if self.start_show_view ~= nil then
		self.start_show_view:DeleteMe()
		self.start_show_view = nil
	end

	if self.start_show_data ~= nil then
		self.start_show_data:DeleteMe()
		self.start_show_data = nil
	end
	StartShowCtrl.Instance = nil
end

function StartShowCtrl:ShowPanel()
    self.start_show_view:Open()
end

function  StartShowCtrl:TestLink(callBack)
    CS.NetWorkFK.NetManager.Connect("127.0.0.1", 18188)
    callBack()
end

function StartShowCtrl:Link(ip, text, callBack)
    if text == "" then
        CS.NetWorkFK.NetManager.Connect("127.0.0.1", 18188)
    else
        CS.NetWorkFK.NetManager.Connect(ip, text)
    end
    callBack()
end

function StartShowCtrl:EnterLogin(callBack)
    -- 判断服务器是否连接
    local desc = CS.NetWorkFK.NetManager.GetDesc()
    if desc == "" then
        TipCtrl.Instance:ShowMessage("服务器还未连接,暂且不能登录")
        return
    end
    ViewManager.Instance:Open(ViewName.LoginView)
end
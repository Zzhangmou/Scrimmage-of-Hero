require("ControlView/control_view")

ControlCtrl = ControlCtrl or BaseClass(BaseController)

function ControlCtrl:__init()
	if nil ~= ControlCtrl.Instance then
		print("[ControlCtrl] Attemp to create a singleton twice !")
		return
	end
	ControlCtrl.Instance = self
	self.control_view = ControlView.New(ViewName.ControlView)
end

function ControlCtrl:__delete()
	if self.control_view ~= nil then
		self.control_view:DeleteMe()
		self.control_view = nil
	end

	ControlCtrl.Instance = nil
end
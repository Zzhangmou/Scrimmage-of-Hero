require("RegisterView/register_view")

RegisterCtrl = RegisterCtrl or BaseClass(BaseController)

function RegisterCtrl:__init()
	if nil ~= RegisterCtrl.Instance then
		print("[RegisterCtrl] Attemp to create a singleton twice !")
		return
	end
	RegisterCtrl.Instance = self
	self.register_view = RegisterView.New(ViewName.RegisterView)
end

function RegisterCtrl:__delete()
	if self.register_view ~= nil then
		self.register_view:DeleteMe()
		self.register_view = nil
	end

	RegisterCtrl.Instance = nil
end
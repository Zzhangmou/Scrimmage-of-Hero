require("LoginView/login_view")
require("LoginView/login_data")

LoginCtrl = LoginCtrl or BaseClass(BaseController)

function LoginCtrl:__init()
	if nil ~= LoginCtrl.Instance then
		print("[LoginCtrl] Attemp to create a singleton twice !")
		return
	end
	LoginCtrl.Instance = self
	self.start_show_view = LoginView.New(ViewName.LoginView)
	self.start_show_data = LoginData.New()
end

function LoginCtrl:__delete()
	if self.start_show_view ~= nil then
		self.start_show_view:DeleteMe()
		self.start_show_view = nil
	end

	if self.start_show_data ~= nil then
		self.start_show_data:DeleteMe()
		self.start_show_data = nil
	end
	LoginCtrl.Instance = nil
end

function LoginCtrl:ShowPanel()
    self.start_show_view:Open()
end
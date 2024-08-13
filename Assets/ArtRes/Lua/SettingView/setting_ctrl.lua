require("SettingView/setting_view")

SettingCtrl = SettingCtrl or BaseClass(BaseController)

function SettingCtrl:__init()
	if nil ~= SettingCtrl.Instance then
		print("[SettingCtrl] Attemp to create a singleton twice !")
		return
	end
	SettingCtrl.Instance = self
	self.setting_view = SettingView.New(ViewName.SettingView)
end

function SettingCtrl:__delete()
	if self.setting_view ~= nil then
		self.setting_view:DeleteMe()
		self.setting_view = nil
	end

	SettingCtrl.Instance = nil
end
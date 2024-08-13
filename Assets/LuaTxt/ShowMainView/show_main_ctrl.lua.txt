require("ShowMainView/show_main_view")
require("ShowMainView/show_main_data")

ShowMainCtrl = ShowMainCtrl or BaseClass(BaseController)

function ShowMainCtrl:__init()
	if nil ~= ShowMainCtrl.Instance then
		print("[ShowMainCtrl] Attemp to create a singleton twice !")
		return
	end
	ShowMainCtrl.Instance = self
	self.show_main_view = ShowMainView.New(ViewName.ShowMainView)
	self.show_main_data = ShowMainData.New()
end

function ShowMainCtrl:__delete()
	if self.show_main_view ~= nil then
		self.show_main_view:DeleteMe()
		self.show_main_view = nil
	end

	if self.show_main_data ~= nil then
		self.show_main_data:DeleteMe()
		self.show_main_data = nil
	end

	ShowMainCtrl.Instance = nil
end

function ShowMainCtrl:SetUserInfo(userName, userRecord)
	if(not ViewManager.Instance:IsOpen(ViewName.ShowMainView)) then
        ViewManager.Instance:Open(ViewName.ShowMainView)
    end
	self.show_main_view:SetUserInfo(userName, userRecord)
end
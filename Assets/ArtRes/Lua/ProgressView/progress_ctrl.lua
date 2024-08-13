require("ProgressView/progress_view")

ProgressCtrl = ProgressCtrl or BaseClass(BaseController)

function ProgressCtrl:__init()
	if nil ~= ProgressCtrl.Instance then
		print("[ProgressCtrl] Attemp to create a singleton twice !")
		return
	end
	ProgressCtrl.Instance = self
	self.progress_view = ProgressView.New(ViewName.ProgressView)
end

function ProgressCtrl:__delete()
	if self.progress_view ~= nil then
		self.progress_view:DeleteMe()
		self.progress_view = nil
	end

	ProgressCtrl.Instance = nil
end

function ProgressCtrl:ShowHero(heroId)
	if(not ViewManager.Instance:IsOpen(ViewName.ProgressView)) then
        ViewManager.Instance:Open(ViewName.ProgressView)
    end
	self.progress_view:ShowHero(heroId)
end

function ProgressCtrl:ChangeSliderValue(value)
	if(not ViewManager.Instance:IsOpen(ViewName.ProgressView)) then
        ViewManager.Instance:Open(ViewName.ProgressView)
    end
	self.progress_view:ChangeSliderValue(value)
end
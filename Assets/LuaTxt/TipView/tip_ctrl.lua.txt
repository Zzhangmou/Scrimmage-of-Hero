require("TipView/tip_view")

TipCtrl = TipCtrl or BaseClass(BaseController)

function TipCtrl:__init()
	if nil ~= TipCtrl.Instance then
		print("[TipCtrl] Attemp to create a singleton twice !")
		return
	end
	TipCtrl.Instance = self
	self.tip_view = TipView.New(ViewName.TipView)
end

function TipCtrl:__delete()
	if self.tip_view ~= nil then
		self.tip_view:DeleteMe()
		self.tip_view = nil
	end

	TipCtrl.Instance = nil
end

function TipCtrl:ShowMessage(message)
    if(not ViewManager.Instance:IsOpen(ViewName.TipView)) then
        ViewManager.Instance:Open(ViewName.TipView)
    end
    self.tip_view:ShowMessage(message)
end
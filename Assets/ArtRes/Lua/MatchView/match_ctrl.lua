require("MatchView/match_view")

MatchCtrl = MatchCtrl or BaseClass(BaseController)

function MatchCtrl:__init()
	if nil ~= MatchCtrl.Instance then
		print("[MatchCtrl] Attemp to create a singleton twice !")
		return
	end
	MatchCtrl.Instance = self
	self.match_view = MatchView.New(ViewName.MatchView)
end

function MatchCtrl:__delete()
	if self.match_view ~= nil then
		self.match_view:DeleteMe()
		self.match_view = nil
	end

	MatchCtrl.Instance = nil
end

function MatchCtrl:UpdateText(text)
	if(not ViewManager.Instance:IsOpen(ViewName.MatchView)) then
        ViewManager.Instance:Open(ViewName.MatchView)
    end
	self.match_view:UpdateText(text)
end
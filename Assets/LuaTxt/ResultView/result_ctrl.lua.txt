require("ResultView/result_view")

ResultCtrl = ResultCtrl or BaseClass(BaseController)

function ResultCtrl:__init()
	if nil ~= ResultCtrl.Instance then
		print("[ResultCtrl] Attemp to create a singleton twice !")
		return
	end
	ResultCtrl.Instance = self
	self.result_view = ResultView.New(ViewName.ResultView)
end

function ResultCtrl:__delete()
	if self.result_view ~= nil then
		self.result_view:DeleteMe()
		self.result_view = nil
	end

	ResultCtrl.Instance = nil
end
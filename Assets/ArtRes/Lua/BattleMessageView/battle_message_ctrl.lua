require("BattleMessageView/battle_message_view")

BattleMessageCtrl = BattleMessageCtrl or BaseClass(BaseController)

function BattleMessageCtrl:__init()
	if nil ~= BattleMessageCtrl.Instance then
		print("[BattleMessageCtrl] Attemp to create a singleton twice !")
		return
	end
	BattleMessageCtrl.Instance = self
	self.battle_message_view = BattleMessageView.New(ViewName.BattleMessageView)
end

function BattleMessageCtrl:__delete()
	if self.battle_message_view ~= nil then
		self.battle_message_view:DeleteMe()
		self.battle_message_view = nil
	end

	BattleMessageCtrl.Instance = nil
end

function BattleMessageCtrl:InitBattleMessage(camp,heroId,id)
	if not ViewManager.Instance:IsOpen(ViewName.BattleMessageView) then
		ViewManager.Instance:Open(ViewName.BattleMessageView)
	end 
	self.battle_message_view:InitBattleMessage(camp,heroId,id)
end

function BattleMessageCtrl:FlushData(camp,id)
	if not ViewManager.Instance:IsOpen(ViewName.BattleMessageView) then
		ViewManager.Instance:Open(ViewName.BattleMessageView)
	end 
	self.battle_message_view:FlushData(camp,id)
end
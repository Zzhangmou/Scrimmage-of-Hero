ModulesController = ModulesController or BaseClass()

function ModulesController:__init()
	if ModulesController.Instance ~= nil then
		print("[ModulesController] attempt to create singleton twice!")
		return
	end
	ModulesController.Instance = self

    self:CreateCoreModule()

	self.ctrl_list = {}
	self.push_list = {}
	self.cur_index = 0

	self:Start()
end

function ModulesController:__delete()
	self:DeleteGameModule()

	ViewManager.Instance:DeleteMe()

	ModulesController.Instance = nil
end

function ModulesController:Start(call_back)
    self.push_list = {
		StartShowCtrl,
		ShowMainCtrl,
		ChoiceHeroCtrl,
		LoginCtrl,
		RegisterCtrl,
		TipCtrl,
		MatchCtrl,
		ProgressCtrl,
		ControlCtrl,
		ResultCtrl,
		HeroShowCtrl,
		SettingCtrl,
		BattleMessageCtrl,
    }
end

function ModulesController:CreateCoreModule()
	ViewManager.New()
end

function ModulesController:CreateGameModule()
	for k, v in pairs(self.push_list) do
		if nil == v.Instance then
			table.insert(self.ctrl_list, v.New())
		end
	end
end

function ModulesController:DeleteGameModule()
	local count = #self.ctrl_list
	for i = count, 1, -1 do
		self.ctrl_list[i]:DeleteMe()
	end
	self.ctrl_list = {}
end
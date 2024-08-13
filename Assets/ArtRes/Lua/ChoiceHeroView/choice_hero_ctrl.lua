require("ChoiceHeroView/choice_hero_view")
require("ChoiceHeroView/choice_hero_data")

ChoiceHeroCtrl = ChoiceHeroCtrl or BaseClass(BaseController)

function ChoiceHeroCtrl:__init()
	if nil ~= ChoiceHeroCtrl.Instance then
		print("[ChoiceHeroCtrl] Attemp to create a singleton twice !")
		return
	end
	ChoiceHeroCtrl.Instance = self
	self.show_main_view = ChoiceHeroView.New(ViewName.ChoiceHeroView)
	self.show_main_data = ChoiceHeroData.New()
end

function ChoiceHeroCtrl:__delete()
	if self.show_main_view ~= nil then
		self.show_main_view:DeleteMe()
		self.show_main_view = nil
	end

	if self.show_main_data ~= nil then
		self.show_main_data:DeleteMe()
		self.show_main_data = nil
	end
	ChoiceHeroCtrl.Instance = nil
end

function ChoiceHeroCtrl:ShowPanel()
    self.show_main_view:Open()
end
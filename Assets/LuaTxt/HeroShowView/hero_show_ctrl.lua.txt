require("HeroShowView/hero_show_view")

HeroShowCtrl = HeroShowCtrl or BaseClass(BaseController)

function HeroShowCtrl:__init()
	if nil ~= HeroShowCtrl.Instance then
		print("[HeroShowCtrl] Attemp to create a singleton twice !")
		return
	end
	HeroShowCtrl.Instance = self
	self.hero_show_view = HeroShowView.New(ViewName.HeroShowView)
end

function HeroShowCtrl:__delete()
	if self.hero_show_view ~= nil then
		self.hero_show_view:DeleteMe()
		self.hero_show_view = nil
	end

	HeroShowCtrl.Instance = nil
end

function HeroShowCtrl:UpdateHeroDesc(heroId)
   if(not ViewManager.Instance:IsOpen(ViewName.HeroShowView)) then
        ViewManager.Instance:Open(ViewName.HeroShowView)
   end
   self.hero_show_view:UpdateHeroDesc(heroId)
end
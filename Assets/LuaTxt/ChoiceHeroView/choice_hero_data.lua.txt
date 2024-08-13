ChoiceHeroData = ChoiceHeroData or BaseClass()

function ChoiceHeroData:__init()
	if ChoiceHeroData.Instance ~= nil then
		print("[ChoiceHeroData] Attemp to create a singleton twice !")
		return
	end
	ChoiceHeroData.Instance = self
end

function ChoiceHeroData:__delete()
	if ChoiceHeroData.Instance ~= nil then
		ChoiceHeroData.Instance = nil
	end
end
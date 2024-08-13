ShowMainData = ShowMainData or BaseClass()

function ShowMainData:__init()
	if ShowMainData.Instance ~= nil then
		print("[ShowMainData] Attemp to create a singleton twice !")
		return
	end
	ShowMainData.Instance = self

    self.heroId = 5
end

function ShowMainData:__delete()

	if ShowMainData.Instance ~= nil then
		ShowMainData.Instance = nil
	end
end

function ShowMainData:SetHeroId(id)
    self.heroId = id
end

function  ShowMainData:GetHeroId()
    return self.heroId
end
StartShowData = StartShowData or BaseClass()

function StartShowData:__init()
	if StartShowData.Instance ~= nil then
		print("[StartShowData] Attemp to create a singleton twice !")
		return
	end
	StartShowData.Instance = self
end

function StartShowData:__delete()

	if StartShowData.Instance ~= nil then
		StartShowData.Instance = nil
	end
end
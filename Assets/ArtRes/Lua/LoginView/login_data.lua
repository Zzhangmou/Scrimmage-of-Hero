LoginData = LoginData or BaseClass()

function LoginData:__init()
	if LoginData.Instance ~= nil then
		print("[LoginData] Attemp to create a singleton twice !")
		return
	end
	LoginData.Instance = self
end

function LoginData:__delete()

	if LoginData.Instance ~= nil then
		LoginData.Instance = nil
	end
end
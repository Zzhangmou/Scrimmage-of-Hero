ViewManager = ViewManager or BaseClass()

function ViewManager:__init()
	if nil ~= ViewManager.Instance then
		print("[ViewManager]:Attempt to create singleton twice!")
	end
	ViewManager.Instance = self

	self.view_list = {}

	self.open_view_list = {}

	self.wait_load_chat_list = {}
end

function ViewManager:__delete()
	ViewManager.Instance = nil
end

function ViewManager:DestoryAllAndClear()
	for k,v in pairs(self.view_list) do
		if v:IsOpen() then
			v:Close()
			v:Release()
		end
	end

	self.view_list = {}
	self.open_view_list = {}
	self.wait_load_chat_list = {}
end

-- 注册一个界面
function ViewManager:RegisterView(view, view_name)
	self.view_list[view_name] = view
end

-- 反注册一个界面
function ViewManager:UnRegisterView(view_name)
	self.view_list[view_name] = nil
end

-- 获取一个界面
function ViewManager:GetView(view_name)
	return self.view_list[view_name]
end

-- 界面是否打开
function ViewManager:IsOpen(view_name)
	if nil == self.view_list[view_name] then
		return false
	end

	return self.view_list[view_name]:IsOpen()
end

-- 打开界面
local now_view = nil
function ViewManager:Open(view_name)
	now_view = self.view_list[view_name]
	if nil ~= now_view then
        now_view:Open()
	end
end

-- 关闭界面
function ViewManager:Close(view_name, ...)
	now_view = self.view_list[view_name]
	if nil ~= now_view then
		now_view:Close(...)
	end
end

-- 关闭所有界面
function ViewManager:CloseAll()
	for k,v in pairs(self.view_list) do
		if v:CanActiveClose() then
			if v:IsOpen() then
				v:Close()
			end
		end
	end
end

-- 刷新界面
function ViewManager:FlushView(view_name, ...)
	now_view = self.view_list[view_name]
	if nil ~= now_view then
		now_view:Flush(...)
	end
end

-- 获得UI节点
function ViewManager:GetUiNode(view_name, node_name)
	now_view = self.view_list[view_name]
	if nil ~= now_view then
		return now_view:OnGetUiNode(node_name)
	end
	return nil
end

function ViewManager:AddOpenView(view)

end

function ViewManager:RemoveOpenView(view, ignore)

end
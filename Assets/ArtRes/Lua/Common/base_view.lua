BaseView = BaseView or BaseClass()

CloseMode = {
	CloseVisible = 1,			-- 隐藏
	CloseDestroy = 2,			-- 延时销毁
}

UiLayer = {
	Normal = 0,					-- 普通界面
	Pop = 1,					-- 弹出框
}

ViewCacheTime = {
	LEAST = 5,
	NORMAL = 60,
	MOST = 3000,
}

GmAdapter = false

local UIRoot = GameObject.Find("GameRoot/UILayer").transform

function BaseView:__init(view_name)
	self.close_mode = CloseMode.CloseDestroy				-- 默认关闭后会销毁
	self.view_layer = UiLayer.Normal

	self.ui_config = nil									-- {bundle_name, prefab_name}
	self.root_node = nil									-- UI根节点

	self.is_loading = false									-- 是否加载中
	self.is_open = false									-- 是否已打开
	self.is_rendering = false								-- 是否渲染
	self.is_real_open = false								-- 是否已打开

	if nil ~= view_name and "" ~= view_name then
		self.view_name = view_name							-- 界面名字 在view_def.lua中定义
		ViewManager.Instance:RegisterView(self, view_name)
	end
end

function BaseView:__delete()
	self:Release()
end

function BaseView:Release()
	self.is_loading = false
	if nil == self.root_node then
		return
	end

	self:ReleaseCallBack()

	GameObject.Destroy(self.root_node)
	self.root_node = nil

	self.is_open = false
	self.is_rendering = false
	self.is_real_open = false
end

function BaseView:Load()
	if nil == self.ui_config or self:IsLoaded() or self.is_loading then
		return
	end

	self.is_loading = true

	local obj = ABManager:LoadRes(self.ui_config[1], self.ui_config[2], typeof(GameObject))
    self:PrefabLoadCallback(obj)
end

function BaseView:PrefabLoadCallback(obj)
	if nil == obj or not self.is_loading then
		self.is_loading = false
		return
	end

	obj.name = string.gsub(obj.name, "%(Clone%)", "")

	self.is_loading = false

	self.root_node = obj

	local transform = self.root_node.transform
	transform:SetParent(UIRoot, false)

	self:LoadCallBack(0, 1)

	if self:IsOpen() then
		self:OpenCallBack()
	else
		self:SetActive(false, true)
	end
end

function BaseView:Open()
	self.is_real_open = true
	if not self.is_open then
		self:SetActive(true)

		if not self:IsLoaded() then
			self:Load()
		else
			if nil ~= self.root_node then
				self:OpenCallBack()
			end
		end
	end
end

function BaseView:Close(...)
	self.is_real_open = false
	if not self.is_open then
		self:CloseDestroy()
		return
	end
	self:CloseCallBack(...)

    if self.close_mode == CloseMode.CloseVisible then
        self:CloseVisible()
    elseif self.close_mode == CloseMode.CloseDestroy then
        self:CloseDestroy()
    end
end

function BaseView:CloseVisible()
	self.is_real_open = false
	if self:IsOpen() then
		ViewManager.Instance:RemoveOpenView(self)
	end
	self:SetActive(false)
end

function BaseView:CloseDestroy()
	self:CloseVisible()
	self:Release()
end

function BaseView:SetActive(active, force)
	if self.is_open ~= active or force then
		self.is_open = active
		self.is_rendering = active
		if nil ~= self.root_node then
			self.root_node:SetActive(active)
		end
	end
end

function BaseView:GetLayer()
	return self.view_layer
end

function BaseView:IsOpen()
	return self.is_open
end

function BaseView:IsRendering()
	return self.is_rendering
end

function BaseView:IsLoaded()
	return nil ~= self.root_node
end

function BaseView:GetRootNode()
	return self.root_node
end

function BaseView:GetViewName()
	return self.view_name or ""
end

----------------------------------------------------
-- 继承 begin
----------------------------------------------------
-- 创建完调用
function BaseView:LoadCallBack()
	-- override
end

-- 打开后调用
function BaseView:OpenCallBack()
	-- override
end

-- 关闭前调用
function BaseView:CloseCallBack()
	-- override
end

-- 销毁前调用
function BaseView:ReleaseCallBack()
	-- override
end

-- 刷新
function BaseView:OnFlush(param_list)
	-- override
end
----------------------------------------------------
-- 继承 end
----------------------------------------------------
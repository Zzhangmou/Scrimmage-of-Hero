ControlView = ControlView or BaseClass(BaseView)

function ControlView:__init()
	self.ui_config = {"ui", "ControlPanel"}

end

function ControlView:__delete()

end

function ControlView:ReleaseCallBack()
    
end

function ControlView:LoadCallBack()
    self.root_node.transform:SetParent(TipCanvas, false)
end
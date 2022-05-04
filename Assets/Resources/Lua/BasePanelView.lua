--面板基类
Object:subClass("BasePanelView")

BasePanelView.panelObj = nil

function BasePanelView:Init(panelName,layer)
    self.panelObj = ABMgr:LoadRes("ui", panelName, typeof(GameObject))
    self.panelObj.transform:SetParent(layer, false)
    self:InitCallBack()
end

function BasePanelView:Open()
    self:OpenCallBack()
end

function BasePanelView:Close()
    self:CloseCallBack()
end

--region 子类重写
function BasePanelView:InitCallBack() 
end

function BasePanelView:OpenCallBack()
end

function BasePanelView:CloseCallBack()    
end
--endregion
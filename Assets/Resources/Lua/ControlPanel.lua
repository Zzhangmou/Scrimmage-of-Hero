ControlPanel = {}

ControlPanel.panelObj = nil

function ControlPanel:Init()
    --通用函数  解决调用C#泛型函数不支持类型(没有约束,有约束无参数,非class约束)
    --有使用限制(支持Mono打包   Il2cpp 只支持引用类型)
    --Il2cpp 如果泛型参数是值类型 除非C#那边已经调用过了 同类型的参数 lua中才能使用
    local Load = xlua.get_generic_method(CS.Common.ResourcesManager, "Load")
    local Load_R = Load(GameObject)
    self.panelObj = CS.UnityEngine.Object.Instantiate(Load_R("ControlPanel"))
    self.panelObj.transform:SetParent(Canvas, false)
end

function ControlPanel:Show()
    if self.panelObj == nil then
        self:Init()
    end
    self.panelObj:SetActive(true)
end

function ControlPanel:Close()
    self.panelObj:SetActive(false)
end
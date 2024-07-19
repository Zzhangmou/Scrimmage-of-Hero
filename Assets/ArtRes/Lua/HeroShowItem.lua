--字典存储英雄信息
HeroList = {}
--父类对象
HeroShowTargetPosition = GameObject.Find("HeroShowTargetPosition")

CurrectHero = nil

Object:subClass("HeroShowItem")
--"成员"
HeroShowItem.obj = nil

--函数
function HeroShowItem:Init(name, active)
    if HeroList[name] ~= nil then
        return
    end
    self.obj = ABManager:LoadRes("character", name, typeof(GameObject))
    --暂时只有一种
    local animatorController = ABManager:LoadRes("character", "Character_Show", typeof(RuntimeAnimatorController))
    self.obj:GetComponent(typeof(Animator)).runtimeAnimatorController = animatorController

    self.obj.transform:SetParent(HeroShowTargetPosition.transform, false)
    --改变可视层级
    self:ChangeLayer(self.obj.transform, 6)
    self.obj:SetActive(active)

    HeroList[name] = self.obj
    if active then
        CurrectHero = self.obj
    end
end

function HeroShowItem:Select(name)
    CurrectHero:SetActive(false)
    HeroList[name]:SetActive(true)
    CurrectHero = HeroList[name]
end

function HeroShowItem:ChangeLayer(trans,  targetLayer)
    trans.gameObject.layer = targetLayer;
    for _, v in pairs(trans) do
        self:ChangeLayer(v,targetLayer)
    end
end
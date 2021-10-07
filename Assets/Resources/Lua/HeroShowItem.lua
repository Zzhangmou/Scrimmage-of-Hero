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
    self.obj = ABMgr:LoadRes("character_show", name, typeof(GameObject))
    self.obj.transform:SetParent(HeroShowTargetPosition.transform, false)
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

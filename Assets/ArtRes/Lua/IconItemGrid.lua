--生成一个table 集成Object 通过继承方法subClass和new
IconItemGrid = IconItemGrid or BaseClass()

function IconItemGrid:__init()
    --"成员"
    self.obj = nil
    self.heroName = nil
    self.HeroImage = nil
    self.HeroNameText = nil
    self.HeroChoiceBtn = nil
end

function IconItemGrid:__delete()
    self.obj = nil
    self.heroName = nil
    self.HeroImage = nil
    self.HeroNameText = nil
    self.HeroChoiceBtn = nil
end


--函数
function IconItemGrid:Init(father) --实例化格子对象
    self.obj = ABManager:LoadRes("ui", "HeroItem", typeof(GameObject))
    self.obj.transform:SetParent(father, false)
    --获取组件
    self.HeroImage = self.obj.transform:Find("HeroImage"):GetComponent(typeof(Image))
    self.HeroChoiceBtn = self.obj.transform:Find("HeroImage"):GetComponent(typeof(Button))
    self.HeroNameText = self.obj.transform:Find("HeroNameText"):GetComponent(typeof(Text))
end

--初始化格子
function IconItemGrid:InitData(data)
    --设置
    local spriteAtlas = ABManager:LoadRes("ui", "CommonView", typeof(SpriteAtlas))
    self.HeroImage.sprite = spriteAtlas:GetSprite(data.icon)
    self.HeroNameText.text = data.name
    self.heroName = data.name
    --添加点击事件
    self.HeroChoiceBtn.onClick:AddListener(
        function()
            ShowMainData.Instance:SetHeroId(data.id)
            HeroShowCtrl.Instance:UpdateHeroDesc(data.id)
            self:SwitchHero()
        end
    )
end
--选择人物
function IconItemGrid:SwitchHero()
    print(self.heroName)
    HeroShowItem:Select(self.heroName)
end

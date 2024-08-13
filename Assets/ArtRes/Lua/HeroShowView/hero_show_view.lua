HeroShowView = HeroShowView or BaseClass(BaseView)

local L_EndPos = Vector2(280, 11)
local L_StartPos = Vector2(-280, 11)
local R_EndPos = Vector2(-280, -64.5)
local R_StartPos = Vector2(280, -64.5)
local moveTime = 0.6

function HeroShowView:__init()
    self.ui_config = {"ui", "HeroShowPanel"}

    self.heroImage = nil
    self.returnBtn = nil
    self.backBtn = nil
    self.selectBtn = nil
    self.heroName = nil
    self.heroDesc = nil
    self.skill1Desc = nil
    self.skill2Desc = nil
    self.damage_value = nil
    self.health_value = nil
    self.moveSpeed_value = nil
    -- Tween
    self.group_Left = nil
    self.group_Right = nil
end

function HeroShowView:__delete()

end

function HeroShowView:ReleaseCallBack()
    self.heroImage = nil
    self.returnBtn = nil
    self.backBtn = nil
    self.selectBtn = nil
    self.heroName = nil
    self.heroDesc = nil
    self.skill1Desc = nil
    self.skill2Desc = nil
    self.damage_value = nil
    self.health_value = nil
    self.moveSpeed_value = nil
    -- Tween
    self.group_Left = nil
    self.group_Right = nil
end

function HeroShowView:LoadCallBack()
    self.root_node.transform:SetParent(Canvas, false)
    -- 重新赋值RenderText
    self.heroImage = self.root_node.transform:Find("HeroRawImage"):GetComponent(typeof(RawImage))
    self.heroImage.texture = Resources.Load("Target")
    self.heroImage.gameObject:AddComponent(typeof(CS.Scrimmage.ObjTouchRotate))
    -- Btn
    self.returnBtn = self.root_node.transform:Find("ReturnButton"):GetComponent(typeof(Button))
    self.backBtn = self.root_node.transform:Find("BackButton"):GetComponent(typeof(Button))
    self.selectBtn = self.root_node.transform:Find("SelectButton"):GetComponent(typeof(Button))
    -- Text
    self.heroName = self.root_node.transform:Find("Group_Left/HeroName"):GetComponent(typeof(Text))
    self.heroDesc = self.root_node.transform:Find("Group_Left/HeroDesc"):GetComponent(typeof(Text))
    self.skill1Desc = self.root_node.transform:Find("Group_Right/Skill1Desc"):GetComponent(typeof(Text))
    self.skill2Desc = self.root_node.transform:Find("Group_Right/Skill2Desc"):GetComponent(typeof(Text))
    self.damage_value = self.root_node.transform:Find("Group_Right/Ability/Damage/Text_Value")
        :GetComponent(typeof(Text))
    self.health_value = self.root_node.transform:Find("Group_Right/Ability/Health/Text_Value")
        :GetComponent(typeof(Text))
    self.moveSpeed_value = self.root_node.transform:Find("Group_Right/Ability/MoveSpeed/Text_Value"):GetComponent(
        typeof(Text))
    -- Tween
    self.group_Left = self.root_node.transform:Find("Group_Left")
    self.group_Right = self.root_node.transform:Find("Group_Right")
    self.returnBtn.onClick:AddListener(function()
        self:Close()
    end)
    self.backBtn.onClick:AddListener(function()
        ViewManager.Instance:Close(ViewName.ChoiceHeroView)
        self:Close()
    end)
    self.selectBtn.onClick:AddListener(function()
        ViewManager.Instance:Close(ViewName.ChoiceHeroView)
        self:Close()
    end)
end

function HeroShowView:UpdateHeroDesc(heroId)
    self.heroName.text = HeroSkillDataList[heroId].normal.name
    self.skill1Desc.text = HeroSkillDataList[heroId].normal.description
    self.skill2Desc.text = HeroSkillDataList[heroId].special.description
    self.damage_value.text = HeroSkillDataList[heroId].normal.baseATK
    self.health_value.text = HeroSkillDataList[heroId].normal.maxHp
    self.moveSpeed_value.text = HeroSkillDataList[heroId].normal.moveSpeed
    self:TweenShow()
end

function HeroShowView:TweenShow()
    self.group_Left.transform.anchoredPosition = L_StartPos
    self.group_Left.transform:DOAnchorPos(L_EndPos, moveTime)
    self.group_Right.transform.anchoredPosition = R_StartPos
    self.group_Right.transform:DOAnchorPos(R_EndPos, moveTime)
end

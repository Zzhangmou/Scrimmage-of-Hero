-- 角色展示界面
HeroShowPanel = {}

HeroShowPanel.panelObj = nil

local L_EndPos = Vector2(280, 11)
local L_StartPos = Vector2(-280, 11)
local R_EndPos = Vector2(-280, -64.5)
local R_StartPos = Vector2(280, -64.5)
local moveTime = 0.6

function HeroShowPanel:Init()
    self.panelObj = ABManager:LoadRes("ui", "HeroShowPanel", typeof(GameObject))
    self.panelObj.transform:SetParent(Canvas, false)
    -- 重新赋值RenderText
    self.heroImage = self.panelObj.transform:Find("HeroRawImage"):GetComponent(typeof(RawImage))
    self.heroImage.texture = Resources.Load("Target")
    self.heroImage.gameObject:AddComponent(typeof(CS.Scrimmage.ObjTouchRotate))
    -- Btn
    self.returnBtn = self.panelObj.transform:Find("ReturnButton"):GetComponent(typeof(Button))
    self.backBtn = self.panelObj.transform:Find("BackButton"):GetComponent(typeof(Button))
    self.selectBtn = self.panelObj.transform:Find("SelectButton"):GetComponent(typeof(Button))
    -- Text
    self.heroName = self.panelObj.transform:Find("Group_Left/HeroName"):GetComponent(typeof(Text))
    self.heroDesc = self.panelObj.transform:Find("Group_Left/HeroDesc"):GetComponent(typeof(Text))
    self.skill1Desc = self.panelObj.transform:Find("Group_Right/Skill1Desc"):GetComponent(typeof(Text))
    self.skill2Desc = self.panelObj.transform:Find("Group_Right/Skill2Desc"):GetComponent(typeof(Text))
    self.damage_value = self.panelObj.transform:Find("Group_Right/Ability/Damage/Text_Value"):GetComponent(typeof(Text))
    self.health_value = self.panelObj.transform:Find("Group_Right/Ability/Health/Text_Value"):GetComponent(typeof(Text))
    self.moveSpeed_value = self.panelObj.transform:Find("Group_Right/Ability/MoveSpeed/Text_Value"):GetComponent(typeof(
        Text))
    -- Tween
    self.group_Left = self.panelObj.transform:Find("Group_Left")
    self.group_Right = self.panelObj.transform:Find("Group_Right")
    self.returnBtn.onClick:AddListener(function()
        self:Close()
    end)
    self.backBtn.onClick:AddListener(function()
        ChoiceHeroPanel:Close()
        self:Close()
    end)
    self.selectBtn.onClick:AddListener(function()
        ChoiceHeroPanel:Close()
        self:Close()
    end)
end
function HeroShowPanel:Show(heroId)
    if self.panelObj == nil then
        self:Init()
    end
    self.panelObj:SetActive(true)
    self.heroName.text = HeroSkillDataList[heroId].normal.name
    self.skill1Desc.text = HeroSkillDataList[heroId].normal.description
    self.skill2Desc.text = HeroSkillDataList[heroId].special.description
    self.damage_value.text = HeroSkillDataList[heroId].normal.baseATK
    self.health_value.text = HeroSkillDataList[heroId].normal.maxHp
    self.moveSpeed_value.text = HeroSkillDataList[heroId].normal.moveSpeed
    self:TweenShow()
end

function HeroShowPanel:Close()
    self.panelObj:SetActive(false)
end

function HeroShowPanel:TweenShow()
    self.group_Left.transform.anchoredPosition = L_StartPos
    self.group_Left.transform:DOAnchorPos(L_EndPos, moveTime)
    self.group_Right.transform.anchoredPosition = R_StartPos
    self.group_Right.transform:DOAnchorPos(R_EndPos, moveTime)
end

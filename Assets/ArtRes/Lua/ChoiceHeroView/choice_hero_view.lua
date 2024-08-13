ChoiceHeroView = ChoiceHeroView or BaseClass(BaseView)

function ChoiceHeroView:__init()
	self.ui_config = {"ui", "ChoiceHeroPanel"}

    self.showHeroRImage = nil
    self.Content = nil
    self.returnBtn = nil
    self.backBtn = nil
    self.list = nil
end

function ChoiceHeroView:__delete()

end

function ChoiceHeroView:ReleaseCallBack()
    self.showHeroRImage = nil
    self.Content = nil
    self.returnBtn = nil
    self.backBtn = nil
    self.list = nil
end

function ChoiceHeroView:LoadCallBack()
    self.root_node.transform:SetParent(Canvas, false)

    --获取组件
    self.returnBtn = self.root_node.transform:Find("ReturnButton"):GetComponent(typeof(Button))
    self.backBtn = self.root_node.transform:Find("BackButton"):GetComponent(typeof(Button))

    self.Content = self.root_node.transform:Find("ChoiceHeroView/Viewport/Content")

    self.list = RecyclingList.New()

    self:CreateHeroIcon()

    --添加事件
    self.returnBtn.onClick:AddListener(
        function()
            self:Close()
        end
    )
    self.backBtn.onClick:AddListener(
        function()
            self:Close()
        end
    )
end

function ChoiceHeroView:CreateHeroIcon()
    self.list:InitContentAndSVH(self.root_node.transform:Find("ChoiceHeroView"), IconItemGrid, HeroiconDataList)
end
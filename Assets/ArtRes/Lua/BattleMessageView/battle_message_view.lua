BattleMessageView = BattleMessageView or BaseClass(BaseView)

local spriteAtlas = ABManager:LoadRes("ui", "CommonView", typeof(SpriteAtlas))

function BattleMessageView:__init()
	self.ui_config = {"ui", "BattleMessagePanel"}

    self.dataList = {}
    self.index_red = 1
    self.index_blue = 1
end

function BattleMessageView:__delete()

end

function BattleMessageView:ReleaseCallBack()
    self.dataList = {}
    self.index_red = 1
    self.index_blue = 1
end

function BattleMessageView:LoadCallBack()
    self.root_node.transform:SetParent(TipCanvas, false)
    self.dataList.red = {}
    self.dataList.blue = {}
    for i = 1, 3 do
        local item_red = self.root_node.transform:Find("Red/showList/Item_".. i)
        self.dataList.red[i] = {}
        self.dataList.red[i].icon = item_red.transform:Find("icon")
        self.dataList.red[i].icon_dead = item_red.transform:Find("icon_dead")
        self.dataList.red[i].icon.gameObject:SetActive(false)
        self.dataList.red[i].icon_dead.gameObject:SetActive(false)

        self.dataList.blue[i] = {}
        local item_blue = self.root_node.transform:Find("Blue/showList/Item_".. i)
        self.dataList.blue[i].icon = item_blue.transform:Find("icon")
        self.dataList.blue[i].icon_dead = item_blue.transform:Find("icon_dead")
        self.dataList.blue[i].icon.gameObject:SetActive(false)
        self.dataList.blue[i].icon_dead.gameObject:SetActive(false)
    end
end

function BattleMessageView:InitBattleMessage(camp,heroId,id)
    if camp == 1 then
        self.dataList.red[self.index_red].icon.gameObject:SetActive(true)
        self.dataList.red[self.index_red].icon:GetComponent(typeof(Image)).sprite = spriteAtlas:GetSprite(HeroiconDataList[heroId].icon)
        self.dataList.red[self.index_red].id = id
        self.index_red = self.index_red + 1
    else
        self.dataList.blue[self.index_blue].icon.gameObject:SetActive(true)
        self.dataList.blue[self.index_blue].icon:GetComponent(typeof(Image)).sprite = spriteAtlas:GetSprite(HeroiconDataList[heroId].icon)
        self.dataList.blue[self.index_blue].id = id
        self.index_blue = self.index_blue + 1
    end
 end

 function BattleMessageView:FlushData(camp,id)
    if camp == 1 then
         for _, v in pairs(self.dataList.red) do
             if v.id == id then
                v.icon_dead.gameObject:SetActive(true)
             end
         end
    else
        for _, v in pairs(self.dataList.blue) do
            if v.id == id then
                v.icon_dead.gameObject:SetActive(true)
            end
        end
     end
 end
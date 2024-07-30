BattleMessagePanel = {}

BattleMessagePanel.panelObj = nil

BattleMessagePanel.dataList = {}
BattleMessagePanel.index_red = 1
BattleMessagePanel.index_blue = 1

local spriteAtlas = ABManager:LoadRes("ui", "CommonView", typeof(SpriteAtlas))

function BattleMessagePanel:Init()
    self.panelObj = ABManager:LoadRes("ui", "BattleMessagePanel", typeof(GameObject))
    self.panelObj.transform:SetParent(TipCanvas, false)
    self.panelObj:SetActive(false)
    self.dataList.red = {}
    self.dataList.blue = {}
    for i = 1, 3 do
        local item_red = self.panelObj.transform:Find("Red/showList/Item_".. i)
        self.dataList.red[i] = {}
        self.dataList.red[i].icon = item_red.transform:Find("icon")
        self.dataList.red[i].icon_dead = item_red.transform:Find("icon_dead")
        self.dataList.red[i].icon.gameObject:SetActive(false)
        self.dataList.red[i].icon_dead.gameObject:SetActive(false)

        self.dataList.blue[i] = {}
        local item_blue = self.panelObj.transform:Find("Blue/showList/Item_".. i)
        self.dataList.blue[i].icon = item_blue.transform:Find("icon")
        self.dataList.blue[i].icon_dead = item_blue.transform:Find("icon_dead")
        self.dataList.blue[i].icon.gameObject:SetActive(false)
        self.dataList.blue[i].icon_dead.gameObject:SetActive(false)

    end
end

function BattleMessagePanel:Show()
    if self.panelObj == nil then
        self:Init()
    end
    self.panelObj:SetActive(true)
end

function BattleMessagePanel:Close()
    self.panelObj:SetActive(false)
    self.index_red = 1
    self.index_blue = 1
end

function BattleMessagePanel:InitBattleMessage(camp,heroId,id)
    if self.panelObj == nil then
        self:Init()
    end
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

 function BattleMessagePanel:FlushData(camp,id)
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
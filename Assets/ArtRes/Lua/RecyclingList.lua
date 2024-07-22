-- 复用列表
Object:subClass("RecyclingList")

RecyclingList.list = nil
RecyclingList.scrollRect = nil
RecyclingList.content = nil
RecyclingList.layoutGroup = nil

RecyclingList.item = nil
RecyclingList.dataList = {}

RecyclingList.cellSize = {}
RecyclingList.padding = {}
RecyclingList.spacing = {}
RecyclingList.constraintCount = 0

RecyclingList.oldMinIndex = -1;
RecyclingList.oldMaxIndex = -1;

RecyclingList.nowShowItems = {}

--模拟对象池
RecyclingList.collectObjectList = {}


function RecyclingList:InitContentAndSVH(trans, item, dataList)
    self.list = trans:GetComponent(typeof(CS.RecyclingList))
    self.scrollRect = self.list.scrollRect
    self.content = self.list.content
    self.layoutGroup = self.list.layoutGroup

    self.cellSize = self.layoutGroup.cellSize
    self.padding = self.layoutGroup.padding
    self.spacing = self.layoutGroup.spacing
    self.constraintCount = self.layoutGroup.constraintCount
    self.item = item
    self.dataList = dataList

    self.scrollRect.content.sizeDelta = Vector2(0, math.floor(math.ceil(#dataList / self.constraintCount)) * (self.cellSize.y + self.spacing.y) + self.padding.top);

    self:CheckShowOrHide(true)

    -- xlua.hotfix(CS.RecyclingList, "CheckShowOrHide", function(self, show)
    --     self:CheckShowOrHide(show)
    -- end)

    self.scrollRect.onValueChanged:AddListener(function(normalisedPos)
        self:OnScrollChanged(normalisedPos)
    end)
end

function RecyclingList:OnScrollChanged(normalisedPos)
    self:CheckShowOrHide(false)
end


function RecyclingList:CheckShowOrHide(clearContents)
    if clearContents then
        self.scrollRect:StopMovement()
        self.verticalNormalizedPosition = 1
    end
    local viewPortH = self.scrollRect.viewport.rect.height;

    --求出索引下标
    local minIndex = math.floor(((self.scrollRect.content.anchoredPosition.y - self.padding.top) / (self.cellSize.y + self.spacing.y))) * self.constraintCount
    local maxIndex = math.floor(((self.scrollRect.content.anchoredPosition.y + viewPortH - self.padding.top) / (self.cellSize.y + self.spacing.y))) * self.constraintCount + self.constraintCount - 1

    if minIndex < 0 then
        minIndex = 0
    end
    if maxIndex >= #self.dataList  then
        maxIndex = #self.dataList - 1
    end
    --清理移出视野的格子
    if minIndex ~= self.oldMinIndex or maxIndex ~= self.oldMaxIndex then
        for i = self.oldMinIndex, minIndex - 1 do
            if self.nowShowItems[i] then
                self.nowShowItems[i].obj.gameObject:SetActive(false)
                table.insert(self.collectObjectList, self.nowShowItems[i])
                self.nowShowItems[i] = nil
            end
        end
        for i = maxIndex + 1, self.oldMaxIndex do
            if self.nowShowItems[i] then
                self.nowShowItems[i].obj.gameObject:SetActive(false)
                table.insert(self.collectObjectList, self.nowShowItems[i])
                self.nowShowItems[i] = nil
            end
        end
    end

    self.oldMinIndex = minIndex;
    self.oldMaxIndex = maxIndex;

    --生成新格子
    for i = minIndex, maxIndex do
        if(self.nowShowItems[i]) then

        else
                local pos = Vector3((i % self.constraintCount) * (self.cellSize.x + self.spacing.x) + self.cellSize.x / 2 + self.padding.left , -math.floor(i / self.constraintCount) * (self.cellSize.y + self.spacing.y) + -self.cellSize.y / 2 + -self.padding.top, 0);

                --创建格子
                local grid
                if #self.collectObjectList > 0 then
                    grid = self.collectObjectList[#self.collectObjectList]
                    self.collectObjectList[#self.collectObjectList].obj.gameObject:SetActive(true)
                    table.remove(self.collectObjectList, #self.collectObjectList)
                else
                    grid = IconItemGrid:new()
                    grid:Init(self.content)
                end
                grid:InitData(self.dataList[i+1])
                --加载人物模型  后续应该让各自格子自己执行相应更新逻辑
                local hero = HeroShowItem:new()
                hero:Init(self.dataList[i+1].name, false)
                
                local  go = grid.obj
                go.transform:SetParent(self.scrollRect.content)
                go.transform.localScale = Vector3.one
                go.transform.localPosition = pos

                self.nowShowItems[i] = grid
        end
    end
end
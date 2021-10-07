--将json数据读取到lua表中进行存储
--从ab包获取json表
local txt = ABMgr:LoadRes("json", "HeroObjectItem", typeof(TextAsset))
--获取文本信息进行json解析
local iconList = Json.decode(txt.text)

--转存信息
heroiconDataList = {}

for _, value in pairs(iconList) do
    heroiconDataList[value.id] = value
end
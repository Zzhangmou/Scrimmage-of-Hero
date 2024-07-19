--将json数据读取到lua表中进行存储
--从ab包获取json表
local txt = ABManager:LoadRes("json", "HeroObjectItem", typeof(TextAsset))
--获取文本信息进行json解析
local iconList = Json.decode(txt.text)

--转存信息
HeroiconDataList = {}

for _, value in pairs(iconList) do
    HeroiconDataList[value.id] = value
end


local skilltxt =  ABManager:LoadRes("json", "PlayerDataInfo", typeof(TextAsset))
local skillList = Json.decode(skilltxt.text)

HeroSkillDataList = {}

for _, value in pairs(skillList) do
    if not HeroSkillDataList[value.id] then
        HeroSkillDataList[value.id] = {}
        HeroSkillDataList[value.id].normal = value
    end
    if HeroSkillDataList[value.id] then
        HeroSkillDataList[value.id].special = value
    end
end
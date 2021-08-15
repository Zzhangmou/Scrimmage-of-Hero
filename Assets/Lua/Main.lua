require("InitClass")

--初始化道具表信息
require("HeroIconData")

--初始化人物
require("HeroShowItem")
--初始化界面人物
HeroShowItem:Init("Chemicalman", true)
--初始化格子对象
require("IconItemGrid")

require("GameMainPanel")
require("ChoiceHeroPanel")
require("StartShowPanel")
require("LoginPanel")
require("RegisterPanel")

StartShowPanel:Show()
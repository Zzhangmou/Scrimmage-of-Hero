require("InitClass")

--初始化道具表信息
require("HeroIconData")

--初始化人物
require("HeroShowItem")
--初始化界面人物
HeroShowItem:Init("Chemicalman", true)
--初始化格子对象
require("IconItemGrid")
--游戏主界面
require("GameMainPanel")
--选择人物面板
require("ChoiceHeroPanel")
--游戏开始面板
require("StartShowPanel")
--登录面板
require("LoginPanel")
--注册面板
require("RegisterPanel")
--提示面板
require("TipPanel")
--匹配面板
require("MatchPanel")
--进度面板
require("ProgressPanel")
--控制面板
require("ControlPanel")
---结果面板
require("ResultPanel")

StartShowPanel:Show()
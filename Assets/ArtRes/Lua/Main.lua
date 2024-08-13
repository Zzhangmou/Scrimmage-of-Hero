require("InitClass")
local list = require("Common/require_list")
for _, v in ipairs(list) do
    require(v)
end

require("Common/modules_controller")
local module = ModulesController.New()
module.Instance:CreateGameModule()

-- 初始化界面人物
HeroShowItem:Init("Chemicalman", true)

StartShowCtrl.Instance:ShowPanel()
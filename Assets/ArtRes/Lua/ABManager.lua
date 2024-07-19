ABManager = {}

function ABManager:LoadRes(abName, panelName, type)
   local obj =  ABMgr:LoadRes(abName, panelName, type)
   if (type == typeof(GameObject)) then
        return GameObject.Instantiate(obj);
   end
   return obj
end
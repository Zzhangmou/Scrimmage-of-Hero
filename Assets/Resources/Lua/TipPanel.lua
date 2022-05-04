TipPanel = {}

TipPanel.panelObj = nil

TipPanel.contentText = nil
TipPanel.closeBtn = nil

function TipPanel:Init()

    self.panelObj = ABMgr:LoadRes("ui", "TipPanel", typeof(GameObject))
    self.panelObj.transform:SetParent(TipCanvas, false)
    self.tipObj = self.panelObj.transform:Find("TipArea")

    --Btn
    self.contentText = self.panelObj.transform:Find("TipArea/ContentText"):GetComponent(typeof(Text))
    self.closeBtn = self.panelObj.transform:Find("TipArea/CloseButton"):GetComponent(typeof(Button))
    self.okBtn = self.panelObj.transform:Find("TipArea/OKButton"):GetComponent(typeof(Button))

    self.closeBtn.onClick:AddListener(
        function()
            self:Close()
        end
    )
    self.okBtn.onClick:AddListener(
        function()
            self:Close()
        end
    )
end

function TipPanel:Show(showText)
    if self.panelObj == nil then
        self:Init()
    end
    self.panelObj:SetActive(true)
    self.contentText.text = showText
    self:TipInTween()
end

function TipPanel:Close()
    self:TipOutTween()
end

function TipPanel:TipInTween()
    self.tipObj.transform.localScale = Vector3.zero
    local time = 0.2
    self.tipObj.transform:DOScale(Vector3(1,1,1), time);
 end
 function TipPanel:TipOutTween()
     local time = 0.1
     local tween = self.tipObj.transform:DOScale(Vector3.zero, time);
     tween:OnComplete(
         function ()
             self.panelObj:SetActive(false) 
             self.tipObj.transform.localScale = Vector3(1,1,1)
         end
         )
  end
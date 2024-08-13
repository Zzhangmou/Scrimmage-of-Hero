TipView = TipView or BaseClass(BaseView)

function TipView:__init()
	self.ui_config = {"ui", "TipPanel"}

    self.tipObj = nil
    self.contentText = nil
    self.closeBtn = nil
    self.okBtn = nil
end

function TipView:__delete()

end

function TipView:ReleaseCallBack()
    self.tipObj = nil
    self.contentText = nil
    self.closeBtn = nil
    self.okBtn = nil
end

function TipView:LoadCallBack()
    self.root_node.transform:SetParent(TipCanvas, false)
    self.tipObj = self.root_node.transform:Find("TipArea")

    --Btn
    self.contentText = self.root_node.transform:Find("TipArea/ContentText"):GetComponent(typeof(Text))
    self.closeBtn = self.root_node.transform:Find("TipArea/CloseButton"):GetComponent(typeof(Button))
    self.okBtn = self.root_node.transform:Find("TipArea/OKButton"):GetComponent(typeof(Button))

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

function TipView:CloseCallBack()
    -- self:TipOutTween()
end

function TipView:ShowMessage(message)
    self.contentText.text = message
    self:TipInTween()
end

function TipView:TipInTween()
    self.tipObj.transform.localScale = Vector3.zero
    local time = 0.2
    self.tipObj.transform:DOScale(Vector3(1,1,1), time);
 end
 
 function TipView:TipOutTween()
     local time = 0.1
     local tween = self.tipObj.transform:DOScale(Vector3.zero, time);
     tween:OnComplete(
         function ()
             self.root_node:SetActive(false) 
             self.tipObj.transform.localScale = Vector3(1,1,1)
         end
         )
  end
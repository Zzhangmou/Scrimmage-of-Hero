--常用别名

--准备之前导入的脚本
--面向对象相关
require("Object")

--Unity相关
GameObject = CS.UnityEngine.GameObject
Resources = CS.UnityEngine.Resources
Transform = CS.UnityEngine.Transform
RectTransform = CS.UnityEngine.RectTransform
TextAsset = CS.UnityEngine.TextAsset
--图集对象类
SpriteAtlas = CS.UnityEngine.U2D.SpriteAtlas
RenderTexture=CS.UnityEngine.RenderTexture

Vector3 = CS.UnityEngine.Vector3
Vector2 = CS.UnityEngine.Vector2

--UI相关
UI = CS.UnityEngine.UI
RawImage=UI.RawImage
Image = UI.Image
Text = UI.Text
Button = UI.Button
Toggle = UI.Toggle
ScrollRect = UI.ScrollRect

Canvas = GameObject.Find("Canvas").transform
--自己写的C#脚本相关
ABMgr = CS.Common.AbManager.Instance
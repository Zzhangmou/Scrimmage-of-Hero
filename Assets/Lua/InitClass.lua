--常用别名

--准备之前导入的脚本
--面向对象相关
require("Object")
--字符串拆分
require("SplitTools")
--Json解析
Json = require("JsonUtility")

--Unity相关
GameObject = CS.UnityEngine.GameObject
Resources = CS.UnityEngine.Resources
Transform = CS.UnityEngine.Transform
RectTransform = CS.UnityEngine.RectTransform
TextAsset = CS.UnityEngine.TextAsset
--图集对象类
SpriteAtlas = CS.UnityEngine.U2D.SpriteAtlas
RenderTexture = CS.UnityEngine.RenderTexture

Vector3 = CS.UnityEngine.Vector3
Vector2 = CS.UnityEngine.Vector2

--UI相关
UI = CS.UnityEngine.UI
RawImage = UI.RawImage
Image = UI.Image
Text = UI.Text
Button = UI.Button
Toggle = UI.Toggle
ScrollRect = UI.ScrollRect
InputField = UI.InputField

Canvas = GameObject.Find("Canvas/PanelLayer").transform
TipCanvas = GameObject.Find("Canvas/TipLayer").transform
--自己写的C#脚本相关
ABMgr = CS.Common.AbManager.Instance

--协议
NetManager = CS.NetWorkFK.NetManager

MsgLogin = CS.proto.MsgLogin
MsgRegister = CS.proto.MsgRegister
MsgGetUserInfo = CS.proto.MsgGetUserInfo

LoginHelper = CS.Helper.LoginHelper
RegisterHelper = CS.Helper.RegisterHelper
GameMainHelper = CS.Helper.GameMainHelper

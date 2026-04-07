####  for your reference(github):  https://tedliou.com/unity/vcs-github/
####  事前注意事項
* Unity version: Unity 6.3 LTS
* Visible Meta Files： 確保 Edit > Project Settings > Control 是 Visible Meta Files
* Force Text： 確保 Project Settings > Editor > Asset Serialization 是 Force Text。
* Git LFS：確保安裝了 Git LFS(for 3D模型)
#### 版本控制的注意事項！
* 先pull再push
* 上傳禁忌： 絕對不可以只傳單一檔案。像如果你傳了 Player.fbx，一定要連同 Player.fbx.meta 一起勾選上傳。
#### 一些避免collision的小措施
* Prefab 化開發： 盡量不要在 Scene 裡直接新增東西
先做成prefab，在prefab 模式下修改，scene只用來擺放這些大 prefab
* 不要隨意搬移資料夾： 在unity裡搬資料夾會改動大量的 .meta 檔案。要搬移之前，請確保大家都已經把手上的東西push上來
* 增加新的tag或layer後，請立刻push ProjectSettings 資料夾，並提醒大家pull
#### 小小提醒！
* 座標參考： 美術在做空間prefab時，請先做一個 1.6m 的方塊當人體比例尺，避免 VR 視角進去後發現視角不對勁


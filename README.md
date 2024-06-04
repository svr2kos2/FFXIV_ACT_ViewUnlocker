# FFXIV_ACT_ViewUnlocker
Unlock fov and zoom range for FF14
FF14的视角解锁插件
在 [Release](https://github.com/svr2kos2/FFXIV_ACT_ViewUnlocker/releases) 里下载FFXIV_ACT_ViewUnlocker.dll后,在ACT里导入即可  

# 如果你想自己寻找Offset
首先需要知道的是我们要寻找并修改的缩放距离和fov的默认值和他们在内存中的结构  
|当前缩放|最小缩放距离|最大缩放距离|当前fov|视角拉近时fov|视角拉远时fov|  
|--|--|--|--|--|--|
|\*|1.50|20.00|\*|0.69|0.78|
|\*|0x3FC00000|0x41A00000|\*|0x3F30A3D7|0x3F47AE14|  

当我们把视角拉远之后就可以在内存里搜索array of byte   
00 00 A0 41 00 00 C0 3F 00 00 A0 41 14 AE 47 3F D7 A3 30 3F 14 AE 47 3F  
把得到的地址减去0x114既是这个数据结构的地址  
我们再在静态区搜索这个地址即可获得指向这个结构的指针地址  
该指针的地址既是我们要找的offset  

# FFXIV_ACT_ViewUnlocker

Unlock FOV and camera zoom range for FFXIV.

Download `FFXIV_ACT_ViewUnlocker.exe` from the [Releases](https://github.com/svr2kos2/FFXIV_ACT_ViewUnlocker/releases) page. You can run it directly, or import it into ACT.

## If you want to find the offset yourself

First, you need to know the default values for the zoom distance and FOV that we want to locate and modify, as well as how they are laid out in memory:

| Current Zoom | Min Zoom Distance | Max Zoom Distance | Current FOV | FOV (Zoomed In) | FOV (Zoomed Out) |
|--|--|--|--|--|--|
| * | 1.50 | 20.00 | * | 0.69 | 0.78 |
| * | 0x3FC00000 | 0x41A00000 | * | 0x3F30A3D7 | 0x3F47AE14 |

After zooming the camera out, search memory for the following array of bytes:

00 00 A0 41 00 00 C0 3F 00 00 A0 41 14 AE 47 3F D7 A3 30 3F 14 AE 47 3F

Subtract 0x124 from the address you find to get the base address of this data structure (for versions prior to 7.3, use 0x114).

Then, search this address in the static region to get a pointer that points to this structure. The address of that pointer is the offset we are looking for.

---

## 中文说明

FF14的视角解锁插件  
在 [Release](https://github.com/svr2kos2/FFXIV_ACT_ViewUnlocker/releases) 里下载FFXIV_ACT_ViewUnlocker.exe后, 可以直接运行也可以导入到ACT。

# 如果你想自己寻找Offset
首先需要知道的是我们要寻找并修改的缩放距离和fov的默认值和他们在内存中的结构  
|当前缩放|最小缩放距离|最大缩放距离|当前fov|视角拉近时fov|视角拉远时fov|  
|--|--|--|--|--|--|
|\*|1.50|20.00|\*|0.69|0.78|
|\*|0x3FC00000|0x41A00000|\*|0x3F30A3D7|0x3F47AE14|  

当我们把视角拉远之后就可以在内存里搜索array of byte   
00 00 A0 41 00 00 C0 3F 00 00 A0 41 14 AE 47 3F D7 A3 30 3F 14 AE 47 3F  
把得到的地址减去0x124既是这个数据结构的地址(7.3之前的版本为0x114)  
我们再在静态区搜索这个地址即可获得指向这个结构的指针地址  
该指针的地址既是我们要找的offset  

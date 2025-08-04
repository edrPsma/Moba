@echo off
setlocal enabledelayedexpansion

REM ===== 配置路径 =====
set "target_dir=C:\Users\Neon\Desktop\Moba\Protocol\proto"
set "executor_dir=C:\Users\Neon\Desktop\Moba\Protocol\tools\bin\protoc.exe"
set "output_dir=C:\Users\Neon\Desktop\Moba\Protocol\scripts"

REM ===== 创建输出目录 =====
if not exist "%output_dir%" mkdir "%output_dir%"

REM ===== 收集所有 proto 文件到变量 FILES =====
set "FILES="
for /r "%target_dir%" %%f in (*.proto) do (
    set FILES=!FILES! "%%f"
)

REM ===== 一次性编译所有 proto =====
echo 正在生成 C# 脚本...
"%executor_dir%" --proto_path="%target_dir%" --csharp_out="%output_dir%" !FILES!

echo 生成完成！
pause
@echo off
setlocal

REM 设置你的目标目录
set "target_dir=C:\Users\Neon\Desktop\Moba\Protocol\proto"
set "excutor_dir=C:\Users\Neon\Desktop\Moba\Protocol\tools\bin\protoc.exe"
set "output_dir=C:\Users\Neon\Desktop\Moba\Protocol\scripts"

REM 列出所有 .proto 文件
for %%f in ("%target_dir%\*.proto") do (
    echo 生成 %%f
    call "%excutor_dir%" --proto_path="%target_dir%" --csharp_out="%output_dir%" "%%f"
)

pause

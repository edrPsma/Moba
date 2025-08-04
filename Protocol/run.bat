@echo off
setlocal enabledelayedexpansion

REM ===== ����·�� =====
set "target_dir=C:\Users\Neon\Desktop\Moba\Protocol\proto"
set "executor_dir=C:\Users\Neon\Desktop\Moba\Protocol\tools\bin\protoc.exe"
set "output_dir=C:\Users\Neon\Desktop\Moba\Protocol\scripts"

REM ===== �������Ŀ¼ =====
if not exist "%output_dir%" mkdir "%output_dir%"

REM ===== �ռ����� proto �ļ������� FILES =====
set "FILES="
for /r "%target_dir%" %%f in (*.proto) do (
    set FILES=!FILES! "%%f"
)

REM ===== һ���Ա������� proto =====
echo �������� C# �ű�...
"%executor_dir%" --proto_path="%target_dir%" --csharp_out="%output_dir%" !FILES!

echo ������ɣ�
pause
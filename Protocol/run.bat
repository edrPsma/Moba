@echo off
setlocal

REM �������Ŀ��Ŀ¼
set "target_dir=C:\Users\Neon\Desktop\Moba\Protocol\proto"
set "excutor_dir=C:\Users\Neon\Desktop\Moba\Protocol\tools\bin\protoc.exe"
set "output_dir=C:\Users\Neon\Desktop\Moba\Protocol\scripts"

REM �г����� .proto �ļ�
for %%f in ("%target_dir%\*.proto") do (
    echo ���� %%f
    call "%excutor_dir%" --proto_path="%target_dir%" --csharp_out="%output_dir%" "%%f"
)

pause

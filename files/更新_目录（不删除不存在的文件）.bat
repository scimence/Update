
#REM 从配置MD5.txt中获取文件信息，下载“files/”开始的所有文件到本地目录 “E:\tmp2\files2\”
#REM 未配置在MD5.txt中，仅存在于“E:\tmp2\files2\”中的文件不会被删除。共有的文件会更新。

#REM call "%~dp0Update.exe"  "[CONFIG]https://scimence.gitee.io/update/MD5.txt"  "E:\tmp2\\" "files/->files2/" false

call "%~dp0Update.exe"  "[CONFIG]https://scimence.gitee.io/update/MD5.txt"  "%~dp0目录keep\\" "files/->files2/" false

pause
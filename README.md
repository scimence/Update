# Update
文件和目录批量更新

Update.exe下载：
https://scimence.gitee.io/update/files/Update.exe

-----------------------------

###1、更新_文件.bat

REM 从给定地址下载文件到本地目录 “files/Update2.exe”

"%~dp0Update.exe" https://scimence.gitee.io/update/files/Update.exe "%~dp0files/Update2.exe"

-----------------------------

###2、更新_所有文件.bat

REM 更新配置在MD5.txt中所有文件，下载“files/”到目录“tmpALL\”

call "%~dp0Update.exe" "[CONFIG]https://scimence.gitee.io/update/MD5.txt" "%~dp0tmpALL\\"

-----------------------------

###3、更新_目录（删除不存在的文件）.bat

REM 从配置MD5.txt中获取文件信息，下载“files/”开始的所有文件到本地目录 “E:\tmp2\files2\”
REM 未配置在MD5.txt中，仅存在于“E:\tmp2\files2\”中的文件会被删除。共有的文件会更新。

REM call "%~dp0Update.exe"  "[CONFIG]https://scimence.gitee.io/update/MD5.txt"  "E:\tmp2\\"   "files/->files2/"

call "%~dp0Update.exe"  "[CONFIG]https://scimence.gitee.io/update/MD5.txt"  "%~dp0目录del\\"   "files/->files3/"

-----------------------------

###4、更新_目录（不删除不存在的文件）.bat

REM 从配置MD5.txt中获取文件信息，下载“files/”开始的所有文件到本地目录 “E:\tmp2\files2\”
REM 未配置在MD5.txt中，仅存在于“E:\tmp2\files2\”中的文件不会被删除。共有的文件会更新。

REM call "%~dp0Update.exe"  "[CONFIG]https://scimence.gitee.io/update/MD5.txt"  "E:\tmp2\\" "files/->files2/" false

call "%~dp0Update.exe"  "[CONFIG]https://scimence.gitee.io/update/MD5.txt"  "%~dp0目录keep\\" "files/->files2/" false

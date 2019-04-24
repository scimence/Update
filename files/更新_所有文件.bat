
#REM 更新配置在MD5.txt中所有文件，下载“files/”到目录“tmpALL\”

call "%~dp0Update.exe" "[CONFIG]https://scimence.gitee.io/update/MD5.txt" "%~dp0tmpALL\\"

pause
:: Pull tML
cd ..
mkdir tmod
cd tmod
curl https://github.com/tModLoader/tModLoader/releases/latest/download/tModLoader.zip -L --output tModLoader.zip
unzip tModLoader.zip
cd ..

:: Create tModLoader.targets
cd ..
echo ^<Project ToolsVersion="14.0" xmlns="http://schemas.microsoft.com/developer/msbuild/2003"^>^<Import Project="../../tmod/tMLMod.targets" /^>^</Project^> > tModLoader.targets

# Этот скрипт конфигурирует IPS SDK


# переменные окружения, используемые в IPS SDK
# TODO: укажите пути к каталогам сервера приложений и клиента IPS
$env:IPS_VERSION = 8
$env:IPS_SERVER_DIR = Resolve-Path "C:\Program Files\IPS\Server"
$env:IPS_CLIENT_DIR = Resolve-Path "C:\Program Files\IPS\Client"
$env:DEVENV_DIR = Resolve-Path "C:\Program Files\Microsoft Visual Studio\2022\Community\Common7\IDE"

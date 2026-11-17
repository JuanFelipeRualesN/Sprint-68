#!/bin/bash
# Arranca la API del agente conectada a MySQL (SOLO LECTURA) pidiendo las credenciales por teclado.
#  · La clave NO se muestra, NO se guarda en archivos y NO queda en el historial: vive solo en las variables
#    de entorno de este proceso mientras la API corre.
#  · Requiere la VPN activa (la base está detrás de un private endpoint).
#  · La API abre cada sesión en READ ONLY y se niega a arrancar si no lo logra, pero aun así conviene
#    usar la cuenta con menos permisos que tengas.
# Uso:  ./correr_api_mysql.sh            (deja la API en http://127.0.0.1:5099; Ctrl+C para detenerla)
set -euo pipefail
cd "$(dirname "$0")/EyaInsightsApi"

# Valores por defecto (no son secretos): basta con pulsar Enter en cada pregunta y escribir solo la clave.
POR_DEFECTO_HOST="mysql-msj-prod-eastus-001.mysql.database.azure.com"
POR_DEFECTO_USUARIO="admin_user_prod_msj"

read -r -p "Host [${POR_DEFECTO_HOST}]: " EYA_MYSQL_HOST;      EYA_MYSQL_HOST=${EYA_MYSQL_HOST:-$POR_DEFECTO_HOST}
read -r -p "Puerto [3306]: " EYA_MYSQL_PORT;                   EYA_MYSQL_PORT=${EYA_MYSQL_PORT:-3306}
read -r -p "Usuario [${POR_DEFECTO_USUARIO}]: " EYA_MYSQL_USER; EYA_MYSQL_USER=${EYA_MYSQL_USER:-$POR_DEFECTO_USUARIO}
read -r -s -p "Clave (no se muestra): " EYA_MYSQL_PASSWORD; echo
echo "Conectando a ${EYA_MYSQL_HOST}:${EYA_MYSQL_PORT} como ${EYA_MYSQL_USER}..."
export EYA_MYSQL_HOST EYA_MYSQL_PORT EYA_MYSQL_USER EYA_MYSQL_PASSWORD

export Datos__Origen=mysql

# Asistente del panel junto a Power BI (POST /asistente/preguntar): SOLO para desarrollo local.
#  · Foundry se autentica con tu sesión de `az login` (no hay clave que guardar).
#  · CORS limitado al front local (http://localhost:3000). Desplegado, el asistente debe ir detrás del backend de la plataforma.
export Asistente__Habilitado=true
export Asistente__Credencial=azcli
export Asistente__Origenes=http://localhost:3000
export ASPNETCORE_URLS=http://127.0.0.1:5099     # solo accesible desde esta máquina
export DOTNET_ROLL_FORWARD=Major                 # en esta máquina solo hay runtime 10
exec dotnet run --no-launch-profile

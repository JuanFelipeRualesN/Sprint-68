#!/bin/bash
# Arranca el sustituto LOCAL de embed-powerbi-info (http://127.0.0.1:5098) pidiendo las credenciales de Power BI por teclado.
#  · Las credenciales NO se muestran, NO se guardan en archivos y NO quedan en el historial: viven solo en las variables
#    de entorno de este proceso mientras corre (Ctrl+C lo detiene y las borra).
#  · Pega el VALOR del secreto (no el "ID del secreto"). Consíguelo por el canal autorizado del equipo; nunca en chats ni en git.
set -euo pipefail
cd "$(dirname "$0")/EmbedLocal"

read -r -s -p "Tenant ID (no se muestra): " POWERBI_TENANT_ID; echo
read -r -s -p "Client ID (no se muestra): " POWERBI_CLIENT_ID; echo
read -r -s -p "Client secret VALUE (no se muestra): " POWERBI_CLIENT_SECRET; echo
export POWERBI_TENANT_ID POWERBI_CLIENT_ID POWERBI_CLIENT_SECRET

export EMBED_ORIGEN=http://localhost:3000        # único origen permitido (el front local)
export ASPNETCORE_URLS=http://127.0.0.1:5098     # solo accesible desde esta máquina
export DOTNET_ROLL_FORWARD=Major                 # en esta máquina solo hay runtime 10
# Identidad RLS del token: por defecto la de administrador (ViewAll). Para un usuario admin-ore: EMBED_RLS_ROLE=Admin_ORE_Filter EMBED_RLS_USER=ORE
exec dotnet run --no-launch-profile

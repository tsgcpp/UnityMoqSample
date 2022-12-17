#!/bin/bash
set -eu

WORK_DIR="PluginGenerationToolOutput"
OBJ_DIR="${WORK_DIR}/obj"
META_FILES_DIR="./MetaFiles"

PLUGINS_DIR="Plugins/Moq"
DOTNET_STANDARD21_DIR="${WORK_DIR}/netstandard2.1"
DOTNET_STANDARD21_PLUGINS_DIR="${DOTNET_STANDARD21_DIR}/${PLUGINS_DIR}"
DOTNET_STANDARD21_LICENSE_FILE="./LICENSE_netstandard2.1.md"
DOTNET_FRAMEWORK_DIR="${WORK_DIR}/net462"
DOTNET_FRAMEWORK_PLUGINS_DIR="${DOTNET_FRAMEWORK_DIR}/${PLUGINS_DIR}"
DOTNET_FRAMEWORK_LICENSE_FILE="./LICENSE_net462.md"

MOQ_VERSION="4.18.3"
CASTLECORE_VERSION="5.1.0"
EVENTLOG_VERSION="7.0.0"
THREADINGTASKSEXTENSIONS_VERSION="4.5.4"
RUNTIMECOMPILERSERVICESUNSAFE_VERSION="6.0.0"

if [ ! -f "${META_FILES_DIR}/Moq.dll.meta" ]
then
  echo "[ERROR] \"${META_FILES_DIR}/Moq.dll.meta\" was not found."
  exit 1
fi

rm -rf "${WORK_DIR}"
mkdir -p "${WORK_DIR}"
mkdir -p "${OBJ_DIR}"

curl -sL "https://www.nuget.org/api/v2/package/Moq/${MOQ_VERSION}" > "${OBJ_DIR}/moq.nupkg"
unzip "${OBJ_DIR}/moq.nupkg" -d "${OBJ_DIR}/moq"

curl -sL "https://www.nuget.org/api/v2/package/Castle.Core/${CASTLECORE_VERSION}" > "${OBJ_DIR}/castle.core.nupkg"
unzip "${OBJ_DIR}/castle.core.nupkg" -d "${OBJ_DIR}/castle.core"

curl -sL "https://www.nuget.org/api/v2/package/System.Diagnostics.EventLog/${EVENTLOG_VERSION}" > "${OBJ_DIR}/system.diagnostics.eventlog.nupkg"
unzip "${OBJ_DIR}/system.diagnostics.eventlog.nupkg" -d "${OBJ_DIR}/system.diagnostics.eventlog"

curl -sL "https://www.nuget.org/api/v2/package/System.Threading.Tasks.Extensions/${THREADINGTASKSEXTENSIONS_VERSION}" > "${OBJ_DIR}/system.threading.tasks.extensions.nupkg"
unzip "${OBJ_DIR}/system.threading.tasks.extensions.nupkg" -d "${OBJ_DIR}/system.threading.tasks.extensions"

curl -sL "https://www.nuget.org/api/v2/package/System.Runtime.CompilerServices.Unsafe/${RUNTIMECOMPILERSERVICESUNSAFE_VERSION}" > "${OBJ_DIR}/system.runtime.compilerservices.unsafe.nupkg"
unzip "${OBJ_DIR}/system.runtime.compilerservices.unsafe.nupkg" -d "${OBJ_DIR}/system.runtime.compilerservices.unsafe"

echo "### .NET Standard 2.1 ###"
mkdir -p "${DOTNET_STANDARD21_PLUGINS_DIR}"
cp "${DOTNET_STANDARD21_LICENSE_FILE}" "${DOTNET_STANDARD21_DIR}"
cp \
  "${OBJ_DIR}/moq/lib/netstandard2.1/Moq.dll" \
  "${META_FILES_DIR}/Moq.dll.meta" \
  "${OBJ_DIR}/castle.core/lib/netstandard2.1/Castle.Core.dll" \
  "${META_FILES_DIR}/Castle.Core.dll.meta" \
  "${OBJ_DIR}/system.diagnostics.eventlog/lib/netstandard2.0/System.Diagnostics.EventLog.dll" \
  "${META_FILES_DIR}/System.Diagnostics.EventLog.dll.meta" \
  "${DOTNET_STANDARD21_PLUGINS_DIR}"
echo "[INFO] .NET Standard 2.1 => \"${DOTNET_STANDARD21_PLUGINS_DIR}\""

echo "### .NET Framework ###"
mkdir -p "${DOTNET_FRAMEWORK_PLUGINS_DIR}"
cp "${DOTNET_FRAMEWORK_LICENSE_FILE}" "${DOTNET_FRAMEWORK_DIR}"
cp \
  "${OBJ_DIR}/moq/lib/net462/Moq.dll" \
  "${META_FILES_DIR}/Moq.dll.meta" \
  "${OBJ_DIR}/castle.core/lib/net462/Castle.Core.dll" \
  "${META_FILES_DIR}/Castle.Core.dll.meta" \
  "${OBJ_DIR}/system.threading.tasks.extensions/lib/net461/System.Threading.Tasks.Extensions.dll" \
  "${META_FILES_DIR}/System.Threading.Tasks.Extensions.dll.meta" \
  "${OBJ_DIR}/system.runtime.compilerservices.unsafe/lib/net461/System.Runtime.CompilerServices.Unsafe.dll" \
  "${META_FILES_DIR}/System.Runtime.CompilerServices.Unsafe.dll.meta" \
  "${DOTNET_FRAMEWORK_PLUGINS_DIR}"
echo "[INFO] .NET Framework => \"${DOTNET_FRAMEWORK_PLUGINS_DIR}\""

rm -rf "${OBJ_DIR}"

echo "### Done ###"

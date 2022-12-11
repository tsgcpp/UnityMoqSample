#!/bin/bash
set -eu

WORK_DIR="PluginGenerationToolOutput"
OBJ_DIR="${WORK_DIR}/obj"

PLUGINS_DIR="Plugins/Moq"
DOTNET_STANDARD21="${WORK_DIR}/netstandard2.1/${PLUGINS_DIR}"
DOTNET_FRAMEWORK="${WORK_DIR}/net462/${PLUGINS_DIR}"

MOQ_VERSION="4.18.3"
CASTLECORE_VERSION="5.1.0"
EVENTLOG_VERSION="7.0.0"

rm -rf "${WORK_DIR}"
mkdir -p "${WORK_DIR}"
mkdir -p "${OBJ_DIR}"
mkdir -p "${DOTNET_STANDARD21}"
mkdir -p "${DOTNET_FRAMEWORK}"

curl -sL "https://www.nuget.org/api/v2/package/Moq/${MOQ_VERSION}" > "${OBJ_DIR}/moq.nupkg"
unzip "${OBJ_DIR}/moq.nupkg" -d "${OBJ_DIR}/moq"

curl -sL "https://www.nuget.org/api/v2/package/Castle.Core/${CASTLECORE_VERSION}" > "${OBJ_DIR}/castle.core.nupkg"
unzip "${OBJ_DIR}/castle.core.nupkg" -d "${OBJ_DIR}/castle.core"

curl -sL "https://www.nuget.org/api/v2/package/System.Diagnostics.EventLog/${EVENTLOG_VERSION}" > "${OBJ_DIR}/system.diagnostics.eventlog.nupkg"
unzip "${OBJ_DIR}/system.diagnostics.eventlog.nupkg" -d "${OBJ_DIR}/system.diagnostics.eventlog"

echo "### .NET Standard 2.1 ###"
cp \
  "${OBJ_DIR}/moq/lib/netstandard2.1/Moq.dll" \
  "MetaFiles/Moq.dll.meta" \
  "${OBJ_DIR}/castle.core/lib/netstandard2.1/Castle.Core.dll" \
  "MetaFiles/Castle.Core.dll.meta" \
  "${OBJ_DIR}/system.diagnostics.eventlog/lib/netstandard2.0/System.Diagnostics.EventLog.dll" \
  "MetaFiles/System.Diagnostics.EventLog.dll.meta" \
  "${DOTNET_STANDARD21}"
echo "[INFO] .NET Standard 2.1 => \"${DOTNET_STANDARD21}\""

echo "### .NET Framework ###"
cp \
  "${OBJ_DIR}/moq/lib/net462/Moq.dll" \
  "MetaFiles/Moq.dll.meta" \
  "${OBJ_DIR}/castle.core/lib/net462/Castle.Core.dll" \
  "MetaFiles/Castle.Core.dll.meta" \
  "${DOTNET_FRAMEWORK}"
echo "[INFO] .NET Framework => \"${DOTNET_FRAMEWORK}\""

rm -rf "${OBJ_DIR}"

echo "### Done ###"

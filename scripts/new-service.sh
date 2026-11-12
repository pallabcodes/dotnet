#!/usr/bin/env bash
# One-command rename of the scaffold to your service name.
# Usage: ./scripts/new-service.sh <service-name>    (run from the repo root, after cloning)
set -euo pipefail

name="${1:?Usage: ./scripts/new-service.sh <service-name>}"
[[ "$name" =~ ^[A-Za-z][A-Za-z0-9]*$ ]] || { echo "Name must be alphanumeric, no dashes/dots (C# identifier)."; exit 1; }
[[ -d Template ]] || { echo "Run from the repo root (must contain Template/)."; exit 1; }

# 1) Replace the word in all text files (code, csproj, sln, config, CI, README).
find . -path ./.git -prune -o -path ./bin -prune -o -path ./obj -prune -o -path ./scripts -prune -o -type f \
  \( -name '*.cs' -o -name '*.csproj' -o -name '*.sln' -o -name '*.json' -o -name '*.props' \
  -o -name 'Dockerfile' -o -name '.dockerignore' -o -name '*.yml' -o -name '*.md' -o -name '*.editorconfig' \) -print0 |
  xargs -0 sed -i.bak "s/Template/${name}/g"
find . -name '*.bak' -not -path './.git/*' -delete

# 2) Rename files and directories that carry the word.
find . -path ./.git -prune -o -depth -name '*Template*' -print0 |
  while IFS= read -r -d '' p; do
    mv "$p" "$(dirname "$p")/$(basename "$p" | sed "s/Template/${name}/g")"
  done

echo "Renamed to '${name}'. Verify with: dotnet build ${name}/${name}.sln"

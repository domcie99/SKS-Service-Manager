# Ścieżka do folderu, który chcesz usunąć
$TARGET_FOLDER = "[TARGETDIR]"

# Sprawdź, czy folder istnieje
if (Test-Path -Path $TARGET_FOLDER -PathType Container) {
    # Usuń folder i jego zawartość rekurencyjnie
    Remove-Item -Path $TARGET_FOLDER -Force -Recurse
}
else {
    Write-Host "Folder nie istnieje."
}

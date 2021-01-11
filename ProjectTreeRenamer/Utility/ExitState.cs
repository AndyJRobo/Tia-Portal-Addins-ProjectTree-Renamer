﻿namespace ProjectTreeRenamer.Utility
{
    public enum ExitState
    {
        IsChangeable,
        CouldNotExport,
        CouldNotCompile,
        Successful,
        CouldNotImport,
        BlockIsKnowHowProtected,
        ProgrammingLanguageNotSupported,
        XmlEditingError,
        IsLibraryType
    }
}

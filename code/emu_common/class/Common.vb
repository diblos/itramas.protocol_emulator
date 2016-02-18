'===================
'emu_common.common
'===================
Imports System.Net
Imports System.IO

Public Class Common
    Public Enum RooftopSignageColor
        Red = 0
        Green = 1
    End Enum

    Public Enum RooftopSignageEffect
        [Static] = 0
        [Blink] = 1
        [ScrollSpeed1] = 2
        [ScrollSpeed2] = 3
        [ScrollSpeed3] = 4
        [ScrollSpeed4] = 5
        [ScrollSpeed5] = 6
        [ScrollSpeed6] = 7
        [ScrollSpeed7] = 8
        [ScrollSpeed8] = 9
        [ScrollSpeed9] = 10
        [ScrollSpeed10] = 11
    End Enum

    Public Enum Acknowledgement
        Success = 0
        Fail = 1
        Wrong_Info = 2
        Not_Supported = 3
    End Enum
End Class
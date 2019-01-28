using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text;

namespace EynwaDiscordBot.Models.Enum
{
    [Flags]
    public enum Roles
    {
        Unknow,
        [EnumMember(Value = "Lucanes (Admin)")]
        Admin,
        [EnumMember(Value = "Goliathus (Modo)")]
        Modo,
        [EnumMember(Value = "Dynastinae (amis)")]
        Amis,
        [EnumMember(Value = "Oryctes (joueur)")]
        Joueur,
        [EnumMember(Value = "Bousiers")]
        Bousiers,
    }
}

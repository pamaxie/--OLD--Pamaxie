﻿using System.ComponentModel.DataAnnotations;

namespace Pamaxie.Database.Sql.DataClasses
{
    public class GamblingUrl
    {
        [Key] public long Id { get; set; }
        public string Url { get; set; }
    }
}
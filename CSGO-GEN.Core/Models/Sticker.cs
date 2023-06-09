﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSGO_GEN.Core.Models
{
    public class Sticker : IGenable, IBuffDetails
    {
        public string name { get; set; } = string.Empty;
        public int gen_id { get; set; }
        public string tournament { get; set; } = string.Empty;
        public string rarity { get; set; } = string.Empty;
        public int? BuffGoodsId { get; set; } = null;
        public int? BuffStickerId { get; set; } = null;
    }

    public class AppliedSticker : Sticker
    {
        public decimal Scratched { get; set; }

        public AppliedSticker(Sticker sticker)
        {
            name = sticker.name;
            gen_id = sticker.gen_id;
            tournament = sticker.tournament;
            rarity = sticker.rarity;
        }
    }
}
